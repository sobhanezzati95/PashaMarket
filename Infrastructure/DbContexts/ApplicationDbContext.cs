using Domain.Entities.DiscountAggregate;
using Domain.Entities.OrderAggregate;
using Domain.Entities.ProductAggregate;
using Domain.Entities.UserAggregate;
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
        public DbSet<ProductFeature> ProductFeatures { get; set; }


        #endregion

        #region DiscountAggregate
        public DbSet<Discount> Discounts { get; set; }

        #endregion

        #region UserAggregate

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region SeedData

            modelBuilder.Entity<ProductCategory>().HasData(new List<ProductCategory>()
            {
             new ProductCategory( id:1,
                                  name:"خشکبار",
                                  description:"تست",
                                  picture:"خشکبار/2025-04-17-20-18-35-2025-04-13-22-38-49-AJ70.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"خشکبار"),

             new ProductCategory( id:2,
                                  name:"لوازم خانگی",
                                  description:"تست",
                                  picture:"لوازم خانگی/2025-04-17-20-18-50-2025-04-13-22-52-35-Luxury-brand-of-home-appliances.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"لوازم خانگی"),

             new ProductCategory( id:3,
                                  name:"پوشاک",
                                  description:"تست",
                                  picture:"پوشاک/2025-04-17-20-19-05-2025-04-13-22-52-56-WSZV3569-min.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"پوشاک"),

             new ProductCategory( id:4,
                                  name:"کالای دیجیتال",
                                  description:"تست",
                                  picture:"کالای دیجیتال/2025-04-17-20-19-15-2025-04-13-22-53-06-کالای-دیجیتال.jpg",
                                  pictureAlt:"تست",
                                  pictureTitle:"تست",
                                  keywords:"تست",
                                  metaDescription:"تست",
                                  slug:"کالای دیجیتال"),

             new ProductCategory( id:5,
                                  name:"آرایشی بهداشتی",
                                  description:"تست",
                                  picture:"آرایشی بهداشتی/2025-04-17-20-19-24-2025-04-13-22-54-28-cosmetic-formula-2.jpg",
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
                          description :"تست",
                          picture :"خشکبار/زعفران/2025-04-17-20-13-47-2025-04-13-22-57-04-1.jpg",
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
                          description :"تست",
                          picture :"خشکبار/پسته/2025-04-17-20-16-31-2025-04-13-22-58-26-پسته-اکبری-زعفرانی-فوق-لوکس-دستچین.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"پسته",
                          keywords :"تست",
                          metaDescription :"تست"),

            new Product( id:3,
                          name :"چای ماسالا",
                          code :"462",
                          brand:"پاشا",
                          unitPrice :200000,
                          count:50,
                          description :"تست",
                          picture :"خشکبار/چای-ماسالا/2025-04-17-20-16-47-2025-04-13-22-58-45-Photoroom-20240718_141333_5.webp",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"چای ماسالا",
                          keywords :"تست",
                          metaDescription :"تست"),

            new Product( id:4,
                          name :"قهوه",
                          code :"253",
                          brand:"پاشا",
                          unitPrice :600000,
                          count:50,
                          description :"تست",
                          picture :"خشکبار/قهوه/2025-04-17-20-16-53-2025-04-13-22-59-21-image1-31.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"قهوه",
                          keywords :"تست",
                          metaDescription :"تست"),

            new Product( id:5,
                          name :"اسپرسوساز",
                          code :"562",
                          brand:"پاشا",
                          unitPrice :5400000,
                          count:50,
                          description :"تست",
                          picture :"لوازم خانگی/اسپرسوساز/2025-04-19-01-21-22-2025-04-13-22-59-38-1724224224_78676.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :2,
                          slug :"اسپرسوساز",
                          keywords :"تست",
                          metaDescription :"تست"),

            new Product( id:6,
                          name :"ادویه کاری",
                          code :"245",
                          brand:"پاشا",
                          unitPrice :40000,
                          count:50,
                          description :"تست",
                          picture :"خشکبار/ادویه-کاری/2025-04-17-20-17-16-2025-04-13-23-07-02-karii.jpg",
                          pictureAlt :"تست",
                          pictureTitle :"تست",
                          categoryId :1,
                          slug :"ادویه کاری",
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

            modelBuilder.Entity<ProductPicture>().HasData(new List<ProductPicture>()
            {
                new ProductPicture(id:1,
                                   productId:1,
                                   picture: "خشکبار//زعفران/2025-04-17-20-17-37-2025-04-17-18-40-35-220590f810820bce5d224bec3fbe1bf7c0c73b2d_1597927042.webp",
                                   pictureAlt:"تست",
                                   pictureTitle:  "تست"),

                new ProductPicture(id:2,
                                   productId:1,
                                   picture: "خشکبار//زعفران/2025-04-17-20-17-43-2025-04-17-18-40-48-d7430fdc7ec9a61ae3cd2b98e168249a93d05d19_1597926894.webp",
                                   pictureAlt:"تست",
                                   pictureTitle:  "تست"),

                new ProductPicture(id:3,
                                   productId:1,
                                   picture: "خشکبار//زعفران/2025-04-17-20-17-47-2025-04-17-18-41-00-bdb4bbe6644db924fa87941c2655af23a618f9c6_1597926969.webp",
                                   pictureAlt:"تست",
                                   pictureTitle:  "تست"),

            });

            modelBuilder.Entity<ProductFeature>().HasData(new List<ProductFeature>()
            {

                new ProductFeature(id:1,
                                   displayName:"وزن بسته‌بندی",
                                   value:"۱ گرم",
                                   productId:1),


                new ProductFeature(id:2,
                                   displayName:"ابعاد بسته‌بندی",
                                   value:"۴x۳x۸ سانتی‌متر",
                                   productId:1),


                new ProductFeature(id:3,
                                   displayName:"شماره پروانه بهداشت",
                                   value:"۵۰/۱۶۲۶-۱",
                                   productId:1),

                new ProductFeature(id:4,
                                   displayName:"نوع زعفران",
                                   value:"سرگل",
                                   productId:1),

            });

            modelBuilder.Entity<Role>().HasData(new List<Role>()
            {
                new Role(id:1,name:"Admin"),
                new Role(id:2,name:"SystemUser"),
            });
            #endregion

            modelBuilder.Entity<Product>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<Slide>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<ProductCategory>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<ProductPicture>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<ProductFeature>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<Role>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<Order>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<Discount>().HasQueryFilter(x => x.IsActive);

            var assembly = typeof(ProductCategoryConfiguration).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
