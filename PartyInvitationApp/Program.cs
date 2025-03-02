using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PartyInvitationApp.Data;
using PartyInvitationApp.Services;
using PartyInvitationApp.Models;

var builder = WebApplication.CreateBuilder(args);

// 1) Add MVC
builder.Services.AddControllersWithViews();

// 2) EF + DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3) Register EmailService
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Party}/{action=Index}/{id?}"
);

app.Run();
