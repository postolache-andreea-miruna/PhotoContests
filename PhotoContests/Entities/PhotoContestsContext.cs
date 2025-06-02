using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace PhotoContests.Entities
{
    public class PhotoContestsContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>,
        UserRoles, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public PhotoContestsContext(DbContextOptions<PhotoContestsContext> options) :base(options) { }

        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photographer> Photographers { get; set;}
        public DbSet<Juror> Jurors { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UsersRoles { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Voting> Votings { get; set; }
        public DbSet<Assignement> Assignments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.HasMany(u => u.UsersRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Role>(b =>
            {
                b.HasMany(r => r.UsersRoles)
                    .WithOne(r => r.Role)
                   .HasForeignKey(ur => ur.RoleId)
                   .IsRequired();
            });

            //Type - Competition (1-M)
            builder.Entity<Type>()
                .HasMany(t => t.Competitions)
                .WithOne(c => c.Type);

            //Nationality - Photographer (1-M)
            builder.Entity<Nationality>()
                .HasMany(n => n.Photographers)
                .WithOne(p => p.Nationality);

            //Photographer - Photo (1-M)
            builder.Entity<Photographer>()
                .HasMany(p => p.Photos)
                .WithOne(ph => ph.Photographer);

            //Photo - Section - Competition (M-M-M) => Participation
            builder.Entity<Participation>()
                .HasKey(p => new { p.idPhoto, p.idCompetition, p.idSection });

            builder.Entity<Participation>()
                .HasOne(p => p.Photo)
                .WithMany(ph => ph.Participations)
                .HasForeignKey(p => p.idPhoto);

            builder.Entity<Participation>()
                .HasOne(p => p.Section)
                .WithMany(s => s.Participations)
                .HasForeignKey(p => p.idSection);

            builder.Entity<Participation>()
                .HasOne(p => p.Competition)
                .WithMany(c => c.Participations)
                .HasForeignKey(p => p.idCompetition);

            // User - Notification (1-M)
            builder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User);

            // User - Message (1-M)
            builder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(m => m.User);

            // Photographer - Juror (M-M) => Video
            /* builder.Entity<Video>()
                 .HasKey(v => new { v.idPhotographer, v.idJuror, v.date });

             builder.Entity<Video>()
                 .HasOne(v => v.Photographer)
                 .WithMany(p => p.Videos)
                 .HasForeignKey(v => v.idPhotographer);

             builder.Entity<Video>()
                 .HasOne(v => v.Juror)
                 .WithMany(j => j.Videos)
                 .HasForeignKey(v => v.idJuror)
                 .OnDelete(DeleteBehavior.NoAction);*/

            //Juror - Video (1-M)
            builder.Entity<Video>()
                .HasKey(v => v.idVideo);

            builder.Entity<Juror>()
                .HasMany(j => j.Videos)
                .WithOne(v => v.Juror);

            //Video - Participation (1-1)
            builder.Entity<Video>()
                .HasOne(v => v.Participation)
                .WithOne(p => p.Video)
                .HasForeignKey<Video>(v => new { v.idPhoto, v.idCompetition, v.idSection })
                .OnDelete(DeleteBehavior.NoAction);

            // Juror - Competition (M-M) => Assignement
            builder.Entity<Assignement>()
                .HasKey(a => new {a.idUser,a.idCompetition});

            builder.Entity<Assignement>()
                .HasOne(a => a.Juror)
                .WithMany(j => j.Assignements)
                .HasForeignKey(a => a.idUser);

            builder.Entity<Assignement>()
                .HasOne(a => a.Competition)
                .WithMany(c => c.Assignements)
                .HasForeignKey(a => a.idCompetition);

            // Juror - Participation (M-M) => Review
            builder.Entity<Review>()
                .HasOne(r => r.Participation)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => new { r.idPhoto, r.idCompetition, r.idSection })
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Review>()
                .HasOne(r => r.Juror)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.idUser);

            // User - Participation (M-M) => Voting
            builder.Entity<Voting>()
               .HasOne(v => v.Participation)
               .WithMany(p => p.Votings)
               .HasForeignKey(v => new { v.idPhoto, v.idCompetition, v.idSection })
               .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Voting>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votings)
                .HasForeignKey(v => v.idUser);

            //User - Participation (M-M) => Report
            builder.Entity<Report>()
                .HasOne(r => r.Participation)
                .WithMany(p => p.Reports)
                .HasForeignKey(r => new { r.idPhoto, r.idCompetition, r.idSection })
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.idUser);

        }

    }
}
