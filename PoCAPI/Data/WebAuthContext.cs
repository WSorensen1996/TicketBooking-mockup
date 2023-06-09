﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoCAPI.Controllers; 
using System.Reflection.Emit;

namespace PoCAPI.Data;

public class WebAuthContext : IdentityDbContext<ApplicationUser>
{
    public WebAuthContext(DbContextOptions<WebAuthContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Auth");
        base.OnModelCreating(modelBuilder);


    }
}

