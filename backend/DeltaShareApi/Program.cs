//using AvantiPoint.MobileAuth;
//using DeltaShareApi.Data;
//using DeltaShareApi.Util;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Swashbuckle.AspNetCore.Filters;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.AddMobileAuth(auth =>
//{
//    auth.ConfigureDbTokenStore<UserContext>(options =>
//    {
//        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
//    });

//    auth.AddMobileAuthClaimsHandler<CustomClaimsHandler>();
//});

//builder.Services.AddIdentityApiEndpoints<IdentityUser>()
//    .AddEntityFrameworkStores<UserContext>();

//// Swagger auth setup
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//        Name = "Authorization",
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
//    });
//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();
//app.UseAuthentication();

//app.MapControllers();
//app.MapDefaultMobileAuthRoutes();
//app.MapIdentityApi<IdentityUser>();

//app.Run();

using System.Security.Claims;
using DeltaShareApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.Audience = builder.Configuration["Authentication:Audience"];
        o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
        o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"]
        };
    });

builder.Services.AddControllers();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("users/me/", (ClaimsPrincipal claimsPrincipal) =>
{
    return claimsPrincipal.Claims
        .ToDictionary(c => c.Type, c => c.Value);
}).RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
