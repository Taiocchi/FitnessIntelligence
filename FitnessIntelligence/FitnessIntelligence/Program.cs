using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Google;

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

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://accounts.google.com";
                options.Audience = "il-tuo-client-id"; // Sostituiscilo con il tuo Google Client ID
            })
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Google:ClientId"];
                options.ClientSecret = builder.Configuration["Google:ClientSecret"];
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
