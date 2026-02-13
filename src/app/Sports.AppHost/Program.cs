var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("sql-password", secret: true);

var sqlServer = builder
    .AddSqlServer("sql-server", password: sqlPassword, port: 14330)
    .AddDatabase("SportsDB");

var messaging = builder.AddRabbitMQ("messaging");

var loki = builder.AddContainer("loki", "grafana/loki", "3.5.0")
    .WithHttpEndpoint(port: 3100, targetPort: 3100, name: "http")
    .WithBindMount("../../../infrastructure/loki/loki-config.yaml", "/etc/loki/local-config.yaml")
    .WithArgs("-config.file=/etc/loki/local-config.yaml")
    .WithHttpHealthCheck("/ready");

var grafana = builder.AddContainer("grafana", "grafana/grafana", "11.6.0")
    .WithHttpEndpoint(targetPort: 3000, name: "http")
    .WithEnvironment("GF_SECURITY_ADMIN_USER", "admin")
    .WithEnvironment("GF_SECURITY_ADMIN_PASSWORD", "admin")
    .WithEnvironment("GF_AUTH_ANONYMOUS_ENABLED", "true")
    .WithEnvironment("GF_AUTH_ANONYMOUS_ORG_ROLE", "Admin")
    .WithBindMount("../../../infrastructure/grafana/provisioning", "/etc/grafana/provisioning")
    .WaitFor(loki);

var lokiEndpoint = loki.GetEndpoint("http");

var api = builder.AddProject<Projects.Sports_Api>("sports-api")
.WithReference(sqlServer)
.WithReference(messaging)
.WithEnvironment("Loki__Endpoint", lokiEndpoint)
.WaitFor(sqlServer)
.WaitFor(messaging)
.WaitFor(loki);

builder.AddProject<Projects.Sports_MatchSimulationWorker>("sports-worker")
    .WithReference(sqlServer)
    .WithReference(messaging)
    .WithEnvironment("Loki__Endpoint", lokiEndpoint)
    .WaitFor(sqlServer)
    .WaitFor(messaging)
    .WaitFor(loki);

builder.AddNpmApp("sports-ui", "../../../SportsUI", "dev")
.WithReference(api)
.WithHttpEndpoint(env: "PORT")
.WithExternalHttpEndpoints();

builder.Build().Run();
