using DataAccessObjects.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using Repositories.Repository;
using Services;
using Services.IService;
using Services.Service;
using TechTalk.SpecFlow.Assist;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FunewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FUNewsManagement")));
// Category
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");
// NewsArticle
builder.Services.AddScoped<INewsArticleRepo, NewsArticleRepo>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
// SystemAccount
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();
// Tag
builder.Services.AddScoped<ITagRepo, TagRepo>();
builder.Services.AddScoped<ITagService, TagService>();
// HTTP session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout
    options.Cookie.HttpOnly = true; // For security
    options.Cookie.IsEssential = true; // Ensure session cookie is always created
});


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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
