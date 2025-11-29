using System.Text;
using Kinaru.Api.Endpoints;
using Kinaru.Api.Services.Implementations;
using Kinaru.Api.Services.Interfaces;
using Kinaru.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Database Context
builder.Services.AddDbContext<KinaruDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IMessagingService, MessagingService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddHttpContextAccessor();

// Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapAuthEndpoints();
app.MapPropertyEndpoints();
app.MapUserEndpoints();
app.MapFavoriteEndpoints();
app.MapReservationEndpoints();
app.MapMessagingEndpoints();
app.MapNotificationEndpoints();
app.MapAgentEndpoints();
app.MapImageEndpoints();

app.Run();
