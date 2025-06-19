using Application.Configurations;
using Framework.Application;
using Framework.Infrastructure;
using Infrastructure.Configurations;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Presentation.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.SetupServices();
builder.Services.SetupInfrastructureServices();
builder.Services.AddTransient<IFileUploader, FileUploader>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LoginPath = new PathString("/Login");
        o.LogoutPath = new PathString("/Logout");
        o.AccessDeniedPath = new PathString("/NotFound");
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminArea",
        builder => builder.RequireRole(new List<string> { Roles.Admin }));

});

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeAreaFolder("Admin", "/", "AdminArea");
    });

var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetService<ApplicationDbContext>();


if (context != null)
{
    await context.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseCookiePolicy();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();