namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data.Models;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                 .Entity<PlayerStatistic>(entity =>
                {
                    entity.HasKey(k => new { k.GameId, k.PlayerId });

                    entity.HasOne(e => e.Game)
                     .WithMany(g => g.PlayerStatistics)
                     .HasForeignKey(e => e.GameId);

                    entity.HasOne(e => e.Player)
                        .WithMany(p => p.PlayerStatistics)
                        .HasForeignKey(e => e.PlayerId);
                });

            modelBuilder
                .Entity<Team>(entity =>
                {
                    entity.HasKey(e => e.TeamId);

                    entity
                    .HasOne(e => e.PrimaryKitColor)
                    .WithMany(pc => pc.PrimaryKitTeams)
                    .HasForeignKey(k => k.PrimaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                    entity
                    .HasOne(e => e.SecondaryKitColor)
                    .WithMany(pc => pc.SecondaryKitTeams)
                    .HasForeignKey(k => k.SecondaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(e => e.Town)
                    .WithMany(t => t.Teams)
                    .HasForeignKey(e => e.TownId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder
                .Entity<Town>(entity =>
                {
                    entity.HasKey(k => k.TownId);

                    entity
                    .HasOne(e => e.Country)
                    .WithMany(t => t.Towns)
                    .HasForeignKey(k => k.CountryId);
                });

            modelBuilder
                .Entity<Game>(entity =>
                {
                    entity.HasKey(e => e.GameId);

                    entity
                    .HasOne(e => e.HomeTeam)
                    .WithMany(p => p.HomeGames)
                    .HasForeignKey(k => k.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                    entity
                    .HasOne(e => e.AwayTeam)
                    .WithMany(p => p.AwayGames)
                    .HasForeignKey(k => k.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity<Bet>(entity =>
            {
                entity.HasKey(e => e.BetId);

                entity.HasOne(e => e.Game)
                .WithMany(e => e.Bets)
                .HasForeignKey(e => e.GameId);

                entity.HasOne(e => e.User)
                .WithMany(e => e.Bets)
                .HasForeignKey(e => e.UserId);

            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.HasOne(e => e.Position)
                .WithMany(e => e.Players)
                .HasForeignKey(e => e.PositionId);

                entity.HasOne(e => e.Team)
               .WithMany(e => e.Players)
               .HasForeignKey(e => e.TeamId);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.ColorId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });

            modelBuilder.Entity<Bet>(entity =>
            {
                entity.HasKey(e => e.BetId);

                entity.HasOne(e => e.Game)
                .WithMany(e => e.Bets)
                .HasForeignKey(e => e.GameId);

                entity.HasOne(e => e.User)
                .WithMany(e => e.Bets)
                .HasForeignKey(e => e.UserId);

            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.PositionId);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.GameId);

                entity.HasOne(e => e.HomeTeam)
                    .WithMany(e => e.HomeGames)
                    .HasForeignKey(e => e.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AwayTeam)
                   .WithMany(e => e.AwayGames)
                   .HasForeignKey(e => e.AwayTeamId)
                   .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
