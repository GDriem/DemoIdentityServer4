using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProtectedApi.Middlewares;
using ProtectedApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Consola
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configuración de IdentityServer
builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true; // Configuración necesaria para evitar errores de validación
}).AddInMemoryClients(Config.Clients)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddTestUsers(Config.TestUsers) // Agrega usuarios de prueba
    .AddDeveloperSigningCredential(); // ⚠️ Solo para desarrollo


// Configurar autenticación con JWT
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.Authority = "https://localhost:7108"; // URL del servidor de identidad
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateAudience = false 
//        };
//    });

// Habilitar autenticación con tokens en la API
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7108"; // URL del IdentityServer
        options.Audience = "api1"; // Nombre de la API definida en Config.cs

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
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

//Redireccion del Login
builder.Services.Configure<IdentityServerOptions>(options =>
{
    options.UserInteraction.LoginUrl = "https://localhost:7168/account/login"; // 🔹 Cliente MVC manejará el login
    options.UserInteraction.LogoutUrl = "https://localhost:7168/account/logout";
    options.UserInteraction.ErrorUrl = "https://localhost:7168/home/error";
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

app.Use(async (context, next) =>
{
    Console.WriteLine($"Incoming request: {context.Request.Path} {context.Request.QueryString}");
    await next();
});
// Configurar middleware
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();




app.MapControllers();

app.Run();
