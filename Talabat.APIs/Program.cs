using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helper;
using Talabat.APIs.MiddleWare;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Data.Identity;
using Talabat.Service;
using Talabat.Service.AuthService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddApplicationServicies();
builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(connection);
});

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});

builder.Services.AddIdentity<AppUser , IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));

builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:ValidIsssure"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"] ?? string.Empty)),
    };
});

var app = builder.Build();

using var Scope = app.Services.CreateScope();
var Services = Scope.ServiceProvider;
var _dbContext = Services.GetRequiredService<StoreContext>();
var _IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();

var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

try
{
   await _dbContext.Database.MigrateAsync();  //Update-DataBase
   await StoreContextSeed.SeedingAsync(_dbContext);  // DataSeeding
   await _IdentityDbContext.Database.MigrateAsync();

    var _userManger = Services.GetRequiredService<UserManager<AppUser>>(); // Explicitly 
    await AppIdentityDbContextSeed.SeedUserAsync(_userManger); // Data Seeding
}
catch (Exception ex)
{

    var Logger = LoggerFactory.CreateLogger<Program>();
    Logger.LogError(ex, "an error occured during applying Migration");
}


// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerMiddleware();
}

app.UseStatusCodePagesWithReExecute("/Errors/{0}");

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
