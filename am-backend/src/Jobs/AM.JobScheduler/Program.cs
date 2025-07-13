using AM.Infrastructure;
using AM.JobScheduler;
using AM.JobScheduler.Interfaces;
using AM.JobScheduler.Services;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JobsSettings>(builder.Configuration.GetSection("JobsSettings"));

builder.Services.AddRepositories(builder.Configuration.GetConnectionString("DefaultConnection"));

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
