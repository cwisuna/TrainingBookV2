using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainingBookV2.Models;

namespace TrainingBookV2.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TrainingStep> TrainingSteps { get; set; }
        public DbSet<TrainingStepRevision> TrainingStepRevisions { get; set; }
        public DbSet<TrainingStepSignOff> TrainingStepSignOffs { get; set; }
        public DbSet<UserTrainingBook> UserTrainingBooks { get; set; }
        public DbSet<UserTrainingStep> UserTrainingStep { get; set; }
        public DbSet<TrainingNote> TrainingNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.Id).HasColumnName("UserID");

                entity.HasOne(u => u.Department)
                      .WithMany(d => d.Users)
                      .HasForeignKey(u => u.DepartmentID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.UserRoles)
                      .WithOne()
                      .HasForeignKey(ur => ur.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.Property(r => r.Id).HasColumnName("RoleID");

                entity.HasMany(r => r.UserRoles)
                      .WithOne()
                      .HasForeignKey(ur => ur.RoleId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

            // Departments
            builder.Entity<Department>()
                .ToTable("Departments")
                .HasKey(d => d.DepartmentID);

            // TeamMembers
            builder.Entity<TeamMember>()
                .ToTable("TeamMembers")
                .HasKey(tm => tm.TeamMemberID);

            builder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany(u => u.TeamMemberships)
                .HasForeignKey(tm => tm.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TeamMember>()
                .HasOne(tm => tm.Department)
                .WithMany(d => d.TeamMembers)
                .HasForeignKey(tm => tm.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // TrainingSteps
            builder.Entity<TrainingStep>()
                .ToTable("TrainingSteps")
                .HasKey(ts => ts.StepID);

            builder.Entity<TrainingStep>()
                .HasOne(ts => ts.Department)
                .WithMany(d => d.TrainingSteps)
                .HasForeignKey(ts => ts.DepartmentID);

            builder.Entity<TrainingStep>()
                .HasOne(ts => ts.ModifiedByUser)
                .WithMany()
                .HasForeignKey(ts => ts.LastModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TrainingStep>()
                .HasOne(ts => ts.CompletedByUser)
                .WithMany()
                .HasForeignKey(ts => ts.CompletedByUserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TrainingStep>()
                .HasOne(ts => ts.SignedOffByUser)
                .WithMany()
                .HasForeignKey(ts => ts.SignedOffByUserID)
                .OnDelete(DeleteBehavior.Restrict);

            // TrainingStepRevisions
            builder.Entity<TrainingStepRevision>()
                .ToTable("TrainingStepRevisions")
                .HasKey(r => r.RevisionID);

            builder.Entity<TrainingStepRevision>()
                .HasOne(r => r.Step)
                .WithMany(s => s.Revisions)
                .HasForeignKey(r => r.StepID);

            builder.Entity<TrainingStepRevision>()
                .HasOne(r => r.ModifiedByUser)
                .WithMany()
                .HasForeignKey(r => r.ModifiedByUserID);

            // TrainingStepSignOff
            builder.Entity<TrainingStepSignOff>()
                .ToTable("TrainingStepSignOff")
                .HasKey(s => s.SignOffID);

            builder.Entity<TrainingStepSignOff>()
                .HasOne(s => s.Step)
                .WithMany(s => s.SignOffs)
                .HasForeignKey(s => s.StepID);

            builder.Entity<TrainingStepSignOff>()
                .HasOne(s => s.Manager)
                .WithMany()
                .HasForeignKey(s => s.ManagerID);

            // UserTrainingBooks
            builder.Entity<UserTrainingBook>()
                .ToTable("UserTrainingBooks")
                .HasKey(b => b.UserTrainingBookID);

            builder.Entity<UserTrainingBook>()
                .HasOne(b => b.User)
                .WithMany(u => u.TrainingBooks)
                .HasForeignKey(b => b.UserID);

            builder.Entity<UserTrainingBook>()
                .HasOne(b => b.Department)
                .WithMany(d => d.TrainingBooks)
                .HasForeignKey(b => b.DepartmentID);

            // UserTrainingSteps
            builder.Entity<UserTrainingStep>()
                .ToTable("UserTrainingSteps")
                .HasKey(uts => uts.UserTrainingStepID);

            builder.Entity<UserTrainingStep>()
                .HasOne(uts => uts.UserTrainingBook)
                .WithMany(b => b.TrainingSteps)
                .HasForeignKey(uts => uts.UserTrainingBookID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserTrainingStep>()
                .HasOne(uts => uts.Step)
                .WithMany(s => s.UserTrainingSteps)
                .HasForeignKey(uts => uts.StepID);

            //TrainingNotes
            builder.Entity<TrainingNote>(entity =>
            {
                entity.ToTable("TrainingNotes");
                entity.HasKey(e => e.TrainingNoteID);

                entity.Property(n => n.Note)
                .IsRequired()
                .HasMaxLength(5000);

                entity.Property(n => n.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

                entity.HasOne(n => n.UserTrainingStep)
                .WithMany(uts => uts.Notes)
                .HasForeignKey(n => n.UserTrainingStepID)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.Author)
                .WithMany()
                .HasForeignKey(n => n.AuthorID)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
