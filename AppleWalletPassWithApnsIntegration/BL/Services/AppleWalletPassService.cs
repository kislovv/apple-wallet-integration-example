using System.Security.Cryptography.X509Certificates;
using Azure.Storage.Blobs;
using BL.Abstractions;
using BL.Configurations;
using BL.Dtos;
using BL.Exceptions;
using Microsoft.Extensions.Options;
using Passbook.Generator;
using Passbook.Generator.Fields;

namespace BL.Services;

public class AppleWalletPassService(
    IOptionsMonitor<AppleWalletPassConfig> appleWalletConfigOptions,
    ICardRepository cardRepository)
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
            
            var blobContainerClient = new BlobServiceClient(
                            new Uri(""))
                    .GetBlobContainerClient("images");

            const X509KeyStorageFlags flags = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;

            var request = new PassGeneratorRequest
            {
                    AppleWWDRCACertificate = new X509Certificate2(
                            Convert.FromBase64String(_appleWalletPassConfig.WWDRCertificateBase64)),
                    PassbookCertificate = new X509Certificate2(
                            Convert.FromBase64String(_appleWalletPassConfig.PassbookCertificateBase64),
                            _appleWalletPassConfig.PassbookPassword, flags)
            };
            
            var icon = await GetFile(partnerPassSpecific.IconPath, blobContainerClient);
            var logo = await GetFile(partnerPassSpecific.LogoPath, blobContainerClient);
            var strip = await GetFile(partnerPassSpecific.StripPath, blobContainerClient);

            request.Images.Add(PassbookImage.Icon, icon);
            request.Images.Add(PassbookImage.Logo, logo);
            request.Images.Add(PassbookImage.Logo2X, logo);
            request.Images.Add(PassbookImage.Logo3X, logo);
            request.Images.Add(PassbookImage.Icon2X, icon);
            request.Images.Add(PassbookImage.Icon3X, icon);
            request.Images.Add(PassbookImage.Strip, strip);

            //TODO: Вставить данные с PartnerSpecific
            request.PassTypeIdentifier = _appleWalletPassConfig.PassTypeIdentifier;
            request.BackgroundColor = partnerPassSpecific.BackgroundColor;
            request.TeamIdentifier = _appleWalletPassConfig.TeamIdentifier;
            request.SerialNumber = Guid.NewGuid().ToString();
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

            //TODO: Добавить конфиг для подставления узла из ngrok 
            request.WebServiceUrl = "";
            //TODO: Продумать Токен для подтверждения
            request.AuthenticationToken = "";

            return generator.Generate(request);
    }
    
    
    //TODO: Вынести в метод сервиса и само подключение к blob ранилищу тоже вынести в отдельный сервис
    async Task<byte[]> GetFile(string fileName, BlobContainerClient blobContainerClient)
    {
        var client = blobContainerClient.GetBlobClient(fileName);

        var blobDownloadInfoResponse = await client.DownloadAsync();

        using var memoryStream = new MemoryStream();
        await blobDownloadInfoResponse.Value.Content.CopyToAsync(memoryStream);
    
        return memoryStream.ToArray();
    }

}