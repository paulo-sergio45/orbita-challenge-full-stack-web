using AcademicManagementApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagementApi.Data
{
    public class AcademicContext : DbContext
    {
        public AcademicContext(DbContextOptions<AcademicContext> options) : base(options)
        { }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.RA)
                .IsUnique();
        }
    }
}

