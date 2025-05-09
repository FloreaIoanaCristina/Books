using System.Text;
using Books.Data;
using Books.Entities;
using Books.Extensions;
using Books.Interfaces;
using Books.Middleware;
using Books.Services;
using Books.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();



// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
.WithOrigins("https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager= services.GetRequiredService<UserManager<AppUser>>();
    var roleManager= services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
    await  Seed.SeedUsers(userManager, roleManager);

}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await  SeedB.SeedBooks(context);

}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
