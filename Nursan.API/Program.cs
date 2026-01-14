using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nursan.API.Extensions;
using Nursan.API.Middleware;
using Nursan.API.Services;
using Nursan.Domain.AmbarModels;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Domain.VideoModels;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Persistanse.Repository;
using System.Text;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Конфигурация на API версиониране
//builder.Services.AddApiVersioning(options =>
//{
//    options.DefaultApiVersion = new ApiVersion(1, 0);
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    options.ReportApiVersions = true;
//    // Използваме URL segment versioning: /api/v1/controller
//   // options.ApiVersionReader = Microsoft.AspNetCore.Mvc.ApiVersioning.ApiVersionReader.Combine(
//        new Microsoft.AspNetCore.Mvc.Versioning.UrlSegmentApiVersionReader(),
//        new Microsoft.AspNetCore.Mvc.Versioning.QueryStringApiVersionReader("version"),
//        new Microsoft.AspNetCore.Mvc.Versioning.HeaderApiVersionReader("X-Version")
//    );
//});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
builder.Services.AddControllersWithViews(); // Добавяме MVC поддръжка
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Добавяме JWT Bearer authentication в Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Добавяме API Key authentication в Swagger
    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key header using the X-API-Key scheme. Example: \"X-API-Key: {your-api-key}\"",
        Name = "X-API-Key",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
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
            Array.Empty<string>()
        },
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
    
    // Поддръжка за версиониране в Swagger
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Nursan Production API", 
        Version = "v1",
        Description = "API за управление на производство на кабелни инсталации. Версия 1.0"
    });
});

// Конфигурация на Entity Framework контексти
// Забележка: Ако connection string-ът не работи, проверете appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    // Fallback connection string - ПРОМЕНЕТЕ СПОРЕД ВАШАТА КОНФИГУРАЦИЯ
    connectionString = "Server=10.168.0.5;Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True";
}

var ambarConnectionString = builder.Configuration.GetConnectionString("AmbarConnection");
if (string.IsNullOrWhiteSpace(ambarConnectionString))
{
    ambarConnectionString = "Server=10.168.0.5;Database=Ambar;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True";
}

var personalConnectionString = builder.Configuration.GetConnectionString("PersonalConnection");
if (string.IsNullOrWhiteSpace(personalConnectionString))
{
    personalConnectionString = "Server=10.168.0.5;Database=Personal;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True";
}

builder.Services.AddDbContext<UretimOtomasyonContext>(options =>
{
    // Използваме connection string директно, за да избегнем OnConfiguring който използва XML
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(30);
    });
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
}, ServiceLifetime.Scoped);

