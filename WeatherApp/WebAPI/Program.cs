using BL;
using BL.Validation;
using DAL;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Quartz;
using WebAPI;
using WebAPI.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WeatherHistoryOptions>(builder.Configuration.GetSection("WeatherHistory"));

builder.Services.AddDbContext<WeatherDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IGeocodingRepository, GeocodingRepository>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IWeatherHistoryRepository, WeatherHistoryRepository>();
builder.Services.AddScoped<IValidator<string>, CityNameValidator>();
builder.Services.AddScoped<IValidator<int>, ForecastDaysValidator>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
});
builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});
builder.Services.AddHostedService<Scheduler>();

var app = builder.Build();

app.UseExceptionHandler("/Error");

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
