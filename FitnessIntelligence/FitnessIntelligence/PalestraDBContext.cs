using Microsoft.EntityFrameworkCore;

namespace FitnessIntelligence
{
    public class PalestraDBContext : DbContext
    {
        public DbSet<Utente> Utenti { get; set; }

        public PalestraDBContext(DbContextOptions<PalestraDBContext> options) : base(options) { }
    }
}
