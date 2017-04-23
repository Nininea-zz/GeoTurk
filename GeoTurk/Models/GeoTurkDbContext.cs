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