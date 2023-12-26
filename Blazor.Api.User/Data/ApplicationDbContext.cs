using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace Blazor.Api.User.Data;

public class ApplicationDbContext : IdentityDbContext, ISecurityKeyContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<KeyMaterial> SecurityKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new KeyMaterialMap());
        base.OnModelCreating(modelBuilder);
    }
}