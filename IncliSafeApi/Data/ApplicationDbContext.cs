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

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios", tb => tb.IsTemporal());
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
                
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasMany(u => u.Vehiculos)
                    .WithOne(v => v.Usuario)
                    .HasForeignKey(v => v.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.ToTable("Vehiculos");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Placa).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Marca).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Modelo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Estado).IsRequired();

                entity.HasOne(v => v.Usuario)
                    .WithMany(u => u.Vehiculos)
                    .HasForeignKey(v => v.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(v => v.Inspecciones)
                    .WithOne(i => i.Vehiculo)
                    .HasForeignKey(i => i.VehiculoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.Licenses)
                    .WithOne(l => l.Vehiculo)
                    .HasForeignKey(l => l.VehiculoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DobackData>(entity =>
            {
                entity.ToTable("DobackData");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.StabilityIndex).HasColumnType("decimal(18,4)");
                entity.Property(e => e.SafetyScore).HasColumnType("decimal(18,4)");
                entity.Property(e => e.MaintenanceScore).HasColumnType("decimal(18,4)");
                
                entity.HasOne(d => d.Analysis)
                    .WithMany(a => a.Data)
                    .HasForeignKey(d => d.DobackAnalysisId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Cycle)
                    .WithMany(c => c.DobackData)
                    .HasForeignKey(d => d.CycleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Cycle>(entity =>
            {
                entity.ToTable("Cycles");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.StabilityIndex).HasColumnType("decimal(18,4)");
                entity.Property(e => e.SafetyScore).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Status).IsRequired();
            });

            modelBuilder.Entity<Alert>(entity =>
            {
                entity.ToTable("Alerts");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notifications");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<KnowledgePattern>(entity =>
            {
                entity.ToTable("KnowledgePatterns");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Pattern).IsRequired();
                entity.Property(e => e.Confidence).HasColumnType("decimal(18,4)");
            });

            modelBuilder.Entity<Inspeccion>(entity =>
            {
                entity.ToTable("Inspecciones");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Score).HasColumnType("decimal(18,2)");

                entity.HasOne(i => i.Inspector)
                    .WithMany()
                    .HasForeignKey(i => i.InspectorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BlacklistedToken>(entity =>
            {
                entity.ToTable("BlacklistedTokens");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired();
                entity.Property(e => e.RevokedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.ExpiresAt).IsRequired();
            });

            modelBuilder.Entity<NotificationSettings>(entity =>
            {
                entity.ToTable("NotificationSettings");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.StabilityThreshold).HasColumnType("decimal(18,4)");
                entity.Property(e => e.SafetyThreshold).HasColumnType("decimal(18,4)");
                entity.Property(e => e.EnableNotifications).HasDefaultValue(true);
                
                entity.HasOne(n => n.Vehiculo)
                    .WithOne(v => v.NotificationSettings)
                    .HasForeignKey<NotificationSettings>(n => n.VehiculoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DobackAnalysis>(entity =>
            {
                entity.ToTable("DobackAnalyses");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Confidence).HasColumnType("decimal(18,4)");
                entity.Property(e => e.StabilityIndex).HasColumnType("decimal(18,4)");
                
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
                entity.ToTable("TrendAnalysis");
                entity.HasKey(e => e.Id);

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
                entity.ToTable("Anomalies");
                entity.HasKey(e => e.Id);
                
                entity.HasOne(a => a.Cycle)
                    .WithMany(c => c.Anomalies)
                    .HasForeignKey(a => a.CycleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.TrendAnalysis)
                    .WithMany(t => t.Anomalies)
                    .HasForeignKey(a => a.TrendAnalysisId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AnalysisResult>(entity =>
            {
                entity.ToTable("AnalysisResults");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.StabilityScore).HasColumnType("decimal(18,4)");
                entity.Property(e => e.SafetyScore).HasColumnType("decimal(18,4)");
                entity.Property(e => e.EfficiencyScore).HasColumnType("decimal(18,4)");
                entity.Property(e => e.MaintenanceScore).HasColumnType("decimal(18,4)");
                
                // La relación con DobackAnalysis se configura desde el lado de DobackAnalysis
            });

            modelBuilder.Entity<DetectedPattern>(entity =>
            {
                entity.ToTable("DetectedPatterns");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.ConfidenceScore).HasColumnType("decimal(18,4)");
                
                entity.HasOne(d => d.DobackAnalysis)
                    .WithMany(a => a.DetectedPatterns)
                    .HasForeignKey(d => d.DobackAnalysisId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PatternDetection>(entity =>
            {
                entity.ToTable("PatternDetections");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.ConfidenceScore).HasColumnType("decimal(18,4)");
                
                entity.HasOne(p => p.KnowledgePattern)
                    .WithMany()
                    .HasForeignKey(p => p.KnowledgePatternId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
