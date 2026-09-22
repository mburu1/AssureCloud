using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

// SQL Server
var sqlServer = builder.AddSqlServer("sql")
    .WithDataVolume()
    .AddDatabase("assurecloud");

// Azure Service Bus Emulator (using container)
var serviceBus = builder.AddContainer("servicebus", "mcr.microsoft.com/azure-messaging/servicebus-emulator:latest")
    .WithHttpEndpoint(port: 5672, targetPort: 5672, name: "amqp")
    .WithHttpEndpoint(port: 9354, targetPort: 9354, name: "management")
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithEnvironment("SERVICE_BUS_EMULATOR_CONNECTION_STRING", "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true");

// Redis for caching
var redis = builder.AddRedis("redis")
    .WithDataVolume();

// IdentityServer (using Duende IdentityServer container)
var identityServer = builder.AddContainer("identityserver", "duende/identityserver:latest")
    .WithHttpEndpoint(port: 5000, targetPort: 80, name: "http")
    .WithHttpEndpoint(port: 5001, targetPort: 443, name: "https")
    .WithEnvironment("IDENTITYSERVER_LICENSE", "")
    .WaitFor(sqlServer);

// API Project
var api = builder.AddProject<Projects.AssureCloud_Api>("api")
    .WithReference(sqlServer)
    .WithReference(serviceBus)
    .WithReference(identityServer)
    .WithReference(redis)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WaitFor(sqlServer)
    .WaitFor(serviceBus)
    .WaitFor(identityServer);

// MVC Project
var mvc = builder.AddProject<Projects.AssureCloud_Mvc>("mvc")
    .WithReference(sqlServer)
    .WithReference(serviceBus)
    .WithReference(identityServer)
    .WithReference(redis)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WaitFor(sqlServer)
    .WaitFor(serviceBus)
    .WaitFor(identityServer);

// Workers Project
var workers = builder.AddProject<Projects.AssureCloud_Workers>("workers")
    .WithReference(sqlServer)
    .WithReference(serviceBus)
    .WithReference(identityServer)
    .WithReference(redis)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WaitFor(sqlServer)
    .WaitFor(serviceBus)
    .WaitFor(identityServer);

// Messaging Project (if separate)
var messaging = builder.AddProject<Projects.AssureCloud_Messaging>("messaging")
    .WithReference(sqlServer)
    .WithReference(serviceBus)
    .WithReference(identityServer)
    .WithReference(redis)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WaitFor(sqlServer)
    .WaitFor(serviceBus)
    .WaitFor(identityServer);

builder.Build().Run();