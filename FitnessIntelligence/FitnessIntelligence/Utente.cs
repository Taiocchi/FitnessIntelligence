using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessIntelligence
{
    public class Utente
    {
        [Key] // Indica che questa è la chiave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremento
        public int Id { get; set; }

        [Required] // Rende il campo obbligatorio
        public string Nome { get; set; }

        [Range(1, 120)] // Limita l'età a un range plausibile
        public int Età { get; set; }

        [Required]
        public string Ruolo { get; set; } // Studente, Docente, Allenatore
    }

}