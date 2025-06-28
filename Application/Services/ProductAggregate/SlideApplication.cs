using Application.Dtos.ProductAggregate.Slide;
using Application.Interfaces.ProductAggregate;
using Domain.Contracts;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate;
public class SlideApplication(IFileUploader fileUploader,
                              IUnitOfWork unitOfWork,
                              ILogger<SlideApplication> logger)
    : ISlideApplication
{
    public async Task<OperationResult> Create(CreateSlide command, CancellationToken cancellationToken = default)
    {
        try
        {
            var pictureName = await fileUploader.Upload(command.Picture, "slides", cancellationToken);

            var slide = Slide.Create(pictureName, command.PictureAlt, command.PictureTitle, command.Heading, command.Title,
                                     command.Text, command.Link, command.BtnText);
            await unitOfWork.SlideRepository.Add(slide, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#SlideApplication.Create.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Edit(EditSlide command, CancellationToken cancellationToken = default)
    {
        try
        {
            var slide = await unitOfWork.SlideRepository.GetById(command.Id, cancellationToken);
            var pictureName = await fileUploader.Upload(command.Picture, "slides", cancellationToken);
            slide.Edit(pictureName, command.PictureAlt, command.PictureTitle, command.Heading, command.Title,
                       command.Text, command.Link, command.BtnText);
            await unitOfWork.SlideRepository.Update(slide);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#SlideApplication.Edit.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<EditSlide> GetDetails(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var slide = await unitOfWork.SlideRepository.GetById(id, cancellationToken);
            return new EditSlide
            {
                Id = slide.Id,
                BtnText = slide.BtnText,
                Heading = slide.Heading,
                PictureAlt = slide.PictureAlt,
                PictureTitle = slide.PictureTitle,
                Text = slide.Text,
                Link = slide.Link,
                Title = slide.Title
            };
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#SlideApplication.GetDetails.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<SlideViewModel>> GetList(CancellationToken cancellationToken = default)
    {
        try
        {
            var slides = await unitOfWork.SlideRepository.GetAllAsQueryable(cancellationToken);
            return await slides
                .Select(x => new SlideViewModel
                {
                    Id = x.Id,
                    Heading = x.Heading,
                    Picture = x.Picture,
                    Title = x.Title,
                    IsActive = x.IsActive,
                    CreationDate = x.CreateDateTime.ToFarsi()
                }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#SlideApplication.GetList.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<SlideQueryModel>> GetSlides(CancellationToken cancellationToken = default)
    {
        try
        {
            var slides = await unitOfWork.SlideRepository.GetAllAsQueryable(cancellationToken);
            return await slides
                    .Where(x => x.IsActive)
                    .Select(x => new SlideQueryModel
                    {
                        Picture = x.Picture,
                        PictureAlt = x.PictureAlt,
                        PictureTitle = x.PictureTitle,
                        BtnText = x.BtnText,
                        Heading = x.Heading,
                        Link = x.Link,
                        Text = x.Text,
                        Title = x.Title
                    }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#SlideApplication.GetSlides.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Remove(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var slide = await unitOfWork.SlideRepository.GetById(id, cancellationToken);
            slide.Remove();
            await unitOfWork.SlideRepository.Update(slide);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#SlideApplication.Remove.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Restore(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var slide = await unitOfWork.SlideRepository.GetById(id, cancellationToken);
            slide.Restore();
            await unitOfWork.SlideRepository.Update(slide);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#SlideApplication.Restore.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
}