using Application.Dtos.ProductAggregate.Slide;
using Application.Interfaces.ProductAggregate;
using Domain;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate
{
    public class SlideApplication : ISlideApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SlideApplication> _logger;

        public SlideApplication(IFileUploader fileUploader, IUnitOfWork unitOfWork, ILogger<SlideApplication> logger)
        {
            _fileUploader = fileUploader;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperationResult> Create(CreateSlide command)
        {

            try
            {
                var pictureName = _fileUploader.Upload(command.Picture, "slides");

                var slide = Slide.Create(pictureName, command.PictureAlt, command.PictureTitle,
                    command.Heading, command.Title, command.Text, command.Link, command.BtnText);

                await _unitOfWork.SlideRepository.Add(slide);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#SlideApplication.Create.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> Edit(EditSlide command)
        {
            try
            {
                var slide = await _unitOfWork.SlideRepository.GetById(command.Id);
                if (slide == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                var pictureName = _fileUploader.Upload(command.Picture, "slides");

                slide.Edit(pictureName, command.PictureAlt, command.PictureTitle,
                    command.Heading, command.Title, command.Text, command.Link, command.BtnText);

                await _unitOfWork.SlideRepository.Update(slide);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#SlideApplication.Edit.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<EditSlide> GetDetails(long id)
        {
            try
            {
                var slide = await _unitOfWork.SlideRepository.GetById(id);

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
                _logger.LogError(e,
                "#SlideApplication.GetDetails.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<SlideViewModel>> GetList()
        {
            try
            {
                var slides = await _unitOfWork.SlideRepository.GetAllAsQueryable();
                return slides.Select(x => new SlideViewModel
                {
                    Id = x.Id,
                    Heading = x.Heading,
                    Picture = x.Picture,
                    Title = x.Title,
                    IsRemoved = x.IsRemoved,
                    CreationDate = x.CreateDateTime.ToFarsi()
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#SlideApplication.GetList.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> Remove(long id)
        {
            try
            {
                var slide = await _unitOfWork.SlideRepository.GetById(id);
                if (slide == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                slide.Remove();

                await _unitOfWork.SlideRepository.Update(slide);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#SlideApplication.Remove.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> Restore(long id)
        {
            try
            {
                var slide = await _unitOfWork.SlideRepository.GetById(id);
                if (slide == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                slide.Restore();

                await _unitOfWork.SlideRepository.Update(slide);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#SlideApplication.Restore.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
    }
}
