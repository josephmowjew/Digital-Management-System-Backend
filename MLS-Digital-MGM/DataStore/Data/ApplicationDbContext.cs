using DataStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Thread = DataStore.Core.Models.Thread;

namespace DataStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,Role,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            builder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<Role>()
                .HasBaseType<IdentityRole>()
                .ToTable("Roles");

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.Property(u => u.NormalizedEmail).HasMaxLength(200);
                entity.Property(u => u.Id).HasMaxLength(200);
                entity.Property(u => u.NormalizedUserName).HasMaxLength(200);
                entity.Property(u => u.UserName).IsUnicode(false);
                entity.Property(u => u.Email).IsUnicode(false);
                entity.Property(u => u.ConcurrencyStamp).HasMaxLength(200);
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
                entity.Property(u => u.NormalizedName).HasMaxLength(85);
                entity.Property(u => u.Id).HasMaxLength(85);
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.Property(u => u.UserId).HasMaxLength(85);
                entity.Property(u => u.RoleId).HasMaxLength(85);
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
                entity.Property(u => u.UserId).HasMaxLength(200);
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
                entity.Property(u => u.UserId).HasMaxLength(200);
                entity.Property(m => m.LoginProvider).HasMaxLength(85);
                entity.Property(m => m.ProviderDisplayName).HasMaxLength(85);
                entity.Property(m => m.LoginProvider).HasMaxLength(85);
                entity.Property(m => m.ProviderKey).HasMaxLength(85);


            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
                entity.Property(u => u.RoleId).HasMaxLength(200);
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
                entity.Property(u => u.UserId).HasMaxLength(85);
                entity.Property(u => u.Name).HasMaxLength(85);
                entity.Property(u => u.LoginProvider).HasMaxLength(85);
                entity.Property(u => u.Value).HasMaxLength(200);
            });

            

        builder.Entity<Committee>()
            .HasOne(c => c.Chairperson)
            .WithMany()
            .HasForeignKey(c => c.ChairpersonID);


            //builder.Entity<ApplicationUser>().Property(u => u.FullName).HasComputedColumnSql("[FirstName] + ' ' + [LastName]");


        }

        public DbSet<Department > Departments{ get; set; }
        public DbSet<AttachmentType> AttachmentTypes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<IdentityType> IdentityTypes { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<YearOfOperation> YearOfOperations { get; set; }
        public DbSet<LicenseApprovalLevel> LicenseApprovalLevels { get; set; }
        public DbSet<Firm> Firms { get; set; }
        public DbSet<ProbonoClient> ProbonoClients { get; set; }
        public DbSet<ProBonoApplication> ProBonoApplications { get; set; }
        public DbSet<ProBonoReport> ProBonoReports { get; set;}
        public DbSet<PropBonoReportFeedback> PropBonoReportFeedbacks { get; set; }
        public DbSet<QualificationType> QualificationTypes { get; set; }
        public DbSet<MemberQualification> MemberQualifications { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<LicenseApplication> LicenseApplications { get; set; }
        public DbSet<LicenseApplicationApproval> LicenseApplicationApprovals { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<ProBono> ProBonos { get; set; }
        public DbSet<LicenseApprovalHistory> LicenseApprovalHistories { get; set; }
        public DbSet<LicenseApprovalComment> LicenseApprovalComments { get; set; }
        public DbSet<CPDTraining> CPDTrainings { get; set; }
        public DbSet<CPDTrainingRegistration> CPDTrainingRegistrations { get; set; }
        public DbSet<CPDUnitsEarned> CPDUnitsEarned { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<PenaltyType> PenaltyTypes { get; set;}
        public DbSet<PenaltyPayment> PenaltyPayments { get; set; }
        public DbSet<Committee> Committees { get; set; } 
        public DbSet<CommitteeMembership> CommitteeMembers { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<InvoiceRequest> InvoiceRequests { get; set; }
        
        
        
    }
}
