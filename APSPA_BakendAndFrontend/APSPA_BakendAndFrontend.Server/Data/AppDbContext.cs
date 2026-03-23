using APSPA_BakendAndFrontend.Server.model;
using Microsoft.EntityFrameworkCore;

namespace APSPA_BakendAndFrontend.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<UserHealthProfile> UserHealthProfiles => Set<UserHealthProfile>();
        public DbSet<ActivityRecord> ActivityRecords => Set<ActivityRecord>();
        public DbSet<AiModel> AiModels => Set<AiModel>();
        public DbSet<Prediction> Predictions => Set<Prediction>();
        public DbSet<AiRequestLog> AiRequestLogs => Set<AiRequestLog>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.Age).HasColumnName("age");
                entity.Property(e => e.Gender).HasColumnName("gender");
                entity.Property(e => e.WeightKg).HasColumnName("weight_kg").HasColumnType("numeric(8,2)");
                entity.Property(e => e.HeightCm).HasColumnName("height_cm").HasColumnType("numeric(8,2)");
                entity.Property(e => e.Role).HasColumnName("role");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_tokens");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Token).IsUnique();
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Token).HasColumnName("token");
                entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
                entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserHealthProfile>(entity =>
            {
                entity.ToTable("user_health_profiles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.RestingHeartRate).HasColumnName("resting_heart_rate");
                entity.Property(e => e.RecoveryScore).HasColumnName("recovery_score");
                entity.Property(e => e.BMI).HasColumnName("bmi").HasColumnType("numeric(8,2)");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.RecordedAt).HasColumnName("recorded_at");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserHealthProfiles)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ActivityRecord>(entity =>
            {
                entity.ToTable("activity_records");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.ActivityType).HasColumnName("activity_type");
                entity.Property(e => e.ActivityDurationMin).HasColumnName("activity_duration_min");
                entity.Property(e => e.ActivityDate).HasColumnName("activity_date");
                entity.Property(e => e.RestingHeartRate).HasColumnName("resting_heart_rate");
                entity.Property(e => e.RecoveryScore).HasColumnName("recovery_score");
                entity.Property(e => e.CaloriesBurned).HasColumnName("calories_burned");
                entity.Property(e => e.PerceivedIntensity).HasColumnName("perceived_intensity");
                entity.Property(e => e.WeatherConditions).HasColumnName("weather_conditions");
                entity.Property(e => e.MeasuredHeartRate).HasColumnName("measured_heart_rate");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.ActivityRecords)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AiModel>(entity =>
            {
                entity.ToTable("ai_models");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ModelName).HasColumnName("model_name");
                entity.Property(e => e.ModelVersion).HasColumnName("model_version");
                entity.Property(e => e.TargetMetric).HasColumnName("target_metric");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<Prediction>(entity =>
            {
                entity.ToTable("predictions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.ActivityRecordId).HasColumnName("activity_record_id");
                entity.Property(e => e.AiModelId).HasColumnName("ai_model_id");
                entity.Property(e => e.PredictedAvgHeartRate).HasColumnName("predicted_avg_heart_rate").HasColumnType("numeric(8,2)");
                entity.Property(e => e.EffortLevel).HasColumnName("effort_level");
                entity.Property(e => e.FatigueRisk).HasColumnName("fatigue_risk");
                entity.Property(e => e.Recommendation).HasColumnName("recommendation");
                entity.Property(e => e.Difference).HasColumnName("difference").HasColumnType("numeric(8,2)");
                entity.Property(e => e.IsAnomaly).HasColumnName("is_anomaly");
                entity.Property(e => e.PredictionStatus).HasColumnName("prediction_status");
                entity.Property(e => e.RawInputPayload).HasColumnName("raw_input_payload").HasColumnType("jsonb");
                entity.Property(e => e.RawOutputPayload).HasColumnName("raw_output_payload").HasColumnType("jsonb");
                entity.Property(e => e.ErrorMessage).HasColumnName("error_message");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CompletedAt).HasColumnName("completed_at");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Predictions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ActivityRecord)
                    .WithMany(a => a.Predictions)
                    .HasForeignKey(e => e.ActivityRecordId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.AiModel)
                    .WithMany(m => m.Predictions)
                    .HasForeignKey(e => e.AiModelId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<AiRequestLog>(entity =>
            {
                entity.ToTable("ai_request_logs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PredictionId).HasColumnName("prediction_id");
                entity.Property(e => e.Endpoint).HasColumnName("endpoint");
                entity.Property(e => e.HttpMethod).HasColumnName("http_method");
                entity.Property(e => e.RequestPayload).HasColumnName("request_payload").HasColumnType("jsonb");
                entity.Property(e => e.ResponsePayload).HasColumnName("response_payload").HasColumnType("jsonb");
                entity.Property(e => e.ResponseStatus).HasColumnName("response_status");
                entity.Property(e => e.DurationMs).HasColumnName("duration_ms");
                entity.Property(e => e.IsSuccess).HasColumnName("is_success");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasOne(e => e.Prediction)
                    .WithMany(p => p.AiRequestLogs)
                    .HasForeignKey(e => e.PredictionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("audit_logs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Action).HasColumnName("action");
                entity.Property(e => e.EntityName).HasColumnName("entity_name");
                entity.Property(e => e.EntityId).HasColumnName("entity_id");
                entity.Property(e => e.Details).HasColumnName("details");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.AuditLogs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
