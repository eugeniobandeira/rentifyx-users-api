using Rentityx.Clients.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Rentityx_Clients_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithSwaggerUi()
    .WithScalar()
    .WithReDoc();

builder.Build().Run();
