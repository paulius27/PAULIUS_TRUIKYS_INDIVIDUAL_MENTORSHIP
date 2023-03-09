using IdentityServer;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

var connectionString = builder.Configuration.GetConnectionString("Default");
var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

Log.Information("Seeding database...");
SeedData.EnsureSeedData(connectionString);
Log.Information("Done seeding database.");

builder.Services.AddDbContext<ApplicationDbContext>(options 
    => options.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(migrationAssembly)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options => 
    { 
        options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(migrationAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(migrationAssembly));
    })
    .AddDeveloperSigningCredential();

var app = builder.Build();

app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
