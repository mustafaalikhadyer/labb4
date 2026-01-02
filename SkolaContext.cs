using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    public class SkolaContext : DbContext
    {
        public DbSet<Student> Studenter { get; set; } = null!;
        public DbSet<Personal> Personal { get; set; } = null!;
        public DbSet<Klass> Klasser { get; set; } = null!;
        public DbSet<Kurs> Kurser { get; set; } = null!;
        public DbSet<Betyg> Betyg { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=SkolaDB;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}