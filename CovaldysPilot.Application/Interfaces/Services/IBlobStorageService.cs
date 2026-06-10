namespace CovaldysPilot.Application.Interfaces.Services;

public interface IBlobStorageService
{
  Task<string> UploadAsync (Stream fileStream, string fileName, string contentType);
  Task DeleteAsync (string fileUrl);
}