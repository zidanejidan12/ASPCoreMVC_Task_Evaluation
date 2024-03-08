using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using MyWebFormApp.BO;

var builder = WebApplication.CreateBuilder(args);

// Add AutoMapper and specify the assembly containing the mapping profiles
builder.Services.AddAutoMapper(typeof(Program));

// Add services
builder.Services.AddControllersWithViews();

// Register your BLL services
builder.Services.AddScoped<ICategoryBLL, CategoryBLL>();
builder.Services.AddScoped<IArticleBLL, ArticleBLL>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Article, ArticleDTO>(); // Define the mapping from Article to ArticleDTO
        // Add other mappings if needed
    }
}