builder.Services.AddDbContext<AmbarContext>(options =>
{
    options.UseSqlServer(ambarConnectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

builder.Services.AddDbContext<PersonalContext>(options =>
{
    options.UseSqlServer(personalConnectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// Регистрация на HttpClient за VideoApi
builder.Services.AddHttpClient();

// Конфигурация на MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Конфигурация на AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Регистрация на MemoryCache за 2FA
builder.Services.AddMemoryCache();

// Регистрация на JWT Service
builder.Services.AddScoped<JwtService>();

// Регистрация на Encryption Service
builder.Services.AddSingleton<EncryptionService>();

// Регистрация на API Key Storage Service (криптирано съхранение във файл - за обратна съвместимост)
builder.Services.AddScoped<ApiKeyStorageService>();

// Регистрация на API Key Database Service (криптирано съхранение в база данни)
builder.Services.AddScoped<ApiKeyDbService>();

// Регистрация на XML Sync Service
builder.Services.AddScoped<XmlSyncService>();

        // Регистрация на 2FA Service
        builder.Services.AddScoped<TwoFactorAuthService>();
        
        // Регистрация на ASP.NET Identity PasswordHasher (за HomeController)
        builder.Services.AddScoped<IPasswordHasher<AspNetUser>, PasswordHasher<AspNetUser>>();

// Конфигурация на JWT Authentication
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "NursanAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "NursanClient";

// Конфигурация на Authentication - Cookie за MVC и JWT Bearer за API
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Home/Login";
    options.LogoutPath = "/Home/Logout";
    options.AccessDeniedPath = "/Home/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    
    // Позволяваме JWT токен да се чете от cookie
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (string.IsNullOrEmpty(context.Token))
            {
                var token = context.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Регистрация на UnitOfWork за всеки контекст
builder.Services.AddScoped<IUnitOfWork>(provider =>
{
    var context = provider.GetRequiredService<UretimOtomasyonContext>();
    return new UnitOfWork(context);
});

// Factory за UnitOfWorkAmbar
builder.Services.AddScoped<IUnitOfWorkAmbar>(provider =>
{
    var context = provider.GetRequiredService<AmbarContext>();
    return new UnitOfWorkAmbar(context);
});

// Factory за UnitOfWorPersonal
builder.Services.AddScoped<IUnitOfWorkPersonal>(provider =>
{
    var context = provider.GetRequiredService<PersonalContext>();
    return new UnitOfWorPersonal(context);
});

// Конфигурация на CORS - По-ограничена политика за сигурност
builder.Services.AddCors(options =>
{
    // Development политика - без credentials за wildcard origin
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // В production трябва да се ограничи до конкретни origins
              .AllowAnyMethod()
              .AllowAnyHeader()
              // НЕ може да се използва AllowCredentials() с AllowAnyOrigin()
              // Ако се нуждаете от credentials, използвайте WithOrigins() с конкретни origins
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10)); // Кеширане на preflight заявки
    });
    
    // Production CORS политика с credentials (за конкретни origins)
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        // Променете origins според вашите нужди
        policy.WithOrigins("https://yourdomain.com", "http://localhost:5000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials() // Може да се използва само с конкретни origins
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
    
    // Production CORS политика - използва конкретни origins от appsettings.json
    options.AddPolicy("Production", policy =>
    {
        // Четем разрешените origins от appsettings.json
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        var allowInternalNetwork = builder.Configuration.GetValue<bool>("Cors:AllowInternalNetwork", false);
        
        if (allowedOrigins != null && allowedOrigins.Length > 0)
        {
            // Използваме конкретни origins от конфигурацията
            policy.WithOrigins(allowedOrigins)
                  .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH")
                  .WithHeaders("Content-Type", "Authorization", "X-API-Key", "X-Device-Id", "Accept")
                  .AllowCredentials() // Разрешаваме credentials когато имаме конкретни origins
                  .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        }
        else if (allowInternalNetwork)
        {
            // За вътрешна мрежа - използваме AllowAnyOrigin (CORS не поддържа CIDR/IP пулове директно)
            // ЗАБЕЛЕЖКА: В production използвай това САМО ако всички клиенти са в доверена вътрешна мрежа!
            policy.AllowAnyOrigin()
                  .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH")
                  .WithHeaders("Content-Type", "Authorization", "X-API-Key", "X-Device-Id", "Accept")
                  // НЕ може AllowCredentials() с AllowAnyOrigin()
                  .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        }
        else
        {
            // Fallback - ако няма конфигурация, използваме AllowAnyOrigin (небезопасно!)
            policy.AllowAnyOrigin()
                  .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                  .WithHeaders("Content-Type", "Authorization", "X-API-Key", "X-Device-Id")
                  .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        }
    });
});

// Конфигурация на API Key от appsettings.json или XML
var apiKeyFromConfig = builder.Configuration["ApiSettings:ApiKey"];
if (string.IsNullOrEmpty(apiKeyFromConfig))
{
    // Опитваме се да вземем от XML
    try
    {
        var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
        if (File.Exists(xmlPath))
        {
            var doc = new XmlDocument();
            doc.Load(xmlPath);
            var node = doc.SelectSingleNode("config/apiKey");
            apiKeyFromConfig = node?.Attributes?["Value"]?.InnerText;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Не може да се зареди API Key от XML: {ex.Message}");
    }
}

if (!string.IsNullOrEmpty(apiKeyFromConfig))
{
    builder.Configuration["ApiSettings:ApiKey"] = apiKeyFromConfig;
}

var app = builder.Build();

        // Configure the HTTP request pipeline.
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            // В production използваме exception handler
            app.UseExceptionHandler("/Error");
            app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");
        }

        app.UseHttpsRedirection();

// Static files за Views (CSS, JS, images)
app.UseStaticFiles();

// Routing middleware - трябва да е преди UseAuthentication
app.UseRouting();

// CORS трябва да е преди authentication middleware
app.UseCors(builder.Environment.IsDevelopment() ? "AllowAll" : "Production");

// Authentication middleware (JWT)
app.UseAuthentication();

// API Key Authentication Middleware (за обратна съвместимост)
app.UseApiKeyAuthentication();

app.UseAuthorization();

// Endpoint routing
app.UseEndpoints(endpoints =>
{
    // MVC routes - трябва да са преди API routes
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    
    // API routes с версиониране
    // По подразбиране използваме v1 ако не е указана версия
    endpoints.MapControllers();
    
    // Версионирани API routes: /api/v1/..., /api/v2/...
    endpoints.MapControllerRoute(
        name: "api-versioned",
        pattern: "api/v{version:apiVersion}/{controller=Home}/{action=Index}/{id?}");
});

app.Run();