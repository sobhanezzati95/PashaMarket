using Domain.Entities.DiscountAggregate;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;

namespace Domain.Contracts.Repositories.DiscountAggregate;
public class DiscountRepository(ApplicationDbContext context)
            : BaseRepository<Discount>(context), IDiscountRepository
{ }