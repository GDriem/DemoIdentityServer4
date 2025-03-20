using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProtectedApi.Data;
using ProtectedApi.Services;

// Configuración inicial
var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configuración de IdentityServer (gestión automática deshabilitada)
builder.Services.AddIdentityServer(options =>
{
    options.KeyManagement.Enabled = false;  // Evita la advertencia del key management
})
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddTestUsers(Config.TestUsers)
    .AddDeveloperSigningCredential(); // 👈 Añadir esta línea que falta

// Autenticación JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7108";
        options.Audience = "api1";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true
        };
    });

// Autorización (Requerido para [Authorize])
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

// Swagger con seguridad JWT
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Conexión a PostgreSQL con EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependencias adicionales
builder.Services.AddSingleton<ProductService>();
builder.Services.AddControllers();

var app = builder.Build();

// Pipeline de middleware (orden correcto)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIdentityServer();   // Primero IdentityServer
app.UseAuthentication();   // Después Autenticación
app.UseAuthorization();    // Finalmente, autorización

app.MapControllers();

app.Run();
