using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using OrderQueueToDB.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Add your services here
        services.AddDbContext<ordersDbContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("DB", EnvironmentVariableTarget.Process);  
            options.UseSqlServer(connectionString);
        });
    })
    .Build();

host.Run();