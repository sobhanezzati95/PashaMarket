using Domain.Entities.DiscountAggregate;
using Domain.Entities.OrderAggregate;
using Domain.Entities.ProductAggregate;
using Domain.Entities.UserAggregate;
using Infrastructure.Configurations.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;
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
            new(id:1,
                name:"قهوه دمی و اسپرسو",
                description:"قهوه دمی و اسپرسو",
                picture:"قهوه-دمی-و-اسپرسو/category1.png",
                pictureAlt:"قهوه دمی و اسپرسو",
                pictureTitle:"قهوه دمی و اسپرسو",
                keywords:"قهوه دمی و اسپرسو",
                metaDescription:"قهوه دمی و اسپرسو",
                slug:"قهوه-دمی-و-اسپرسو"),
            new(id:2,
                name:"لوازم جانبی و تجهیزات",
                description:"لوازم جانبی و تجهیزات",
                picture:"لوازم-جانبی-و-تجهیزات/category2.png",
                pictureAlt:"لوازم جانبی و تجهیزات",
                pictureTitle:"لوازم جانبی و تجهیزات",
                keywords:"لوازم جانبی و تجهیزات",
                metaDescription:"لوازم جانبی و تجهیزات",
                slug:"لوازم-جانبی-و-تجهیزات"),
            new(id:3,
                name:"اسپرسو ساز",
                description:"اسپرسو ساز",
                picture:"اسپرسو-ساز/category3.png",
                pictureAlt:"اسپرسو ساز",
                pictureTitle:"اسپرسو ساز",
                keywords:"اسپرسو ساز",
                metaDescription:"اسپرسو ساز",
                slug:"اسپرسو-ساز"),
            new(id:4,
                name:"پک تستر قهوه",
                description:"پک تستر قهوه",
                picture:"پک-تستر-قهوه/category4.png",
                pictureAlt:"پک تستر قهوه",
                pictureTitle:"پک تستر قهوه",
                keywords:"پک تستر قهوه",
                metaDescription:"پک تستر قهوه",
                slug:"پک-تستر-قهوه"),
            new(id:5,
                name:"قهوه ترک",
                description:"تست",
                picture:"قهوه-ترک/category5.png",
                pictureAlt:"قهوه ترک",
                pictureTitle:"قهوه ترک",
                keywords:"قهوه ترک",
                metaDescription:"قهوه ترک",
                slug:"قهوه-ترک"),
        });

        modelBuilder.Entity<Product>().HasData(new List<Product>()
        {
            new(id:1,
                name :"دانه قهوه اسپرسو یونیکا مقدار 250 گرم",
                code:"123",
                brand:"پاشا",
                unitPrice :3400000,
                count:50,
                description :"دانه قهوه اسپرسو یونیکا مقدار 250 گرم",
                picture:"قهوه-دمی-و-اسپرسو/دانه-قهوه-اسپرسو-یونیکا-مقدار-250-گرم/2025-06-21-14-59-19-p1.png",
                pictureAlt :"دانه قهوه اسپرسو یونیکا مقدار 250 گرم",
                pictureTitle :"دانه قهوه اسپرسو یونیکا مقدار 250 گرم",
                categoryId :1,
                slug :"دانه-قهوه-اسپرسو-یونیکا-مقدار-250-گرم",
                keywords :"دانه قهوه اسپرسو یونیکا مقدار 250 گرم",
                metaDescription :"دانه-قهوه-اسپرسو-یونیکا-مقدار-250-گرم"),
            new(id:2,
                name :"قهوه ترک گرمی مقدار 200 گرم رنگ نارنجی",
                code:"456",
                brand:"پاشا",
                unitPrice :230000,
                count:20,
                description :"قهوه ترک گرمی مقدار 200 گرم رنگ نارنجی",
                picture:"قهوه-ترک/قهوه-ترک-گرمی-مقدار-200-گرم-رنگ-بنفش/2025-06-21-15-01-22-p2.png",
                pictureAlt :"قهوه ترک گرمی مقدار 200 گرم رنگ نارنجی",
                pictureTitle :"قهوه ترک گرمی مقدار 200 گرم رنگ نارنجی",
                categoryId :5,
                slug :"قهوه-ترک-گرمی-مقدار-200-گرم-رنگ-بنفش",
                keywords :"قهوه ترک گرمی مقدار 200 گرم رنگ نارنجی",
                metaDescription :"قهوه ترک گرمی مقدار 200 گرم رنگ نارنجی"),
            new(id:3,
                name :"قهوه ترک گرمی مقدار 200 گرم رنگ سبز",
                code:"789",
                brand:"پاشا",
                unitPrice :230000,
                count:10,
                description :"قهوه ترک گرمی مقدار 200 گرم رنگ سبز",
                picture:"قهوه-ترک/قهوه-ترک-گرمی-مقدار-200-گرم-رنگ-سبز/2025-06-21-15-05-06-p3.png",
                pictureAlt :"قهوه ترک گرمی مقدار 200 گرم رنگ سبز",
                pictureTitle :"قهوه ترک گرمی مقدار 200 گرم رنگ سبز",
                categoryId :5,
                slug :"قهوه-ترک-گرمی-مقدار-200-گرم-رنگ-سبز",
                keywords :"قهوه ترک گرمی مقدار 200 گرم رنگ سبز",
                metaDescription :"قهوه ترک گرمی مقدار 200 گرم رنگ سبز"),
            new(id:4,
                name :"قهوه ترک بن مانو مقدار 250 گرم رنگ بنفش",
                code:"124",
                brand:"پاشا",
                unitPrice :154000,
                count:9,
                description :"قهوه ترک بن مانو مقدار 250 گرم رنگ بنفش",
                picture:"قهوه-ترک//قهوه-ترک-بن-مانو-مقدار-250-گرم-رنگ-بنفش/2025-06-21-15-06-33-p4.png",
                pictureAlt :"قهوه ترک بن مانو مقدار 250 گرم رنگ بنفش",
                pictureTitle :"قهوه ترک بن مانو مقدار 250 گرم رنگ بنفش",
                categoryId :5,
                slug :"قهوه-ترک-بن-مانو-مقدار-250-گرم-رمگ-بنفش",
                keywords :"قهوه ترک بن مانو مقدار 250 گرم رنگ بنفش",
                metaDescription :"قهوه ترک بن مانو مقدار 250 گرم رنگ بنفش"),
            new(id:5,
                name :"دانه قهوه اسپرسو کد 200 مقدار 100 گرم",
                code:"125",
                brand:"پاشا",
                unitPrice :80000,
                count:5,
                description :"دانه قهوه اسپرسو کد 200 مقدار 100 گرم",
                picture:"قهوه-دمی-و-اسپرسو//دانه-قهوه-اسپرسو-کد-200-مقدار-100-گرم/2025-06-21-15-08-14-p6.png",
                pictureAlt :"دانه قهوه اسپرسو کد 200 مقدار 100 گرم",
                pictureTitle :"دانه قهوه اسپرسو کد 200 مقدار 100 گرم",
                categoryId :1,
                slug :"دانه-قهوه-اسپرسو-کد-200-مقدار-100-گرم",
                keywords :"دانه قهوه اسپرسو کد 200 مقدار 100 گرم",
                metaDescription :"دانه قهوه اسپرسو کد 200 مقدار 100 گرم"),
            new(id:6,
                name :"قهوه ترک بن مانو مقدار 250 گرم نارنجی",
                code:"126",
                brand:"پاشا",
                unitPrice :150000,
                count:6,
                description :"قهوه ترک بن مانو مقدار 250 گرم نارنجی",
                picture:"قهوه-ترک//قهوه-ترک-بن-مانو-مقدار-250-گرم-نارنجی/2025-06-21-15-11-30-p5.png",
                pictureAlt :"قهوه ترک بن مانو مقدار 250 گرم نارنجی",
                pictureTitle :"قهوه ترک بن مانو مقدار 250 گرم نارنجی",
                categoryId :5,
                slug :"قهوه-ترک-بن-مانو-مقدار-250-گرم-نارنجی",
                keywords :"قهوه ترک بن مانو مقدار 250 گرم نارنجی",
                metaDescription :"قهوه ترک بن مانو مقدار 250 گرم نارنجی"),
        });

        modelBuilder.Entity<Slide>().HasData(new List<Slide>()
        {
            new(id:1,
                picture:"slides/slide-1.png",
                pictureAlt:"دانه قهوه اسپرسو جیورنو",
                pictureTitle:"دانه قهوه اسپرسو جیورنو",
                heading:"دانه قهوه اسپرسو جیورنو",
                title:"یک طعم باورنکردنی !",
                text:"قطعا نام آشنای جیورنو را شنیده اید، جیورنو یکی از گونه های قهوه است که در نواحی مختلف کمربند قهوه کشت می شود.",
                link:"تست",
                btnText:"مشاهده قروشگاه"),
            new(id:2,
                picture:"slides/slide-2.png",
                pictureAlt:"دانه قهوه اسپرسو جیورنو",
                pictureTitle:"دانه قهوه اسپرسو جیورنو",
                heading:"دانه قهوه اسپرسو جیورنو",
                title:"یک طعم باورنکردنی !",
                text:"قطعا نام آشنای جیورنو را شنیده اید، جیورنو یکی از گونه های قهوه است که در نواحی مختلف کمربند قهوه کشت می شود.",
                link:"تست",
                btnText:"مشاهده قروشگاه"),
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
            new(id:1,name:"Admin"),
            new(id:2,name:"SystemUser"),
        });

        modelBuilder.Entity<User>().HasData(new List<User>()
        {
            User.Create(id:1,
                        fullname:"test",
                        username:"test",
                        mobile:"test",
                        email:"test@gmail.com",
                        password:"10000.UJdStpoXiNqh1QLmQlLLVg==.VapUTtlJx8y8scGTsTkWdwIncix3biPefIYJD3tFeXM=",
                        birthDate:null,
                        roleId:1),
        });
        #endregion

        #region QueryFilter
        modelBuilder.Entity<Product>().HasQueryFilter(x => x.IsActive);
        modelBuilder.Entity<Slide>().HasQueryFilter(x => x.IsActive);
        modelBuilder.Entity<ProductCategory>().HasQueryFilter(x => x.IsActive);
        modelBuilder.Entity<ProductPicture>().HasQueryFilter(x => x.IsActive);
        modelBuilder.Entity<ProductFeature>().HasQueryFilter(x => x.IsActive);
        modelBuilder.Entity<Role>().HasQueryFilter(x => x.IsActive);
        modelBuilder.Entity<Order>().HasQueryFilter(x => x.IsActive);
        modelBuilder.Entity<OrderItem>().HasQueryFilter(x => x.IsActive);
        modelBuilder.Entity<Discount>().HasQueryFilter(x => x.IsActive);
        #endregion

        var assembly = typeof(ProductCategoryConfiguration).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}