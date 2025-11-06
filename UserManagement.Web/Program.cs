using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Data;
using UserManagement.Services.Interfaces;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDataAccess()
    .AddDomainServices()
    .AddMarkdown()
    .AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseInMemoryDatabase("UserManagement"));
builder.Services.AddScoped<IDataContext, DataContext>();


builder.Services.AddScoped<ILogService, LogService>();

var app = builder.Build();

// Seed logs at startup
using (var scope = app.Services.CreateScope())
{
    var logService = scope.ServiceProvider.GetRequiredService<ILogService>();
    await logService.InitializeAsync();
}

app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
