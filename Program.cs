using Microsoft.EntityFrameworkCore;
using SmartQuiz_APP.Service;
using System;
using Microsoft.Extensions.Logging; // Add this using directive for ILogger

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- Configure Session State ---
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure userDbContext for login/signup and quiz attempts data
var userConnectionString = builder.Configuration.GetConnectionString("DevConnection"); // Using DevConnection as per your appsettings.json
builder.Services.AddDbContext<userDbContext>(options =>
    options.UseSqlServer(userConnectionString));

var app = builder.Build();

// --- NEW: Log the detected environment at runtime ---
// Ensure this is placed AFTER builder.Build() and BEFORE any 'if' checks for environment.
var logger = app.Services.GetRequiredService<ILogger<Program>>(); // Get a logger instance
logger.LogInformation($"Application running in environment: {app.Environment.EnvironmentName}");
// ----------------------------------------------------

// Configure the HTTP request pipeline.
// --- CRITICAL: This 'if' block determines error page behavior ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Use generic error page in non-development
    app.UseHsts();
}
else
{
    // In Development environment, use the Developer Exception Page
    // This provides detailed error information and stack traces.
    app.UseDeveloperExceptionPage();
}
// -------------------------------------------------------------

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- CRITICAL: UseSession MUST be between UseRouting and UseAuthorization ---
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
