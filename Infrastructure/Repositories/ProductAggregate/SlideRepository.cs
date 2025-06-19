using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.ProductAggregate;
public class SlideRepository(ApplicationDbContext context)
    : BaseRepository<Slide>(context), ISlideRepository
{ }