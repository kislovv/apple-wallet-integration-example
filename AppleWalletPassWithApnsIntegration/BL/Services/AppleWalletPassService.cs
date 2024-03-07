using System.Security.Cryptography.X509Certificates;
using Azure.Storage.Blobs;
using BL.Abstractions;
using BL.Configurations;
using BL.Dtos;
using Microsoft.Extensions.Options;
using Passbook.Generator;
using Passbook.Generator.Fields;

namespace BL.Services;

public class AppleWalletPassService(
    IOptionsMonitor<AppleWalletPassConfig> appleWalletConfigOptions,
    ICardRepository cardRepository,
    IParticipantRepository participantRepository)
    : IPassService
{
    private readonly AppleWalletPassConfig _appleWalletPassConfig = appleWalletConfigOptions.CurrentValue;
    private readonly ICardRepository _cardRepository = cardRepository;
    private readonly IParticipantRepository _participantRepository = participantRepository;

    public async Task<byte[]> CreatePass(PassDto passDto)
    {
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
            request.Images.Add(PassbookImage.Strip, await GetFile("Intens.png", blobContainerClient));

            //TODO: Вставить данные с PartnerSpecific
            var icon = await GetFile("Intens APP Icon 1x.png", blobContainerClient);
            var icon2X = await GetFile("Intens APP Icon 2x.png", blobContainerClient);
            var icon3X = await GetFile("Intens APP Icon 3x.png", blobContainerClient);

            request.Images.Add(PassbookImage.Icon, icon);
            request.Images.Add(PassbookImage.Logo, icon);
            request.Images.Add(PassbookImage.Logo2X, icon2X);
            request.Images.Add(PassbookImage.Logo3X, icon3X);
            request.Images.Add(PassbookImage.Icon2X, icon2X);
            request.Images.Add(PassbookImage.Icon3X, icon3X);

            //TODO: Вставить данные с PartnerSpecific
            request.PassTypeIdentifier = _appleWalletPassConfig.PassTypeIdentifier;
            request.BackgroundColor = "#5bd1e1";
            request.LabelColor = "#000000";
            request.TeamIdentifier = _appleWalletPassConfig.TeamIdentifier;
            request.SerialNumber = Guid.NewGuid().ToString();
            request.SuppressStripShine = false;
            request.Description = "Интенс APP";
            request.OrganizationName = "DLS, OOO";
            request.LogoText = "Интенс APP";
            request.Style = PassStyle.StoreCard;
            request.AssociatedStoreIdentifiers = [1398198275];
            request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "01927847623423234234", "ISO-8859-1");

            //TODO: Добавить конфиг для подставления узла из ngrok 
            request.WebServiceUrl = "";
            //TODO: Продумать Токен для подтверждения
            request.AuthenticationToken = "";

            //TODO: Вставить данные из баланса Participant
            request.SecondaryFields.Add(new NumberField("balance", "Скидка", 0.6m,
                    FieldNumberStyle.PKNumberStyleDecimal));

            //TODO: подумать, может такой параметр можно где то хранить, мало ли будут разные вариации pass (a.k.a константы для отдельных partners)
            //или через enumtypeconverter в бд прям хранить доп параметром
            request.TransitType = TransitType.PKTransitTypeGeneric;


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