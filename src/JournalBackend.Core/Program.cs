using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using JournalBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<JournalDbContext>(options => options.UseInMemoryDatabase("items"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Journal API",
        Description = "Backend API for a journaling web application.",
        Version = "v1",
    });

});

string? JwtKey = Environment.GetEnvironmentVariable("DOTNET_JWT_KEY");
string? JwtAudience = Environment.GetEnvironmentVariable("VITE_FIREBASE_PROJECT_ID");
string? JwtIssuer = $"https://securetoken.google.com/{JwtAudience}";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.Authority = JwtIssuer is not null ? JwtIssuer : "";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = JwtAudience is not null ? JwtAudience : "",
        ValidIssuer = JwtIssuer is not null ? JwtIssuer : "",
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(JwtKey is not null ? JwtKey : "")),
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Journal API v1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
