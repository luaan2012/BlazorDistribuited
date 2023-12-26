using Microsoft.EntityFrameworkCore;
using clientModel = Blazor.Api.Client.Models;

namespace Blazor.Api.Client.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<clientModel.Client> Client { get; set; }
    }
}
