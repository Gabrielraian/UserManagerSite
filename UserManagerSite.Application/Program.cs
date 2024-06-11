using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagerSite.Application.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserManagerSiteContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UserManagerSiteContext") ?? throw new InvalidOperationException("Connection string 'UserManagerSiteContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
