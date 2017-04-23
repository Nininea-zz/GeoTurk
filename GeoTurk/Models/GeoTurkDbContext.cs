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

namespace GeoTurk.Models
{
    public class GeoTurkDbContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public DbSet<HIT> HITs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskChoise> TaskChoises { get; set; }

        public GeoTurkDbContext() : base(typeof(GeoTurkDbContext).Name)
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
                .Property(h => h.RelatedFilePath).IsOptional();
            modelBuilder.Entity<HIT>()
                .Property(h => h.ExpireDate).IsRequired().HasPrecision(3);
            modelBuilder.Entity<HIT>()
                .Property(h => h.AnswerType).IsRequired();
            modelBuilder.Entity<HIT>()
                .Property(h => h.ChoiseType).IsOptional();
            modelBuilder.Entity<HIT>()
                .HasMany(h => h.TaskChoises).WithRequired(t => t.HIT).HasForeignKey(t => t.HITID).WillCascadeOnDelete(true);
            modelBuilder.Entity<HIT>()
                .HasMany(h => h.Tags).WithMany(t => t.HITs).Map(map => map.MapLeftKey("HITID").MapRightKey("TagID"));
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