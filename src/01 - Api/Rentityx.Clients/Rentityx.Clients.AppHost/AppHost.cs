using Rentityx.Clients.AppHost;
using Rentityx.Clients;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Rentityx_Clients_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithSwaggerUi()
    .WithScalar()
    .WithReDoc();

await builder
    .Build()
    .RunAsync()
    .ConfigureAwait(false);
