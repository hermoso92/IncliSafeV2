using Microsoft.EntityFrameworkCore;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafeApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<DobackData> DobackData { get; set; } = null!;
        public DbSet<Cycle> Cycles { get; set; } = null!;
        public DbSet<Alert> Alerts { get; set; } = null!;
        public DbSet<Vehiculo> Vehiculos { get; set; } = null!;
        public DbSet<DobackAnalysis> DobackAnalyses { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<PatternDetection> PatternDetections { get; set; } = null!;
        public DbSet<License> Licenses { get; set; } = null!;
        public DbSet<KnowledgePattern> KnowledgePatterns { get; set; } = null!;
        public DbSet<Inspeccion> Inspecciones { get; set; } = null!;
        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; } = null!;
        public DbSet<NotificationSettings> NotificationSettings { get; set; } = null!;
        public DbSet<Anomaly> Anomalies { get; set; } = null!;
        public DbSet<TrendAnalysis> TrendAnalysis { get; set; } = null!;
        public DbSet<AnalysisResult> AnalysisResults { get; set; } = null!;
        public DbSet<DetectedPattern> DetectedPatterns { get; set; } = null!;
        public DbSet<AnalysisPrediction> AnalysisPredictions { get; set; } = null!;
        public DbSet<IncliSafe.Shared.Models.Entities.Prediction> MaintenancePredictions { get; set; } = null!;
        public DbSet<VehicleAlert> VehicleAlerts { get; set; } = null!;
        public DbSet<VehicleMetrics> VehicleMetrics { get; set; } = null!;
        public DbSet<AlertSettings> AlertSettings { get; set; } = null!;
        public DbSet<IncliSafe.Shared.Models.Analysis.Prediction> Predictions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración global de fechas
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("datetime2");
                    }
                }
            }

            // Consolidar la configuración de Usuario en un solo lugar
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(e => e.Id);
                
                // Índices
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
                
                // Propiedades
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Rol).IsRequired();
                entity.Property(e => e.Activo).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.PasswordHash).IsRequired();

                // Relaciones
                entity.HasMany(u => u.Vehiculos)
                    .WithOne(v => v.Owner)
                    .HasForeignKey(v => v.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Agregar índices de rendimiento
            modelBuilder.Entity<DobackData>()
                .HasIndex(d => d.Timestamp);

            modelBuilder.Entity<DobackAnalysis>()
                .HasIndex(d => new { d.VehicleId, d.Timestamp });

            modelBuilder.Entity<Notification>()
                .HasIndex(n => new { n.UserId, n.CreatedAt });

            modelBuilder.Entity<PatternDetection>()
                .HasIndex(p => new { p.VehicleId, p.DetectionTime });

            // Configuración de eliminación en cascada
            modelBuilder.Entity<Vehiculo>()
                .HasMany(v => v.Inspecciones)
                .WithOne(i => i.Vehiculo)
                .HasForeignKey(i => i.VehiculoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DobackAnalysis>()
                .HasMany(d => d.Data)
                .WithOne(d => d.Analysis)
                .HasForeignKey(d => d.DobackAnalysisId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de campos requeridos y longitudes máximas
            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(e => e.Modelo)
                    .IsRequired();
                    
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.License)
                    .WithOne(p => p.Vehicle)
                    .HasForeignKey<License>(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.Placa).IsUnique();

                entity.HasOne(v => v.Owner)
                    .WithMany(u => u.Vehiculos)
                    .HasForeignKey(v => v.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DobackData>(entity =>
            {
                entity.Property(e => e.StabilityIndex)
                    .HasColumnType("float");
                
                entity.Property(e => e.SafetyScore)
                    .HasColumnType("float");
                
                entity.Property(e => e.MaintenanceScore)
                    .HasColumnType("float");
                
                entity.HasOne(d => d.Analysis)
                    .WithMany(a => a.Data)
                    .HasForeignKey(d => d.DobackAnalysisId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Cycle)
                    .WithMany(c => c.DobackData)
                    .HasForeignKey(d => d.CycleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Cycle>(entity =>
            {
                entity.Property(e => e.StabilityIndex)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.SafetyScore)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.Status)
                    .IsRequired();
            });

            modelBuilder.Entity<Alert>(entity =>
            {
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Severity).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                
                entity.HasOne(a => a.Vehicle)
                    .WithMany()
                    .HasForeignKey(a => a.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Alerts)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(n => n.Vehicle)
                    .WithMany()
                    .HasForeignKey(n => n.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.User)
                    .WithMany()
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<KnowledgePattern>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Pattern)
                    .IsRequired();
                
                entity.Property(e => e.Confidence)
                    .HasColumnType("decimal(18,4)");
            });

            modelBuilder.Entity<Inspeccion>(entity =>
            {
                entity.Property(e => e.Score)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(i => i.Inspector)
                    .WithMany()
                    .HasForeignKey(i => i.InspectorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BlacklistedToken>(entity =>
            {
                entity.Property(e => e.Token)
                    .IsRequired();
                
                entity.Property(e => e.RevokedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.Property(e => e.ExpiresAt)
                    .IsRequired();
            });

            modelBuilder.Entity<NotificationSettings>(entity =>
            {
                entity.Property(e => e.StabilityThreshold)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.SafetyThreshold)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.EnableNotifications)
                    .HasDefaultValue(true);
                
                entity.HasOne(n => n.Vehiculo)
                    .WithOne(v => v.NotificationSettings)
                    .HasForeignKey<NotificationSettings>(n => n.VehiculoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DobackAnalysis>(entity =>
            {
                entity.Property(e => e.Confidence)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.StabilityIndex)
                    .HasColumnType("decimal(18,4)");
                
                entity.HasOne(d => d.Result)
                    .WithOne(r => r.DobackAnalysis)
                    .HasForeignKey<AnalysisResult>(r => r.DobackAnalysisId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.DetectedPatterns)
                    .WithOne(p => p.DobackAnalysis)
                    .HasForeignKey(p => p.DobackAnalysisId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.Predictions)
                    .WithOne(p => p.DobackAnalysis)
                    .HasForeignKey(p => p.DobackAnalysisId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TrendAnalysis>(entity =>
            {
                entity.HasMany(t => t.Predictions)
                    .WithOne(p => p.TrendAnalysis)
                    .HasForeignKey(p => p.TrendAnalysisId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.Anomalies)
                    .WithOne(a => a.TrendAnalysis)
                    .HasForeignKey(a => a.TrendAnalysisId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Anomaly>(entity =>
            {
                entity.Property(e => e.DetectedAt).IsRequired();
                entity.Property(e => e.Severity).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                
                entity.Property(e => e.ExpectedValue)
                    .HasColumnType("float");
                
                entity.Property(e => e.ActualValue)
                    .HasColumnType("float");
                
                entity.Property(e => e.Deviation)
                    .HasColumnType("float");

                entity.HasOne(a => a.Vehicle)
                    .WithMany()
                    .HasForeignKey(a => a.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AnalysisResult>(entity =>
            {
                entity.Property(e => e.StabilityScore)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.SafetyScore)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.EfficiencyScore)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.MaintenanceScore)
                    .HasColumnType("decimal(18,4)");
            });

            modelBuilder.Entity<DetectedPattern>(entity =>
            {
                entity.Property(e => e.ConfidenceScore)
                    .HasColumnType("float");
                
                entity.HasOne(d => d.Analysis)
                    .WithMany(a => a.DetectedPatterns)
                    .HasForeignKey(d => d.DobackAnalysisId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AnalysisPrediction>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.PredictionType)
                    .IsRequired()
                    .HasConversion<string>();
                    
                entity.Property(e => e.RiskLevel)
                    .IsRequired()
                    .HasConversion<string>();
                    
                entity.Property(e => e.Probability)
                    .HasColumnType("decimal(5,4)");
                    
                entity.Property(e => e.Recommendations)
                    .HasMaxLength(2000);
                    
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                    
                entity.HasOne(d => d.Vehicle)
                    .WithMany()
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(e => new { e.VehicleId, e.PredictionType, e.CreatedAt });
            });

            modelBuilder.Entity<PatternDetection>(entity =>
            {
                entity.Property(e => e.ConfidenceScore)
                    .HasColumnType("decimal(18,4)");
                
                entity.HasOne(p => p.KnowledgePattern)
                    .WithMany()
                    .HasForeignKey(p => p.KnowledgePatternId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<License>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasConversion<string>();
                    
                entity.Property(e => e.ExpirationDate)
                    .IsRequired();
                    
                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });

            modelBuilder.Entity<VehicleAlert>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                    
                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(1000);
                    
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasConversion<string>();
                    
                entity.Property(e => e.Severity)
                    .IsRequired()
                    .HasConversion<string>();
                    
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                    
                entity.Property(e => e.IsRead)
                    .HasDefaultValue(false);
                    
                entity.HasOne(d => d.Vehicle)
                    .WithMany()
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<VehicleMetrics>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.AverageStabilityScore)
                    .HasColumnType("decimal(5,4)");
                    
                entity.Property(e => e.AverageSafetyScore)
                    .HasColumnType("decimal(5,4)");
                    
                entity.HasOne(d => d.Vehicle)
                    .WithOne()
                    .HasForeignKey<VehicleMetrics>(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(e => e.VehicleId)
                    .IsUnique();
            });

            modelBuilder.Entity<AlertSettings>(entity =>
            {
                entity.Property(e => e.StabilityThreshold)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.SafetyThreshold)
                    .HasColumnType("decimal(18,4)");
                
                entity.Property(e => e.EnableNotifications)
                    .HasDefaultValue(true);
                
                entity.HasOne(n => n.Vehicle)
                    .WithOne(v => v.AlertSettings)
                    .HasForeignKey<AlertSettings>(n => n.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Prediction>(entity =>
            {
                entity.Property(e => e.PredictionType).IsRequired();
                entity.Property(e => e.RiskLevel).IsRequired();
                entity.Property(e => e.Probability).IsRequired();
                entity.Property(e => e.Recommendations).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}
