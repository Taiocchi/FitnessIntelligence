using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessIntelligence
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtentiController : ControllerBase
    {
        private readonly PalestraDBContext _context;

        public UtentiController(PalestraDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Utente>>> Get()
        {
            return await _context.Utenti.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Utente nuovoUtente)
        {
            // Log dei dati ricevuti
            Console.WriteLine($"Ricevuto utente: {nuovoUtente.Nome}, {nuovoUtente.Cognome}, {nuovoUtente.Email}, {nuovoUtente.Eta}, {nuovoUtente.Ruolo}");

            _context.Utenti.Add(nuovoUtente);
            await _context.SaveChangesAsync();

            // Log per vedere se l'utente è stato aggiunto
            Console.WriteLine($"Utente aggiunto con ID: {nuovoUtente.Id}");

            return CreatedAtAction(nameof(Get), new { id = nuovoUtente.Id }, nuovoUtente);
        }
    }
}
