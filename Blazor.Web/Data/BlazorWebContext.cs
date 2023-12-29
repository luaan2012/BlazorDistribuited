using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Blazor.Web.Models.BlazorWeb;

namespace Blazor.Web.Data
{
    public partial class BlazorWebContext : DbContext
    {
        public BlazorWebContext()
        {
        }

        public BlazorWebContext(DbContextOptions<BlazorWebContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Blazor.Web.Models.BlazorWeb.Client>()
              .Property(p => p.DateCreated)
              .HasColumnType("datetime2");

            builder.Entity<Blazor.Web.Models.BlazorWeb.Client>()
              .Property(p => p.DateDelete)
              .HasColumnType("datetime2");

            builder.Entity<Blazor.Web.Models.BlazorWeb.Client>()
              .Property(p => p.DateModificated)
              .HasColumnType("datetime2");

            builder.Entity<Blazor.Web.Models.BlazorWeb.AspNetUser>()
              .Property(p => p.LockoutEnd)
              .HasColumnType("datetimeoffset");
            this.OnModelBuilding(builder);
        }

        public DbSet<Blazor.Web.Models.BlazorWeb.Client> Clients { get; set; }

        public DbSet<Blazor.Web.Models.BlazorWeb.AspNetUser> AspNetUsers { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}