using System.Security.Cryptography.X509Certificates;
using Azure.Storage.Blobs;
using DataAccess;
using Passbook.Generator;
using Passbook.Generator.Fields;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext(builder.Configuration);

var app = builder.Build();

app.MapPost("/pass/create", async () =>
{
    //TODO: Логику создания Pass вынести в отдельный сервис
    var generator = new PassGenerator();
    
var blobContainerClient = new BlobServiceClient(
            new Uri(""))
        .GetBlobContainerClient("images");

const X509KeyStorageFlags flags = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;

var request = new PassGeneratorRequest
{
    //TODO: заменить на Options
    AppleWWDRCACertificate = new X509Certificate2(
        Convert.FromBase64String(app.Configuration["appleWalletConfigurations:wWDRCertificateBase64"]!)),
    PassbookCertificate = new X509Certificate2(
        Convert.FromBase64String(app.Configuration["appleWalletConfigurations:passbookCertificateBase64"]!),
        app.Configuration["appleWalletConfigurations:passbookPassword"]!, flags)
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
request.PassTypeIdentifier = "pass.com.dls.applewalletpass";
request.BackgroundColor = "#5bd1e1";
request.LabelColor = "#000000";
request.TeamIdentifier = "4NZ29379K5";
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
request.SecondaryFields.Add(new NumberField("balance", "Скидка", 0.6m, FieldNumberStyle.PKNumberStyleDecimal));

//TODO: подумать, может такой параметр можно где то хранить, мало ли будут разные вариации pass (a.k.a константы для отдельных partners)
//или через enumtypeconverter в бд прям хранить доп параметром
request.TransitType = TransitType.PKTransitTypeGeneric;


var result = generator.Generate(request);

//TODO: Подумать над названием файла (возможно id или имя participant + pass)
return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
});

app.Run();

//TODO: Вынести в метод сервиса и само подключение к blob ранилищу тоже вынести в отдельный сервис
async Task<byte[]> GetFile(string fileName, BlobContainerClient blobContainerClient)
{
    var client = blobContainerClient.GetBlobClient(fileName);

    var blobDownloadInfoResponse = await client.DownloadAsync();

    using var memoryStream = new MemoryStream();
    await blobDownloadInfoResponse.Value.Content.CopyToAsync(memoryStream);
    
    return memoryStream.ToArray();
}