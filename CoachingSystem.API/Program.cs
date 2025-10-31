using System.Text;
using CoachingSystem.Application.Interfaces;
using CoachingSystem.Domain.Entities; 
using CoachingSystem.Infrastructure.Data;
using CoachingSystem.Infrastructure.Repositories;
using CoachingSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using CoachingSystem.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------
// Infrastructure ve Application Servislerini Kaydetme (DI)
// -------------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext Kaydı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString,
        npgsqlOptions =>
        {
            // Domain'deki Enum'ı PostgreSQL'e eşliyoruz
            npgsqlOptions.UseAdminDatabase("postgres").MapEnum<UserRole>();
        }));

// Repository ve Servis Kayıtları
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<CoachingSystem.Application.Interfaces.ITokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

// JWT Yapılandırması
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? string.Empty;
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? string.Empty;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // 🎯 PROFESYONEL ÇÖZÜM: Tüm ayarlar tek bir blokta yapılır.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ✨ KRİTİK DÜZELTME: Rol claim'inin adını "role" olarak ayarlıyoruz.
            // Bu, JwtTokenService'te oluşturduğumuz "role" claim'i ile tam olarak eşleşmelidir.
            RoleClaimType = "role",

            // 1. İMZAYI DOĞRULA (GÜVENLİK ZORUNLULUĞU)
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,

            // 2. PROFESYONEL STANDART: Issuer ve Audience doğrulamaları AÇIK kalır (TRUE).
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            // Audience değerini tek bir string olarak kabul ediyoruz.
            ValidAudience = jwtAudience, 

            // 3. Yaşam Süresi Doğrulaması
            ValidateLifetime = true,
            // Local testlerdeki saat farkı sorununu çözmek için tolerans eklenir (5 dakika)
            ClockSkew = TimeSpan.FromMinutes(5) 
        };
        
        // Bu, JWT claim'lerinin .NET standartlarına eşleşmesini sağlar ve 401 hatalarını çözer.
        options.MapInboundClaims = false;

        // Bu Event, doğrulama başarısız olursa konsola log atar.
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"!!! JWT AUTHENTICATION BAŞARISIZ !!! Hata Tipi: {context.Exception.GetType().Name}, Detay: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

// 🎯 ÇÖZÜM: Global yetkilendirme kuralını ekler (Eğer controller'larda [Authorize] unutulursa diye).
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger Yapılandırması
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Coaching System API", Version = "v1" });
    
    // 1. Authorization Şemasını Tanımlama (Swagger'a JWT'yi tanıtma)
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    // 2. Bu Şemayı Güvenli Uç Noktalara Uygulama
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🎯 KRİTİK SIRALAMA: Routing'i ekleyelim.
app.UseRouting(); 

// 1. Authentication: Token'ı okur ve kimliği oluşturur.
app.UseAuthentication(); 

// 2. Authorization: Kimliğe göre yetkiyi kontrol eder.
app.UseAuthorization();

app.MapControllers();
app.Run();
