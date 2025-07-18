﻿using Framework.Application;

namespace Presentation.Helpers;
public class FileUploader(IWebHostEnvironment webHostEnvironment) : IFileUploader
{
    public async Task<string> Upload(IFormFile file, string path, CancellationToken cancellationToken = default)
    {
        if (file == null)
            return "";

        var directoryPath = $"{webHostEnvironment.WebRootPath}//ProductPictures//{path}";

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
        var filePath = $"{directoryPath}//{fileName}";
        using var output = File.Create(filePath);
        await file.CopyToAsync(output, cancellationToken);
        return $"{path}/{fileName}";
    }
}