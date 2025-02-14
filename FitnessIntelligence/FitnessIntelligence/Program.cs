using Microsoft.EntityFrameworkCore;

namespace FitnessIntelligence
{  
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PalestraDBContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));

            builder.Services.AddControllers();
            var app = builder.Build();

            app.MapControllers();
            app.Run();
        }
    }
}
