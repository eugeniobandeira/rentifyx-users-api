using Rentityx.Users.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Rentityx_Users_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithSwaggerUi()
    .WithScalar()
    .WithReDoc();

await builder
    .Build()
    .RunAsync()
    .ConfigureAwait(false);
