using BL.Api;
using BL.Services;
using DAL.Api;
using DAL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddScoped<IUserDal>(_ => new UserDal(connectionString));
builder.Services.AddScoped<IPropertyDal>(_ => new PropertyDal(connectionString));
builder.Services.AddScoped<IInvoiceDal>(_ => new InvoiceDal(connectionString));
builder.Services.AddScoped<IUserBl, UserBl>();
builder.Services.AddScoped<IPropertyBl, PropertyBl>();
builder.Services.AddScoped<IInvoiceBl, InvoiceBl>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddControllers();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()));

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
