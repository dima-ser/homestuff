using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HomeStuff.Data;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using HomeStuff.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/").AllowAnonymousToFolder("/Auth");
});
builder.Services.AddDbContext<SqliteContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteContext") ?? throw new InvalidOperationException("Connection string 'SqliteContext' not found.")));
builder.Services.AddTransient<IItemService, ItemService>();

// authentication
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(cookieOptions => {
    cookieOptions.LoginPath = "/auth/login";
});

var app = builder.Build();

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SqliteContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseRequestLocalization(new string[] { "en-US", });

app.Run();
