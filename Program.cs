using Crud.API.src.Domain.Interfaces;
using Crud.API.src.Infra.Context;
using Crud.API.src.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
                    
builder.Services.AddControllers();
// Add services to the container.
var builderServices = builder.Services;
builderServices.AddControllers();
builderServices.AddDbContext<SystemContext>(
                context => context.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

builderServices.AddAutoMapper(typeof(Program));

var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
builderServices.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Person API", Version = "v1" });
    c.IncludeXmlComments(apiXmlPath);
});

builderServices.AddScoped<IPersonRepository, PersonRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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
