using Microsoft.EntityFrameworkCore;
using Curotec.Domain.Entities;

namespace Curotec.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public ApplicationDbContext() { }

    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //TODO: move each one of this mapping into its own file
        modelBuilder.Entity<TodoList>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.HasMany(e => e.Items)
                  .WithOne()
                  .HasForeignKey(e => e.ParentListId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd().IsRequired();

            entity.Property(u => u.Name)
                  .IsRequired()
                  .HasMaxLength(250);

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(250);
            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.PasswordHash)
                  .IsRequired();
        });

        modelBuilder.Entity<User>().HasData(
            new
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "johndoe@example.com",
                PasswordHash = "hashed_password"
            }
        );

    }
} 