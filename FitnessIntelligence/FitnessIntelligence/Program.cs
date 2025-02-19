using Microsoft.EntityFrameworkCore;

namespace FitnessIntelligence
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registra DbContext prima di Build()
            builder.Services.AddDbContext<PalestraDBContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));

            // Abilita i controller
            builder.Services.AddControllers();

            // **Registra CORS prima di Build()**
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            // Costruisce l'applicazione
            var app = builder.Build();

            // **Abilita CORS nell'applicazione**
            app.UseCors("AllowAllOrigins");

            // Mappa i controller
            app.MapControllers();

            // Avvia l'applicazione
            app.Run();
        }
    }
}
