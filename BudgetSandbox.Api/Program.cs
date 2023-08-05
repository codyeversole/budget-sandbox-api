using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Auth;
using BudgetSandbox.Api.Data;
using BudgetSandbox.Api.Models;
using BudgetSandbox.Api.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetValue<string>("FrontendUrl")).AllowAnyHeader().AllowAnyMethod();
        policy.WithOrigins(builder.Configuration.GetValue<string>("FrontendTestUrl")).AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, RoleHandler>();

KeycloakSettings keycloakSettings = builder.Configuration.GetSection("Keycloak").Get<KeycloakSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"{keycloakSettings.BaseUrl}/realms/{keycloakSettings.Realm}";
    options.Audience = "account";
    options.MetadataAddress = $"{keycloakSettings.BaseUrl}/realms/{keycloakSettings.Realm}/.well-known/openid-configuration";
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new()
    {
        ValidAudience = "account"
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddServices();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("NormalUser", policy => policy.Requirements.Add(new RoleRequirement(new List<string> { "normal-user" })));
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BudgetSandboxContext>(
        options => options.UseNpgsql(builder.Configuration.GetValue<string>("PostgresDatabaseConnection")).UseSnakeCaseNamingConvention());

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BudgetSandboxContext>();
    db.Database.Migrate();
}

app.UseCors("corspolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
