using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// configuring the token and making sure that the user has the correct token all the time
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("TokenString:TokenKey"));
// Add services to the container.
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var userMachine = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                var user = userMachine.GetUserAsync(context.HttpContext.User);

                if (user == null)
                {
                    context.Fail("UnAuthorised");
                }
                return Task.CompletedTask;
            }
        };
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateAudience = false
        };

    });

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<TokenManagerMiddleware>();
builder.Services.AddTransient<ITokenManager, TokenManager>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedRedisCache(r => { r.Configuration = builder.Configuration["redis:connectionString"]; });
var provider = builder.Configuration["ServerSettings:ServerName"];
string mySqlConnectionStr = builder.Configuration.GetConnectionString("MySqlConnection");



builder.Services.AddDbContext<ApplicationDbContext>(
options => _ = provider switch
{
    "MySQL" => options.UseMySQL(mySqlConnectionStr),
    // "MySQL" => options.UseMySQL(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr),
    //b => b.SchemaBehavior(MySqlSchemaBehavior.Ignore)),

    //"SqlServer" => options.UseSqlServer(
    //    builder.Configuration.GetConnectionString("DefaultConnection")),

    _ => throw new Exception($"Unsupported provider: {provider}")
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultUI()
                   .AddDefaultTokenProviders();


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
