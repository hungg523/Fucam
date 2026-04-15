using Microsoft.EntityFrameworkCore;

namespace Fucam.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Registration> Registrations { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
    }
}
