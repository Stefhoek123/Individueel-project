using Models;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Repositories;

public class BackendDbContext : DbContext, IDesignTimeDbContextFactory<BackendDbContext>
{
    public BackendDbContext()
    {
    }

    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
    {
    }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Family> Families { get; set; }
    public DbSet<ImagePost> ImagePosts { get; set; }
    public DbSet<TextPost> TextPosts { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public BackendDbContext CreateDbContext(string[] args)
    {
        // Get the DB_PASSWORD from the environment variables
       var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

        if (string.IsNullOrEmpty(dbPassword))
        {
            throw new InvalidOperationException("DB_PASSWORD environment variable is not set.");
        }

        // Build the connection string dynamically with the password
        var connectionString = $"Server=mssqlstud.fhict.local;Database=dbi533446_s3indivi;User Id=dbi533446_s3indivi;Password={dbPassword};Encrypt=True;TrustServerCertificate=True";

        // Configure the DbContext options with the updated connection string
        var optionsBuilder = new DbContextOptionsBuilder<BackendDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new BackendDbContext(optionsBuilder.Options);
    }
}
