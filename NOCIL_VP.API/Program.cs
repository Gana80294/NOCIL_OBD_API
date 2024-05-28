using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NOCIL_VP.API.Auth;
using NOCIL_VP.API.Extensions;
using NOCIL_VP.Domain.Core.Configurations;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Infrastructure.Data.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Cors Policy
builder.Services.AddCors(p =>
    p.AddPolicy("corsapp", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

// Add DB Context
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<VpContext>(opts => opts.UseSqlServer(@$"{connectionString}"));

// Add Configuration Settings
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JWTSecurity"));
builder.Services.Configure<SmtpSetting>(builder.Configuration.GetSection("SMTPDetails"));
builder.Services.Configure<OtpSetting>(builder.Configuration.GetSection("OtpDetails"));
builder.Services.Configure<GstSetting>(builder.Configuration.GetSection("GSTINDetails"));
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

// Add Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        RequireExpirationTime = true,
        ValidateLifetime = true,
        RequireSignedTokens = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTSecurity").GetValue<string>("securityKey"))),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("JWTSecurity").GetValue<string>("issuer"),
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("JWTSecurity").GetValue<string>("audience")
    };
});

// Add services to the container.

builder.Services.AddControllers(config => { config.Filters.Add(typeof(CommonExceptionHandler)); }).AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMvc(config =>
{
    config.Filters.Add<CommonExceptionHandler>(-100);
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    // Use the default property (Pascal) ca sing.
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRepositories();
builder.Services.AddPolicies();
builder.Services.AddHttpLogging(o => { });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthMiddleware();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
