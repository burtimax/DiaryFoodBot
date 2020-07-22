using FoodDiaryBot.DataBase.Models;
using FoodDiaryBot.DataBase.Models;
using JustDoItBot.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDiaryBot.DataBase.Context
{
    public class BotDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ChatState> ChatStates { get; set; }
        public DbSet<Marathon> Marathons { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Weight> Weights { get; set; }
        public DbSet<Water> Waters { get; set; }
        public DbSet<QueryToSupport> QueryToSupport { get; set; }



        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region ManyToMany_MarathonUser
            modelBuilder.Entity<MarathonUser>()
                .HasKey(t => new {t.MarathonId, t.UserId});

            modelBuilder.Entity<MarathonUser>()
                .HasOne(mu => mu.User)
                .WithMany(m => m.Marathons)
                .HasForeignKey(mk => mk.MarathonId);

            modelBuilder.Entity<MarathonUser>()
                .HasOne(mm => mm.Marathon)
                .WithMany(u => u.Users)
                .HasForeignKey(mu => mu.UserId);
            #endregion

            #region AutoDatetime

            modelBuilder.Entity<User>()
                .Property(p => p.CreateTime)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Food>()
                .Property(p => p.CreateTime)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Marathon>()
                .Property(p => p.CreateTime)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Message>()
                .Property(p => p.CreateTime)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Water>()
                .Property(p => p.CreateTime)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Weight>()
                .Property(p => p.CreateTime)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<QueryToSupport>()
                .Property(p => p.CreateTime)
                .HasDefaultValueSql("GETDATE()");
            #endregion



        }
    }
}
