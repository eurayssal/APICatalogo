using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Models;
using APICatalogo.Repositories.Implementations;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var secretKey = builder.Configuration["JWT:SecretKey"]
                ?? throw new ArgumentException("Invalid secret key!!");

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions
    .ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

string mySqlConnection = builder.Configuration
                                .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; //Em prd isso deve ser true
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT: ValidAudience"],
        ValidIssuer = builder.Configuration["JWT: ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddScoped<ApiLoggingFilter>();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
