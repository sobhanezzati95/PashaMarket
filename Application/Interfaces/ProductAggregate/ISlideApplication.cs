using Application.Dtos.ProductAggregate.Slide;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate;
public interface ISlideApplication
{
    Task<OperationResult> Create(CreateSlide command, CancellationToken cancellationToken = default);
    Task<OperationResult> Edit(EditSlide command, CancellationToken cancellationToken = default);
    Task<OperationResult> Remove(long id, CancellationToken cancellationToken = default);
    Task<OperationResult> Restore(long id, CancellationToken cancellationToken = default);
    Task<EditSlide> GetDetails(long id, CancellationToken cancellationToken = default);
    Task<List<SlideViewModel>> GetList(CancellationToken cancellationToken = default);
    Task<List<SlideQueryModel>> GetSlides(CancellationToken cancellationToken = default);
}