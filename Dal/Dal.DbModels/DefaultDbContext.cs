using Dal.DbModels.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.DbModels
{
    public partial class DefaultDbContext : DbContext
    {
        public DefaultDbContext() { }

        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Login)
                    .HasDatabaseName("Unique_Users_Login")
                    .IsUnique();

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.RegistrationDate).HasColumnType("timestamp");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Session_User");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Session_Game");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasOne(d => d.Winner)
                    .WithMany()
                    .HasForeignKey(d => d.WinnerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Game_Winner");

                entity.Property(e => e.GameStartTime).HasColumnType("timestamp");
                entity.Property(e => e.GameEndTime).HasColumnType("timestamp");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
