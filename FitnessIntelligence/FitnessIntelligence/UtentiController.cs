using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UtentiController : ControllerBase
{
    private readonly IMongoCollection<Utente> _utentiCollection;

    public UtentiController()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("PalestraDB");
        _utentiCollection = database.GetCollection<Utente>("Utenti");
    }

    [HttpGet]
    public async Task<List<Utente>> Get() =>
        await _utentiCollection.Find(u => true).ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Create(Utente nuovoUtente)
    {
        await _utentiCollection.InsertOneAsync(nuovoUtente);
        return CreatedAtAction(nameof(Get), new { id = nuovoUtente.Id }, nuovoUtente);
    }
}
