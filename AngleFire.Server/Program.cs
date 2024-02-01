using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Redis;
using Hangfire.CosmosDB;
using Hangfire.Redis.StackExchange;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the DI container
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Configure Hangfire
builder.Services.AddHangfire(configuration =>
{
    // Use SQL Server storage
    configuration.UseSqlServerStorage("<connection string>");

    // Use Redis storage
    configuration.UseRedisStorage("<redis connection string>");

});
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration["CacheConnection"];

});
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

var app = builder.Build();

// Middleware setup
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHubService>("/chatHub");

app.MapFallbackToFile("/index.html");

// Enqueue all jobs in the jobs folder
var jobFolder = "<path to jobs folder>";
var jobFiles = Directory.GetFiles(jobFolder, "*.cs");
foreach (var jobFile in jobFiles)
{
    var jobType = Type.GetType(jobFile);
    if (jobType != null)
    {
        BackgroundJob.Enqueue(() => Activator.CreateInstance(jobType));
    }
}

app.Run();