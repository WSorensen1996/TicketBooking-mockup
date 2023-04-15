using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoCWebApp.Data;
using PoCWebApp.Models;
using PoCWebApp.Areas.Identity.Data;
using PoCWebApp.Services; 

var builder = WebApplication.CreateBuilder(args);

// Session setup
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});


builder.Services.AddSingleton<List<Dictionary<string, object>>>(new List<Dictionary<string, object>>());
builder.Services.AddSingleton<List<ordersDTO>>(new List<ordersDTO>());
builder.Services.AddSingleton<BookingsRepository>();
builder.Services.AddSingleton<FlightRepository>();
builder.Services.AddSingleton<CostumerHandler>();


builder.Services.AddDbContext<WebAuthContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<WebAuthContext>();


// Configering paswword requierments
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
}); 

builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Sessoin config 
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
