using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.ProductAggregate;
public class ProductFeatureRepository(ApplicationDbContext context)
    : BaseRepository<ProductFeature>(context), IProductFeatureRepository
{ }