using BackServer.Contexts;
using BackServer.Repositories;
using BackServer.RepositoryChangers.Implementations;
using BackServer.RepositoryChangers.Interfaces;
using BackServer.Services;
using BackServer.Services.Interfaces;
using Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TestContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IHeadersVisitor, HeadersVisitorDb>();
builder.Services.AddScoped<IProductVisitor, ProductsVisitorDb>();
builder.Services.AddScoped<IProjectVisitor, ProjectsVisitorDb>();
builder.Services.AddScoped<IPropertyVisitor, PropertiesVisitorDb>();
builder.Services.AddScoped<ISaleVisitor, SalesVisitorDb>();

builder.Services.AddScoped<IHeadersChanger, HeadersChangerDb>();
builder.Services.AddScoped<IProjectChanger, ProjectChangerDb>();
builder.Services.AddScoped<IProductChanger, ProductChangerDb>();
builder.Services.AddScoped<ISaleChanger, SaleChangerDb>();
builder.Services.AddScoped<IPropertyChanger, PropertyChangerDb>();

builder.Services.AddScoped<IHeadersService, HeadersService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<ISaleService, SaleService>();

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