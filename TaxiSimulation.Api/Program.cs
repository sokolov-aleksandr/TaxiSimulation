using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;
using TaxiSimulation.Api.Settings;
using TaxiSimulation.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configuration
builder.Services.Configure<MapSettings>(builder.Configuration.GetSection("MapSettings"));

// Grid from appsettings
var mapWidth = builder.Configuration.GetValue<int>("MapSettings:Width");
var mapHeight = builder.Configuration.GetValue<int>("MapSettings:Height");
var grid = new Grid(mapWidth, mapHeight);

builder.Services.AddSingleton(grid);
builder.Services.AddSingleton<DriverService>();
builder.Services.AddSingleton<DriverLocatorService>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// middleware parralel request limit
app.UseMiddleware<ParallelLimitMiddleware>();

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
