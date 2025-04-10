using Application.Dtos.ProductAggregate.Slide;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate
{
    public interface ISlideApplication
    {
        Task<OperationResult> Create(CreateSlide command);
        Task<OperationResult> Edit(EditSlide command);
        Task<OperationResult> Remove(long id);
        Task<OperationResult> Restore(long id);
        Task<EditSlide> GetDetails(long id);
        Task<List<SlideViewModel>> GetList();
        Task<List<SlideQueryModel>> GetSlides();
    }
}
