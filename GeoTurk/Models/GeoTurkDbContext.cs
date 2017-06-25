using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using GeoTurk.Helpers;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure.Annotations;

namespace GeoTurk.Models
{
    public class GeoTurkDbContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public DbSet<HIT> HITs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskChoise> TaskChoises { get; set; }
        public DbSet<WorkerHIT> WorkerHITs { get; set; }



        public GeoTurkDbContext() : base("GeoTurkConnectionString")
        {
        }

        public static GeoTurkDbContext Create()
        {
            return new GeoTurkDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<Role>().ToTable("Roles");

            modelBuilder.Entity<User>()
                .Property(u => u.Balance).IsRequired().HasPrecision(10, 2);

            modelBuilder.Entity<HIT>()
                .Property(h => h.HITID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<HIT>()
                .Property(h => h.Title).IsRequired().HasMaxLength(300);
            modelBuilder.Entity<HIT>()
                .Property(h => h.Description).IsOptional().IsMaxLength();
            modelBuilder.Entity<HIT>()
                .Property(h => h.DurationInMinutes).IsRequired().HasPrecision(6, 2);
            modelBuilder.Entity<HIT>()
                .Property(h => h.Instuction).IsRequired().IsMaxLength();
            modelBuilder.Entity<HIT>()
                .Property(h => h.RelatedFilePath).IsOptional().IsMaxLength();
            modelBuilder.Entity<HIT>()
                .Property(h => h.ExpireDate).IsRequired().HasPrecision(3);
            modelBuilder.Entity<HIT>()
                .Property(h => h.PublishDate).IsOptional();
            modelBuilder.Entity<HIT>()
                .Property(h => h.AnswerType).IsRequired();
            modelBuilder.Entity<HIT>()
                .Property(h => h.ChoiseType).IsOptional();
            modelBuilder.Entity<HIT>()
                .HasMany(h => h.TaskChoises).WithRequired(t => t.HIT).HasForeignKey(t => t.HITID).WillCascadeOnDelete(true);
            modelBuilder.Entity<HIT>()
                .Property(h => h.Tags).IsOptional().IsMaxLength();
            modelBuilder.Entity<HIT>()
                .HasRequired(h => h.Creator).WithMany(u => u.OwnHITs).HasForeignKey(h => h.CreatorID).WillCascadeOnDelete(false);
            modelBuilder.Entity<HIT>()
                .Property(h => h.Cost).IsRequired().HasPrecision(10, 2);
            modelBuilder.Entity<HIT>()
                .Property(h => h.WorkersCount).IsRequired();

            modelBuilder.Entity<TaskChoise>()
                .Property(t => t.TaskChoiseID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<TaskChoise>()
            .HasKey(t => t.TaskChoiseID);

            modelBuilder.Entity<Tag>()
              .Property(t => t.TagID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Tag>()
             .Property(t => t.Title).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<WorkerHIT>()
                .HasKey(wh => new { wh.WorkerID, wh.HITID });
            modelBuilder.Entity<WorkerHIT>()
                .HasRequired(wh => wh.Worker).WithMany(w => w.WorkerHITs).HasForeignKey(wh => wh.WorkerID).WillCascadeOnDelete(false);
            modelBuilder.Entity<WorkerHIT>()
                .HasRequired(wh => wh.HIT).WithMany(h => h.WorkerHITs).HasForeignKey(wh => wh.HITID).WillCascadeOnDelete(false);
            modelBuilder.Entity<WorkerHIT>()
                .Property(wh => wh.AssignDate).IsRequired();
            modelBuilder.Entity<WorkerHIT>()
                .HasMany(a => a.HITAnswers).WithRequired(x => x.WorkerHIT).HasForeignKey(a => new { a.WorkerID, a.HITID });

            modelBuilder.Entity<HITAnswer>()
                .HasKey(a => new { a.WorkerID, a.HITID, a.TaskChoiseID });
            modelBuilder.Entity<HITAnswer>()
                .HasRequired(x => x.TaskChoise).WithMany().HasForeignKey(x => x.TaskChoiseID).WillCascadeOnDelete(true);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = string.Format("{0} - ", ex.Message);
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new DbEntityValidationException(errorMessage, ex.EntityValidationErrors);
            }
        }
    }
}