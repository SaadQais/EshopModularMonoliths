var builder = DistributedApplication.CreateBuilder(args);

// Database - PostgreSQL with volume persistence
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var eshopdb = postgres.AddDatabase("EShopDb");

// Distributed Cache - Redis with RedisInsight
var redis = builder.AddRedis("distributedcache")
    .WithRedisInsight(); // Adds RedisInsight for Redis management

var rabbitmq = builder.AddRabbitMQ("messagebus")
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

// Logging - Seq for structured logging
var seq = builder.AddSeq("seq")
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithEnvironment("SEQ_FIRSTRUN_ADMINPASSWORD", "123qweASD");

var keycloak = builder.AddKeycloak("identity", 8080)
    //.WithEnvironment("KEYCLOAK_ADMIN", "admin")
    //.WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    //.WithEnvironment("KC_DB", "postgres")
    //.WithEnvironment("KC_DB_URL", "jdbc:postgresql://postgres:5432/EShopDb?currentSchema=identity")
    //.WithEnvironment("KC_DB_USERNAME", "postgres")
    //.WithEnvironment("KC_DB_PASSWORD", "postgres")
    //.WithEnvironment("KC_HOSTNAME", "http://localhost:9090")
    .WithLifetime(ContainerLifetime.Persistent);

// Main API Application
var api = builder.AddProject<Projects.Api>("api")
    .WithReference(eshopdb)
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithReference(seq)
    .WithReference(keycloak)
    .WaitFor(eshopdb)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithEnvironment("Keycloak__AuthServerUrl", keycloak.GetEndpoint("http"))
    .WithEnvironment("Keycloak__Authority", "http://localhost:9090/realms/myrealm")
    .WithEnvironment("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true")
    .WithHttpsEndpoint(port: 5060, name: "api-https");

builder.Build().Run();