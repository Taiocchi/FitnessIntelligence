using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessIntelligence.Model
{
    public class Utente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cognome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Column("Eta")]
        public int Eta { get; set; }

        [Required]
        public string Ruolo { get; set; }

    }
}