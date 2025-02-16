using DataAccessObjects.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using Repositories.Repository;
using Services;
using Services.IService;
using Services.Service;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FunewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FUNewsManagement")));
// Category
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<Repositories.IRepository.ICategoryRepo, Repositories.Repository.CategoryRepo>();
builder.Services.AddScoped<Services.IService.ICategoryService, Services.Service.CategoryService>();
// NewsArticle
builder.Services.AddScoped<INewsArticleRepo, NewsArticleRepo>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<Repositories.IRepository.INewsArticleRepo, Repositories.Repository.NewsArticleRepo>();
builder.Services.AddScoped<Services.IService.INewsArticleService, Services.Service.NewsArticleService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
