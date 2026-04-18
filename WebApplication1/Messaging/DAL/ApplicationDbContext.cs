using Messaging.Models;
using Microsoft.EntityFrameworkCore;

namespace Messaging.DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<AuthUser> AuthUsers { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMembership> GroupMemberships { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<GroupMessage> GroupMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.User1)
            .WithMany(u => u.Chats) // Assuming User1 is part of Chats list
            .HasForeignKey(c => c.User1Id)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes if needed

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.User2)
            .WithMany() // No navigation property for User2 on User
            .HasForeignKey(c => c.User2Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}