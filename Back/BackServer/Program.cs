using BackServer.Contexts;
using BackServer.Repositories;
using BackServer.RepositoryChangers.Implementations;
using BackServer.RepositoryChangers.Interfaces;
using BackServer.Services;
using BackServer.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TestContext>(options =>
{

    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});

builder.Services.AddTransient<IHeadersVisitor, HeadersVisitorDb>();
builder.Services.AddTransient<IProductVisitor, ProductsVisitorDb>();
builder.Services.AddTransient<IProjectVisitor, ProjectsVisitorDb>();
builder.Services.AddTransient<IPropertyVisitor, PropertiesVisitorDb>();
builder.Services.AddTransient<ISaleVisitor, SalesVisitorDb>();

builder.Services.AddTransient<IHeadersChanger, HeadersChangerDb>();
builder.Services.AddTransient<IProjectChanger, ProjectChangerDb>();
builder.Services.AddTransient<IProductChanger, ProductChangerDb>();
builder.Services.AddTransient<ISaleChanger, SaleChangerDb>();
builder.Services.AddTransient<IPropertyChanger, PropertyChangerDb>();

builder.Services.AddTransient<IHeadersService, HeadersService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IPropertyService, PropertyService>();
builder.Services.AddTransient<ISaleService, SaleService>();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();