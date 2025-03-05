using FitnessIntelligence.Model;
using Microsoft.EntityFrameworkCore;

namespace FitnessIntelligence.Models
{
    public class PalestraDBContext : DbContext
    {
        public DbSet<Utente> Utenti { get; set; }

        public PalestraDBContext(DbContextOptions<PalestraDBContext> options) : base(options) { }
    }
}
