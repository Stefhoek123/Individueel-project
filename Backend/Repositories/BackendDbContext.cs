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
        DbContextOptionsBuilder<BackendDbContext> optionsBuilder = new DbContextOptionsBuilder<BackendDbContext>();
        optionsBuilder.UseSqlServer("Server=mssqlstud.fhict.local;Database=dbi533446_s3indivi;User Id=dbi533446_s3indivi;Password=semester3;Encrypt=True;TrustServerCertificate=True");
        return new BackendDbContext(optionsBuilder.Options);
    }
}
