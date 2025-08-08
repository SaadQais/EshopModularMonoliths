var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults FIRST
builder.AddServiceDefaults();

// Logging configuration (now enhanced by Aspire)
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// Common services with assemblies
var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;
var orderingAssembly = typeof(OrderingModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

builder.Services
    .AddMediatRWithAssemblies(builder.Configuration, catalogAssembly, basketAssembly, orderingAssembly);

// Redis cache with Aspire
builder.AddRedisDistributedCache("distributedcache");

// Message bus with Aspire
builder.AddRabbitMQClient("messagebus");

// Update MassTransit configuration for Aspire
builder.Services
    .AddMassTransitWithAssemblies(
        builder.Configuration,
        catalogAssembly,
        basketAssembly,
        orderingAssembly);

// Authentication
builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// Module services - Updated for Aspire
builder.AddCatalogModule()
       .AddBasketModule()
       .AddOrderingModule();

// API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    var keycloakBase = builder.Configuration["Keycloak:Authority"];

    config.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{keycloakBase}/protocol/openid-connect/auth"),
                TokenUrl = new Uri($"{keycloakBase}/protocol/openid-connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "openid" },
                    { "profile", "profile" }
                }
            }
        },
        Description = "Authorize via Keycloak OpenID Connect"
    });

    OpenApiSecurityScheme keycloakSecurityScheme = new()
    {
        Reference = new OpenApiReference
        {
            Id = "Keycloak",
            Type = ReferenceType.SecurityScheme,
        },
        In = ParameterLocation.Header,
        Name = "Bearer",
        Scheme = "Bearer",
    };

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { keycloakSecurityScheme, Array.Empty<string>() },
    });
});

// Exception handling
builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Add Aspire default endpoints (health checks, etc.)
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
        config.DisplayRequestDuration();
        config.DocExpansion(DocExpansion.None);
        config.EnablePersistAuthorization();
        config.OAuthClientId("myclient");
    });
}

// Middleware pipeline
app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });
app.UseAuthentication();
app.UseAuthorization();

// Module middleware
app.UseCatalogModule()
   .UseBasketModule()
   .UseOrderingModule();

await app.RunAsync();