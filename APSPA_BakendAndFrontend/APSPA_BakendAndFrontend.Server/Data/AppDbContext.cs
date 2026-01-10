using Microsoft.EntityFrameworkCore;
using APSPA_BakendAndFrontend.Server.model;

namespace APSPA_BakendAndFrontend.Server.Data
{
   

    public class AppDbContext : DbContext
    {
        public  AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
          
        
        
        }
    
        // Tabelele tale din PostgreSQL
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Anomaly> Anomalies { get; set; }
    }
}
