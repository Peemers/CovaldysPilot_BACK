using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace CovaldysPilot.Infrastructure.Services;

public class BlobStorageService(IConfiguration configuration) : IBlobStorageService
{
  private readonly string _connectionString = configuration["AzureStorage:ConnectionString"]!;
  private readonly string _containerName = configuration["AzureStorage:ContainerName"]!;

  public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
  {
    var blobServiceClient = new BlobServiceClient(_connectionString);
    var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        
    // creation container si n'existe pas
    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        
    // Generation nom unique
    var uniqueFileName = $"{Guid.NewGuid()}-{fileName}";
    var blobClient = containerClient.GetBlobClient(uniqueFileName);
        
    // Upload
    await blobClient.UploadAsync(fileStream, new BlobHttpHeaders
    {
      ContentType = contentType
    });
        
    return blobClient.Uri.ToString();
  }

  public async Task DeleteAsync(string fileUrl)
  {
    var blobServiceClient = new BlobServiceClient(_connectionString);
    var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        
    // Extrait le nom du blob depuis l'URL
    var blobName = Path.GetFileName(new Uri(fileUrl).LocalPath);
    var blobClient = containerClient.GetBlobClient(blobName);
        
    await blobClient.DeleteIfExistsAsync();
  }
}