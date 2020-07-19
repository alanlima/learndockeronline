using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace LearnDockerWebApp.Data
{
    public class LearnDockerDbContext : DbContext
    {

        private readonly ILogger<LearnDockerDbContext> _logger;

        public DbSet<RequestEntry> Requests { get; set; }

        public LearnDockerDbContext(
            DbContextOptions<LearnDockerDbContext> dbContextOptions,
            ILogger<LearnDockerDbContext> logger)
            : base(dbContextOptions)
        {
            _logger = logger;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureModel(modelBuilder.Entity<RequestEntry>());
        }

        public void LogEnvironmentVariables() {
            _logger.LogInformation("GetEnvironmentVariables: ");
            foreach (System.Collections.DictionaryEntry de in Environment.GetEnvironmentVariables())
                _logger.LogInformation("  {0} = {1}", de.Key, de.Value);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var userName = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "demo";
        //    var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "secret";
        //    var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "demo";
        //    var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";

        //    var connectionString = $"Host={dbHost};Database={dbName};Username={userName};Password={password}";
        //    _logger.LogInformation("Connection String: {ConnectionString}", connectionString);

        //    optionsBuilder.UseNpgsql(connectionString);
        //}

        private void ConfigureModel(EntityTypeBuilder<RequestEntry> builder)
        {
            builder.ToTable("requests").HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id")
                .UseSerialColumn();

            builder.Property(r => r.RequestedAt)
                .HasColumnName("requested_at");

            builder.Property(x => x.Ip)
                .HasColumnName("ip");

            builder.Property(x => x.Host)
                .HasColumnName("host");

            builder.Property(x => x.Path)
                .HasColumnName("path");
        }
        
    }

    public class RequestEntry
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string RequestedAt { get; set; }
    }
}