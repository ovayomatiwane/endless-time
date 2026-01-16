using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using WebApi;
using WebApi.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ApplicationDataContext"),
        b => b.MigrationsAssembly("EndlessTime.Domain"))
);

builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile).Assembly);

builder.Services.AddDependencies();

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
