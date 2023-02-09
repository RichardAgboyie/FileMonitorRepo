using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Entities
{
    public partial class SftpDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        
        public SftpDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SftpDbContext(DbContextOptions<SftpDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<FileMonitor> FileMonitor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            var cnn = _configuration["ConnectionStrings:DBConnectionString"];
            optionsBuilder.UseSqlServer(cnn);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileMonitor>(entity =>
            {
                entity.HasKey(e => e.SourceFileId);

                entity.Property(e => e.SourceFileId).HasColumnName("SourceFileID");

                entity.Property(e => e.FileCopied).IsRequired();

                entity.Property(e => e.TimeCopied).HasColumnType("datetime");

                entity.Property(e => e.TimeMonitored).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
