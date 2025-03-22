using FitnessIntelligence.Model;
using FitnessIntelligence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitnessIntelligence.Controllers
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

        [Authorize]
        [HttpGet("protected")]
        public async Task<ActionResult<List<Utente>>> GetUtenti([FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("L'email è obbligatoria.");
                }

                var utenti = await _context.Utenti
                    .Where(u => u.Email == email)
                    .ToListAsync();

                if (utenti.Count == 0)
                {
                    return NotFound("Nessun utente trovato con questa email.");
                }

                return Ok(utenti);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
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
        /*[Authorize]
        [HttpGet("allenamenti")]
        public async Task<ActionResult<List<Allenamento>>> GetAllenamenti()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var utente = await _context.Utenti.FirstOrDefaultAsync(u => u.Email == email);
            if (utente == null) return NotFound("Utente non trovato");

            var allenamenti = await _context.Allenamenti
                .Where(a => a.Obiettivo == utente.Ruolo)
                .ToListAsync();

            return Ok(allenamenti);
        }*/
    }
}
