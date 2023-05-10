using JWTAuthentication.API.Database.Context;
using JWTAuthentication.API.Interfaces;
using JWTAuthentication.API.Models;
using JWTAuthentication.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// get config
var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<JWTAuthenticationContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceDB")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<JWTAuthenticationContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = (string)config.GetValue(typeof(string), "JWT:ValidAudience"),
            ValidIssuer = (string)config.GetValue(typeof(string), "JWT:Issuer"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((string)config.GetValue(typeof(string), "JWT:Secret")))
        };
    });

builder.Services.AddTransient<IAuthenticateService, AuthenticateService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
