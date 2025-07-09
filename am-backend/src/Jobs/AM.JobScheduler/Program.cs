using AM.Application.Common.Interfaces.Repositories;
using AM.Infrastructure;
using AM.Infrastructure.Repositories;
using AM.JobScheduler;
using AM.JobScheduler.Interfaces;
using AM.JobScheduler.Services;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JobsSettings>(builder.Configuration.GetSection("JobsSettings"));
builder.Services.AddScoped<IMeteoriteRepository, MeteoriteRepository>();

builder.Services
    .AddHttpClient<IMeteoriteFetchService, MeteoriteFetchService>(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(15);
    })
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

builder.Services.AddHostedService<MeteortiresRefreshJob>();

var app = builder.Build();
app.Run();
