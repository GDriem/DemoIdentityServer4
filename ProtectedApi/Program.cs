using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProtectedApi.Middlewares;
using ProtectedApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configuración de IdentityServer
builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true; // Configuración necesaria para evitar errores de validación
})
.AddInMemoryApiScopes(new[]
{
    new ApiScope("api1", "My Protected API") // Definir el scope de la API
})
.AddInMemoryClients(new[]
{
    new Client
    {
        ClientId = "client",
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        ClientSecrets = { new Secret("secret".Sha256()) },
        AllowedScopes = { "api1" }
    }
});

// Configurar autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7108"; // URL del servidor de identidad
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false 
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Agregar esquema de seguridad
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token in the text input below.\nExample: Bearer abc123"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


//Dependencias
builder.Services.AddSingleton<ProductService>();

var app = builder.Build();

// add a middleware to handle exceptions
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Configurar middleware
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
