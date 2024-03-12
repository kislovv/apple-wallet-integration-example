using Azure.Storage.Blobs;
using BL.Abstractions;
using BL.Configurations;
using Microsoft.Extensions.Options;

namespace BL.Services;

public class AzureBlobFileProvider: IFileProvider
{
    private readonly BlobContainerClient _containerClient;

    public AzureBlobFileProvider(IOptionsMonitor<FileProviderConfig> fileProviderConfigOptions)
    {
        var fileProviderConfig = fileProviderConfigOptions.CurrentValue;
        _containerClient = new BlobServiceClient(new Uri(fileProviderConfig.ImagesStoreUrl))
            .GetBlobContainerClient(fileProviderConfig.ImagesContainerName);
    }
    
    public async Task<byte[]> GetFileByPath(string path)
    {
        var client = _containerClient.GetBlobClient(path);

        var blobDownloadInfoResponse = await client.DownloadAsync();

        using var memoryStream = new MemoryStream();
        await blobDownloadInfoResponse.Value.Content.CopyToAsync(memoryStream);
    
        return memoryStream.ToArray();
    }
}