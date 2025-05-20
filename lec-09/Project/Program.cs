using Microsoft.EntityFrameworkCore;
using Tutorial9.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
string? connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseDbContext>(opt =>
{
    opt.UseSqlServer(connectionString);
});
// builder.Services.AddScoped<IService, Service>();
// builder.Services.AddScoped<IRepository, Repository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();