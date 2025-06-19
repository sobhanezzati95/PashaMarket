using Microsoft.AspNetCore.Http;

namespace Framework.Application;
public interface IFileUploader
{
    Task<string> Upload(IFormFile file, string path, CancellationToken cancellationToken = default);
}