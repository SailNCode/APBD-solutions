using Microsoft.EntityFrameworkCore;
using Tutorial12;
using Tutorial12.Controllers;
using Tutorial12.Services;
using DbContext = Tutorial12.Data.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime;
        // You can add more DateFormatSettings if needed here
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<ITripsRepository, TripsRepository>();
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<TransactionHandler>();

builder.Services.AddDbContext<DbContext>(options => 
    SqlServerDbContextOptionsExtensions.UseSqlServer(options)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();