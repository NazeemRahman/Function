using CSM_UserAuthentication.Interface;
using CSM_UserAuthentication.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);


builder.ConfigureFunctionsWebApplication();
builder.Configuration.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddHttpClient()
    .AddSingleton<AuthRepositoryFactory>()
    .AddSingleton<IAuthenticationRepository, LLdapAuthService>();

builder.Build().Run();
