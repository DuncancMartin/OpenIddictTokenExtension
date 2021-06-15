using IdentityApiOpenIddict_TokenExtension.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddictTokenExtension.DataAccessLayer.EntityTypeConfiguration;
using OpenIddictTokenExtension.DataAccessLayer.Interfaces;

namespace OpenIddictTokenExtension.DataAccessLayer
{
    public class IdentityDbContext : IdentityDbContext<IdentityMSUser, IdentityMSRole, int>, IDataContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options) { }

        public virtual DbSet<IdentityMSUser> IdentityUsers { get; set; }
        public virtual DbSet<IdentityMSRole> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.UseOpenIddict();

            builder.ApplyConfiguration(new IdentityMSUserConfiguration());

            builder.Entity<IdentityMSRole>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("Tokens");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        }
    }
}
