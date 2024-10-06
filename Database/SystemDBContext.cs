using DataSetBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataSetBase.Database
{
    public class SystemDBContext : DbContext
    {
        public DbSet<MessageModel> Message { get; set; }
        public DbSet<ConversationModel> Conversation { get; set; }
        public DbSet<ConversationMessage> ConversationMessage { get; set; }
        public DbSet<CategoryModel> Category { get; set; }
        

        public bool StartUpCheck = false;

        public SystemDBContext(DbContextOptions<SystemDBContext> options) : base (options)
        {
            UpdateDatabaseSchema();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MessageModel>().ToTable("Message");
            modelBuilder.Entity<ConversationModel>().ToTable("Conversation");
            modelBuilder.Entity<ConversationMessage>().ToTable("ConversationMessage");
            modelBuilder.Entity<CategoryModel>().ToTable("Category");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Conversation.db");
        }

        public void EnsureDatabaseCreated()
        {
            Database.EnsureCreated();
        }

        public void ApplyMigrations()
        {
            Database.Migrate();
        }

        public void StartUp()
        {
            if(!StartUpCheck)
                this.Database.EnsureCreated();
            StartUpCheck = true;
        }

        public void UpdateDatabaseSchema()
        {
            try
            {
                var pendingMigrations = this.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    this.Database.Migrate();
                    Console.WriteLine("Database schema updated successfully.");
                }
                else
                {
                    Console.WriteLine("Database schema is up to date.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the database schema: {ex.Message}");
            }
        }

    }
}
