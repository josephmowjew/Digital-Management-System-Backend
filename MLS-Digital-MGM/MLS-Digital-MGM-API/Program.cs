using DataStore.Core.Models;
using DataStore.Core.Services;
using DataStore.Core.Services.Interfaces;
using DataStore.Data;
using DataStore.Middleware;
using DataStore.Persistence.Interfaces;
using DataStore.Persistence.SQLRepositories;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

// configuring the token and making sure that the user has the correct token all the time
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("TokenString:TokenKey"));

// Add services to the container
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var user = userManager.GetUserAsync(context.HttpContext.User).Result;

            if (user == null)
            {
                context.Fail("UnAuthorized");
            }
            return Task.CompletedTask;
        }
    };
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateAudience = false
    };
});

builder.Services.AddControllers().AddNewtonsoftJson(x => 
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add repository services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddTransient<TokenManagerMiddleware>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();
builder.Services.AddTransient<ITokenManager, TokenManager>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEntityResolverService, EntityResolverService>();
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedRedisCache(r => { r.Configuration = builder.Configuration["redis:connectionString"]; });

var provider = builder.Configuration["ServerSettings:ServerName"];
string mySqlConnectionStr = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (provider)
    {
        case "MySQL":
            options.UseMySQL(mySqlConnectionStr);
            break;
        default:
            throw new Exception($"Unsupported provider: {provider}");
    }
});

builder.Services.AddIdentity<ApplicationUser, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseMemoryStorage());

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the middleware
app.UseCors("AllowAllOrigins");
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireServer();
app.UseHangfireDashboard();

app.MapControllers();

app.Run("http://localhost:5000");
