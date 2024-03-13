using System.Security.Cryptography.X509Certificates;
using System.Text;
using BL.Abstractions;
using BL.Configurations;
using BL.Dtos;
using BL.Exceptions;
using Microsoft.Extensions.Options;
using Passbook.Generator;
using Passbook.Generator.Fields;
using Pass = BL.Entities.Pass;

namespace BL.Services;

public class AppleWalletPassService(
    IOptionsMonitor<AppleWalletPassConfig> appleWalletConfigOptions,
    ICardRepository cardRepository, IFileProvider fileProvider)
    : IPassService
{
    private readonly AppleWalletPassConfig _appleWalletPassConfig = appleWalletConfigOptions.CurrentValue;

    public async Task<byte[]> CreatePass(PassDto passDto)
    { 
            var card = await cardRepository.GetCardWithPartnerAndPassSpecificByUserHashId(passDto.UserHashId);
            if (card.Partner.PartnerSpecific is null)
            {
                    throw new BusinessException("{NF} Not found Partner or Partner Specific!");
            }

            var partnerPassSpecific = card.Partner.PartnerSpecific;
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
            var serialNumber = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{card.Participant.Id} {passDto.Device}"));
            request.SerialNumber = serialNumber;
            request.SuppressStripShine = false;
            request.Description = partnerPassSpecific.Description;
            request.OrganizationName = _appleWalletPassConfig.OrganizationName;
            request.LogoText = _appleWalletPassConfig.OrganizationName;
            request.Style = PassStyle.StoreCard;
            request.AssociatedStoreIdentifiers = partnerPassSpecific.AssociatedStoreApps.Select(app => app.Id).ToList();
            
            request.AddBarcode(BarcodeType.PKBarcodeFormatQR, passDto.UserHashId, "ISO-8859-1");
            
            request.SecondaryFields.Add(new NumberField("balance", "Скидка", card.Participant.Balance,
                    FieldNumberStyle.PKNumberStyleDecimal));
            request.TransitType = TransitType.PKTransitTypeGeneric;
            
            request.WebServiceUrl = _appleWalletPassConfig.WebServiceUrl;
            request.AuthenticationToken = _appleWalletPassConfig.InstanceApiKey;

            card.Pass = new Pass
            {
                CardId = card.Id,
                LastUpdated = DateTimeOffset.Now,
                PassId = serialNumber
            };

            await cardRepository.UpdateCard(card);

            return generator.Generate(request);
    }
}