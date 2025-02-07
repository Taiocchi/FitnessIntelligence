using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Utente
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Nome { get; set; }
    public int Età { get; set; }
    public string Ruolo { get; set; } // Studente, Docente, Allenatore
}
