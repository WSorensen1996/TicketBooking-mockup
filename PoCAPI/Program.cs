using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using PoCAPI.Controllers;
using PoCAPI.Data;
using PoCAPI.Models;
using PoCAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;



builder.Services.AddDbContext<ordersDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});

builder.Services.AddDbContext<flightsDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});

builder.Services.AddDbContext<WebAuthContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});


builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<WebAuthContext>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IQueueService, QueueService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
