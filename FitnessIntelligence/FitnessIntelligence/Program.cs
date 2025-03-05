using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Hosting;
using FitnessIntelligence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

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

            //abilita autenticazione e autorizzazione via token jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
                 {
                     x.RequireHttpsMetadata = false;
                     x.SaveToken = true;
                     x.TokenValidationParameters = new TokenValidationParameters()
                     {
                         ValidateIssuerSigningKey = true,
                         ValidateLifetime = true,
                         IssuerSigningKey = ConfigHelper.GetSymmetricSecurityKey(builder),
                         ValidAudience = builder.Configuration["Jwt:Audience"],
                         ValidIssuer = builder.Configuration["Jwt:Issuer"]
                     };
                 });

            //per usare authorize
            builder.Services.AddAuthorization();


            // Costruisce l'applicazione
            var app = builder.Build();

            //abilita restituzione pagine Web sotto wwroot
            app.UseStaticFiles();

            // **Abilita CORS nell'applicazione**
            app.UseCors("AllowAllOrigins");

            // Mappa i controller
            app.MapControllers();

            //abilitazione nell'applicazione di quanto configurato
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Avvia l'applicazione
            app.Run();
        }
    }

    public class ConfigHelper
    {
  
        public static string GetSecurityKey(WebApplicationBuilder builder)
        {
            string result = builder.Configuration["Jwt:Key"];
            return result;
        }

        public static byte[] GetSymmetricSecurityKeyAsBytes(WebApplicationBuilder builder)
        {
            var issuerSigningKey = GetSecurityKey(builder);
            byte[] data = Encoding.UTF8.GetBytes(issuerSigningKey);
            return data;
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(WebApplicationBuilder builder)
        {
            byte[] data = GetSymmetricSecurityKeyAsBytes(builder);
            var result = new SymmetricSecurityKey(data);
            return result;
        }

  
    }
}
