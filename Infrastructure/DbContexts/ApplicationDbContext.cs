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
                                  picture:"خشکبار/2025-04-13-22-38-49-AJ70.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"خشکبار"),

             new ProductCategory( id:2,
                                  name:"لوازم خانگی",
                                  description:"تست",
                                  picture:"لوازم خانگی/2025-04-13-22-52-35-Luxury-brand-of-home-appliances.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"لوازم خانگی"),

             new ProductCategory( id:3,
                                  name:"پوشاک",
                                  description:"تست",
                                  picture:"پوشاک/2025-04-13-22-52-56-WSZV3569-min.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"پوشاک"),

             new ProductCategory( id:4,
                                  name:"کالای دیجیتال",
                                  description:"تست",
                                  picture:"کالای دیجیتال/2025-04-13-22-53-06-کالای-دیجیتال.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"کالای دیجیتال"),

             new ProductCategory( id:5,
                                  name:"آرایشی بهداشتی",
                                  description:"تست",
                                  picture:"آرایشی بهداشتی/2025-04-13-22-54-28-cosmetic-formula-2.jpg",
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
                          picture :"خشکبار/زعفران/2025-04-13-22-57-04-1.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"زعفران",
                          keywords :"تست",
                          metaDescription :"تست"),

            new Product( id:2,
                          name :"پسته",
                          code :"238",
                          brand:"پاشا",
                          unitPrice :350000,
                          count:50,
                          shortDescription :"تست",
                          description :"تست",
                          picture :"خشکبار/زعفران/2025-04-13-22-58-26-پسته-اکبری-زعفرانی-فوق-لوکس-دستچین.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"زعفران",
                          keywords :"تست",
                          metaDescription :"تست"),

            new Product( id:3,
                          name :"چای ماسالا",
                          code :"462",
                          brand:"پاشا",
                          unitPrice :200000,
                          count:50,
                          shortDescription :"تست",
                          description :"تست",
                          picture :"خشکبار/زعفران/2025-04-13-22-58-45-Photoroom-20240718_141333_5.webp",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"زعفران",
                          keywords :"تست",
                          metaDescription :"تست"),

            new Product( id:4,
                          name :"قهوه",
                          code :"253",
                          brand:"پاشا",
                          unitPrice :600000,
                          count:50,
                          shortDescription :"تست",
                          description :"تست",
                          picture :"خشکبار/زعفران/2025-04-13-22-59-21-image1-31.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"زعفران",
                          keywords :"تست",
                          metaDescription :"تست"),

            new Product( id:5,
                          name :"اسپرسوساز",
                          code :"562",
                          brand:"پاشا",
                          unitPrice :5400000,
                          count:50,
                          shortDescription :"تست",
                          description :"تست",
                          picture :"خشکبار/زعفران/لوازم-خانگی/زعفران/2025-04-13-22-59-38-1724224224_78676.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"زعفران",
                          keywords :"تست",
                          metaDescription :"تست"),

            new Product( id:6,
                          name :"ادویه کاری",
                          code :"245",
                          brand:"پاشا",
                          unitPrice :40000,
                          count:50,
                          shortDescription :"تست",
                          description :"تست",
                          picture :"خشکبار//ادویه-کاری/2025-04-13-23-07-02-karii.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"زعفران",
                          keywords :"تست",
                          metaDescription :"تست"),

        });

            modelBuilder.Entity<Slide>().HasData(new List<Slide>()
            {
             new Slide(id:1,
                       picture:"slides/2025-04-13-22-54-56-nuts.jpg",
                       pictureAlt:"تست",
                       pictureTitle:"تست",
                       heading:"دانه قهوه اسپرسو جیورنو",
                       title:"یک طعم باورنکردنی",
                       text:"قطعا نام آشنای جیوتو را شنیده اید",
                       link:"تست",
                       btnText:"مشاهده فروشگاه"),

             new Slide(id:2,
                       picture:"slides/2025-04-13-22-55-04-bowl-mixed-nuts-dried-fruit_419341-140379.jpg",
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
