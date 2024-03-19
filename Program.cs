using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeeklyPay.Services;
using WeeklyPay.Utilities;
using WeeklyPay.Utilities.Interfaces;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

var env = builder.Environment;

var config = builder.Configuration
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
    .Build();

builder.Services.AddHostedService<WeeklyPayCalculator>();
builder.Services.AddSingleton<ICheckingAccount, CheckingAccount>();
builder.Services.AddSingleton<ISavingsAccount, SavingsAccount>();

var opt = config.GetSection(nameof(WeeklyPayOptions))
 .Get<WeeklyPayOptions>();

builder.Configuration.GetSection(nameof(WeeklyPayOptions)).Bind(opt);

//builder.Services.Configure<WeeklyPayOptions>(config.GetSection(nameof(WeeklyPayOptions)));





IHost host = builder.Build();
host.Run();