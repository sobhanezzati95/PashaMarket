using Domain.Entities.DiscountAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.Configurations.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region ProductAggregate

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<ProductFeatures> ProductFeatures { get; set; }


        #endregion

        #region DiscountAggregate
        public DbSet<Discount> Discounts { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region SeedData

            modelBuilder.Entity<ProductCategory>().HasData(new List<ProductCategory>()
            {
             new ProductCategory( id:1,
                                  name:"خشکبار",
                                  description:"تست",
                                  picture:"https://kashaninuts.com/wp-content/uploads/2021/02/AJ70.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"خشکبار"),

             new ProductCategory( id:2,
                                  name:"لوازم خانگی",
                                  description:"تست",
                                  picture:"https://kuppersbergiran.com/wp-content/uploads/2024/05/Luxury-brand-of-home-appliances.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"لوازم خانگی"),

             new ProductCategory( id:3,
                                  name:"پوشاک",
                                  description:"تست",
                                  picture:"https://pushakseven.ir/wp-content/uploads/2024/04/WSZV3569-min.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"پوشاک"),

             new ProductCategory( id:4,
                                  name:"کالای دیجیتال",
                                  description:"تست",
                                  picture:"https://peivast.com/wp-content/uploads/%DA%A9%D8%A7%D9%84%D8%A7%DB%8C-%D8%AF%DB%8C%D8%AC%DB%8C%D8%AA%D8%A7%D9%84.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"کالای دیجیتال"),

             new ProductCategory( id:5,
                                  name:"آرایشی بهداشتی",
                                  description:"تست",
                                  picture:"https://formolx.ir/wp-content/uploads/2018/02/cosmetic-formula-2.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"آرایشی بهداشتی"),
        });

            modelBuilder.Entity<Product>().HasData(new List<Product>()
            {
             new Product( id:1,
                          name :"زعفران",
                          code :"123",
                          brand:"پاشا",
                          unitPrice :120000,
                          count:50,
                          shortDescription :"تست",
                          description :"تست",
                          picture :"https://hitasaffron.shop/wp-content/uploads/2024/02/1.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"زعفران",
                          keywords :"تست",
                          metaDescription :"تست")

        });

            modelBuilder.Entity<Slide>().HasData(new List<Slide>()
            {
             new Slide(id:1,
                       picture:"https://images.healthshots.com/healthshots/en/uploads/2022/11/27124337/nuts.jpg",
                       pictureAlt:"تست",
                       pictureTitle:"تست",
                       heading:"دانه قهوه اسپرسو جیورنو",
                       title:"یک طعم باورنکردنی",
                       text:"قطعا نام آشنای جیوتو را شنیده اید",
                       link:"تست",
                       btnText:"مشاهده فروشگاه"),

             new Slide(id:2,
                       picture:"https://img.freepik.com/premium-photo/bowl-mixed-nuts-dried-fruit_419341-140379.jpg?w=1380",
                       pictureAlt:"تست",
                       pictureTitle:"تست",
                       heading:"دانه قهوه اسپرسو جیورنو",
                       title:"یک طعم باورنکردنی",
                       text:"قطعا نام آشنای جیوتو را شنیده اید",
                       link:"تست",
                       btnText:"مشاهده فروشگاه"),

        });

            #endregion

            var assembly = typeof(ProductCategoryConfiguration).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
