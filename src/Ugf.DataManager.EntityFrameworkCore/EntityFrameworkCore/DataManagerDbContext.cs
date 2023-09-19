using Microsoft.EntityFrameworkCore;
using Ugf.DataManager.ClassManagement;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Ugf.DataManager.EntityFrameworkCore
{
    [ReplaceDbContext(typeof(IIdentityDbContext))]
    [ReplaceDbContext(typeof(ITenantManagementDbContext))]
    [ConnectionStringName("Default")]
    public class DataManagerDbContext :
        AbpDbContext<DataManagerDbContext>,
        IIdentityDbContext,
        ITenantManagementDbContext
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here. */

        #region Entities from the modules

        /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
         * and replaced them for this DbContext. This allows you to perform JOIN
         * queries for the entities of these modules over the repositories easily. You
         * typically don't need that for other modules. But, if you need, you can
         * implement the DbContext interface of the needed module and use ReplaceDbContext
         * attribute just like IIdentityDbContext and ITenantManagementDbContext.
         *
         * More info: Replacing a DbContext of a module ensures that the related module
         * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
         */

        //Identity
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityClaimType> ClaimTypes { get; set; }
        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
        public DbSet<IdentityLinkUser> LinkUsers { get; set; }
        public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

        // Tenant Management
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        public DbSet<ClassContainer> ClassContainers { get; set; }
        public DbSet<SpecificObject> SpecificObjects { get; set; }
        public DbSet<ClassProperty> ClassProperties { get; set; }
        public DbSet<DataConfig> DataConfigs { get; set; }

        #endregion

        public DataManagerDbContext(DbContextOptions<DataManagerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();
            builder.ConfigureOpenIddict();
            builder.ConfigureFeatureManagement();
            builder.ConfigureTenantManagement();

            builder.Entity<ClassContainer>(b =>
            {
                b.ConfigureByConvention();

                b.Property(t => t.Name).IsRequired();
                b.HasIndex(u => u.Name);
            });
        
            builder.Entity<SpecificObject>(b =>
            {
                b.ConfigureByConvention();

                b.Property(t => t.Name).IsRequired();
                b.HasIndex(u => u.Name);
            });
        
            builder.Entity<ClassProperty>(b =>
            {
                b.ConfigureByConvention();
                b.Property(cs => cs.ClassId).IsRequired();
                b.HasIndex(u => u.ClassId);
            });
            
            builder.Entity<DataConfig>(b =>
            {
                b.ConfigureByConvention();
                b.Property(cs => cs.Name).IsRequired();
                b.HasIndex(u => u.Name);
            });
            
            builder.Entity<DataConfigItem>(b =>
            {
                b.ConfigureByConvention();
                b.HasKey(u => new { u.ConfigId, u.ObjectId });
                b.HasOne<DataConfig>()
                    .WithMany(p => p.DataConfigItems)
                    .HasForeignKey(u => u.ConfigId);
                b.HasIndex( u => new { u.ConfigId, u.ObjectId });
            });
        }
    }
}