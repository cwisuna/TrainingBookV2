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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

            builder.Entity<ApplicationUser>().Property(u => u.Id).HasColumnName("UserID");
            builder.Entity<Role>().Property(r => r.Id).HasColumnName("RoleID");

            builder.Entity<ApplicationUser>()
            .HasOne(u => u.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentID)
            .OnDelete(DeleteBehavior.Restrict);
            
            
            // department table
            builder.Entity<Department>()
                .ToTable("Departments")
                .HasKey(d => d.DepartmentID);

            // team members table 
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

            // training steps table
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

            // revisions table
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

            // signed off table
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

            // training books table
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

            // training steps per user table
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
        }
    }
}
