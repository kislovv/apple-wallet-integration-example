using System.Security.Cryptography.X509Certificates;
using System.Text;
using BL.Abstractions;
using BL.Configurations;
using BL.Dtos;
using BL.Exceptions;
using BL.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Passbook.Generator;
using Passbook.Generator.Fields;

namespace BL.Services;

public class AppleWalletPassService(
    IOptionsMonitor<AppleWalletPassConfig> appleWalletConfigOptions,
    ICardRepository cardRepository, 
    IFileProvider fileProvider, 
    IPassRepository passRepository,
    IUnitOfWork unitOfWork,
    IDevicesRepository devicesRepository,
    ILogger<AppleWalletPassService> logger,
    IPushService<UpdateAppleWalletPassMessageDto> pushService)
    : IPassService
{
    private readonly AppleWalletPassConfig _appleWalletPassConfig = appleWalletConfigOptions.CurrentValue;

    public async Task<byte[]> CreatePass(PassDto passDto)
    {
        var userHashId = passDto.UserHashId;
        var card = await cardRepository.GetCardWithPartnerAndPassSpecificByUserHashId(userHashId);
        if (card.Partner.PartnerSpecific is null)
        {
                throw new BusinessException("{NF} Not found Partner or Partner Specific!");
        }

        var partnerPassSpecific = card.Partner.PartnerSpecific;
        var serialNumber = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{card.Participant.Id}_{card.PartnerId}"));
        var balance = card.Participant.Balance;
        
        
        var result = await GeneratePass(partnerPassSpecific, serialNumber, userHashId, balance);
        var pass = await passRepository.GetPassBySerialNumber(serialNumber);
        if (pass is null)
        {
            
            _ = await passRepository.CreatePass(new AppleWalletPass
            {
                CardId = card.Id,
                LastUpdated = DateTimeOffset.Now.ToUniversalTime(),
                PassId = serialNumber
            });
        }
        else
        {
            pass.LastUpdated = DateTimeOffset.Now.ToUniversalTime();
            passRepository.UpdatePass(pass);
        }
        await unitOfWork.SaveChangesAsync();
        return result;
    }
    
    public async Task<byte[]> GetUpdatedPass(string serialNumber)
    {
        var pass = await passRepository.GetPassWithPartnerSpecificBySerialNumber(serialNumber);
        
        var partnerPassSpecific = pass.Card.Partner.PartnerSpecific;

        var result = await GeneratePass(
            pass.Card.Partner.PartnerSpecific!,
            serialNumber,
            pass.Card.UserHashId,
            pass.Card.Participant.Balance);

        pass.LastUpdated = DateTimeOffset.Now.ToUniversalTime();
        
        passRepository.UpdatePass(pass);

        await unitOfWork.SaveChangesAsync();
        
        return result;
    }

    public async Task UpdatePass(UpdatePassDto passMessageDto)
    {
        var card = await cardRepository.GetCardWithPassAndParticipantById(passMessageDto.CardId);

        if (card.AppleWalletPass is null)
        {
            throw new BusinessException($"Not found apple pass by card with id {passMessageDto.CardId}");
        }
        
        await pushService.PushMessage(new UpdateAppleWalletPassMessageDto
        {
            NewBalance = card.Participant.Balance,
            DevicesPushToken = card.AppleWalletPass.AppleDevices.Select(d => d.PushToken).ToArray()
        });
    }

    public async Task RegisterPass(RegisteredPassDto passDto)
    {
        var pass = await passRepository.GetPassBySerialNumber(passDto.SerialNumber);
        if (pass is null)
        {
            throw new BusinessException($"Pass with serial number {passDto.SerialNumber} not found!");
        }
        
        pass.AppleDevices.Add(new AppleDevice
        {
            Id = passDto.DeviceId,
            PushToken = passDto.PushToken
        });
        
        passRepository.UpdatePass(pass);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task UnregisterPass(UnregisterPassDto passDto)
    {
        var pass = await passRepository.GetPassBySerialNumber(passDto.SerialNumber);
        if (pass is null)
        {
            logger.LogInformation("Pass with serialNumber {SerialNumber} was already removed!", passDto.SerialNumber);
            return;
        }
        passRepository.Delete(pass);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task<LastUpdatedPassesDto?> GetLastUpdatedPasses(string deviceId, DateTimeOffset updatedBefore)
    {
        var lastUpdatedPasses = await passRepository.GetLastUpdatedPassesByDeviceId(
            deviceId, updatedBefore);

        if (lastUpdatedPasses.IsNullOrEmpty())
        {
            return default;
        }

        return new LastUpdatedPassesDto
        {
            SerialNumbers = lastUpdatedPasses.Select(lu => lu.PassId).ToList(),
            LastUpdated = lastUpdatedPasses[0].LastUpdated
        };
    }
    
    private async Task<byte[]> GeneratePass(AppleWalletPartnerSpecific partnerPassSpecific, string serialNumber, string userHashId,
        decimal balance)
    {
        var generator = new PassGenerator();
        
        const X509KeyStorageFlags flags = X509KeyStorageFlags.MachineKeySet | 
                                          X509KeyStorageFlags.Exportable;
        
        var request = new PassGeneratorRequest
        {
            AppleWWDRCACertificate = new X509Certificate2(
                Convert.FromBase64String(_appleWalletPassConfig.WWDRCertificateBase64)),
            PassbookCertificate = new X509Certificate2(
                Convert.FromBase64String(_appleWalletPassConfig.PassbookCertificateBase64),
                _appleWalletPassConfig.PassbookPassword, flags)
        };
        
        var icon = await fileProvider.GetFileByPath(partnerPassSpecific.IconPath);
        var logo = await fileProvider.GetFileByPath(partnerPassSpecific.LogoPath);
        var strip = await fileProvider.GetFileByPath(partnerPassSpecific.StripPath);

        request.Images.Add(PassbookImage.Icon, icon);
        request.Images.Add(PassbookImage.Logo, logo);
        request.Images.Add(PassbookImage.Logo2X, logo);
        request.Images.Add(PassbookImage.Logo3X, logo);
        request.Images.Add(PassbookImage.Icon2X, icon);
        request.Images.Add(PassbookImage.Icon3X, icon);
        request.Images.Add(PassbookImage.Strip, strip);
        
        request.PassTypeIdentifier = _appleWalletPassConfig.PassTypeIdentifier;
        request.BackgroundColor = partnerPassSpecific.BackgroundColor;
        request.TeamIdentifier = _appleWalletPassConfig.TeamIdentifier;
        request.SerialNumber = serialNumber;
        request.SuppressStripShine = false;
        request.Description = partnerPassSpecific.Description;
        request.OrganizationName = _appleWalletPassConfig.OrganizationName;
        request.LogoText = _appleWalletPassConfig.OrganizationName;
        request.Style = PassStyle.StoreCard;
        request.AssociatedStoreIdentifiers = partnerPassSpecific.AppleAssociatedStoreApps.Select(app => app.Id).ToList();
        
        request.AddBarcode(BarcodeType.PKBarcodeFormatQR, userHashId, "ISO-8859-1");
        
        request.SecondaryFields.Add(new NumberField("balance", "Скидка", balance,
            FieldNumberStyle.PKNumberStyleDecimal));
        request.TransitType = TransitType.PKTransitTypeGeneric;
        
        request.WebServiceUrl = _appleWalletPassConfig.WebServiceUrl;
        request.AuthenticationToken = _appleWalletPassConfig.InstanceApiKey;
        
        var result = generator.Generate(request);
        return result;
    }
}