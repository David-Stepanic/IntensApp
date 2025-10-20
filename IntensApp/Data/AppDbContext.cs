using IntensApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IntensApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Candidate> Candidates { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<CandidateSkill> CandidateSkills { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CandidateSkill>()
                .HasKey(cs => new { cs.CandidateId, cs.SkillId });

            modelBuilder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Candidate)
                .WithMany(c => c.CandidateSkills)
                .HasForeignKey(cs => cs.CandidateId);

            modelBuilder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Skill)
                .WithMany(s => s.CandidateSkills)
                .HasForeignKey(cs => cs.SkillId);

            modelBuilder.Entity<Candidate>()
                .Property(c => c.FullName)
                .IsRequired();

            modelBuilder.Entity<Candidate>()
                .Property(c => c.Email)
                .IsRequired();

            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Email)
                .IsUnique(); 

            modelBuilder.Entity<Candidate>()
                .Property(c => c.ContactNumber)
                .IsRequired();

            modelBuilder.Entity<Candidate>()
                .Property(c => c.DateOfBirth)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Skill>()
                .Property(s => s.Name)
                .IsRequired();

            modelBuilder.Entity<Skill>()
                .HasIndex(s => s.Name)
                .IsUnique(); 

            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "C# Programming" },
                new Skill { Id = 2, Name = "Java Programming" },
                new Skill { Id = 3, Name = "JavaScript" },
                new Skill { Id = 4, Name = "PHP" },
                new Skill { Id = 5, Name = "Golang" },
                new Skill { Id = 6, Name = "Rust" }
            );

            modelBuilder.Entity<Candidate>().HasData(
                new Candidate
                {
                    Id = 1,
                    FullName = "David Stepanic",
                    Email = "david@gmail.com",
                    ContactNumber = "061222333",
                    DateOfBirth = new DateTime(2000, 6, 9, 0, 0, 0, DateTimeKind.Utc)
                },
                new Candidate
                {
                    Id = 2,
                    FullName = "Milos Obilic",
                    Email = "milos@gmail.com",
                    ContactNumber = "069222333",
                    DateOfBirth = new DateTime(1992, 5, 10, 0, 0, 0, DateTimeKind.Utc)
                },
                new Candidate
                {
                    Id = 3,
                    FullName = "Lazar Hrebeljanovic",
                    Email = "lazar@gmail.com",
                    ContactNumber = "061999333",
                    DateOfBirth = new DateTime(1380, 6, 19, 0, 0, 0, DateTimeKind.Utc)
                },
                new Candidate
                {
                    Id = 4,
                    FullName = "Nikola Tesla",
                    Email = "nikola@gmail.com",
                    ContactNumber = "069222555",
                    DateOfBirth = new DateTime(1856, 5, 10, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<CandidateSkill>().HasData(
                new CandidateSkill { CandidateId = 1, SkillId = 1 },
                new CandidateSkill { CandidateId = 1, SkillId = 3 },
                new CandidateSkill { CandidateId = 2, SkillId = 2 },
                new CandidateSkill { CandidateId = 3, SkillId = 1 },
                new CandidateSkill { CandidateId = 4, SkillId = 6 },
                new CandidateSkill { CandidateId = 4, SkillId = 5 }
            );
        }
    }
}
