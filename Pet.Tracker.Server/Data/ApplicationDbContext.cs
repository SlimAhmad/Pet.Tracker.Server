using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pet.Tracker.Server
{
    /// <summary>
    /// The database representational model for our application
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Public Properties

        /// <summary>
        /// The settings for the application
        /// </summary>
        // public DbSet<PetsDataModel> PetsDetails { get; set; }

        public DbSet<Pets> Pets { get; set; }
        /// <summary>
        /// 2015-2016 session table
        /// </summary>
        public DbSet<VetCentersDataModel> VetCenters { get; set; }
        /// <summary>
        /// 2016-2017 session table
        /// </summary>
        public DbSet<TransferDataModel> PetsTransfer { get; set; }
        /// <summary>
        /// 2017-2018 session table
        /// </summary>
        public DbSet<PetFinderDataModel> Lost_Found { get; set; }


        public DbSet<ReceivedDataModel> PetsReceived { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationDbContext()
        {
                
        }

        /// <summary>
        /// Default constructor, expecting database options passed in
        /// </summary>
        /// <param name="options">The database context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

            
           
            
        }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Changed Table names
            //AspNetUsers -> User
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("User");
            //AspNetRoles -> Role
            modelBuilder.Entity<IdentityRole>()
                .ToTable("Roles");
            //AspNetUserRoles -> UserRole
            modelBuilder.Entity<IdentityUserRole<string>>()
                .ToTable("UserRoles");
            //AspNetUserClaims -> UserClaim
            modelBuilder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims")
                ;
            //AspNetUserLogins -> UserLogin
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .ToTable("UserLogins");

            //IdentityRoleClaim -> RoleClaim
            modelBuilder.Entity<IdentityRoleClaim<string>>()
                .ToTable("RoleClaims");

            //IdentityUserToken -> UserTokens
            modelBuilder.Entity<IdentityUserToken<string>>()
                .ToTable("UserTokens");

            //PetsDataModel -> Pets
            modelBuilder.Entity<Pets>()
                .ToTable("Pets");

            //ReceivedDataModel -> PetsReceived
            modelBuilder.Entity<ReceivedDataModel>()
                .ToTable("PetsReceived");

            //TransferDataModel -> PetsTransfer
            modelBuilder.Entity<TransferDataModel>()
                .ToTable("PetsTransfer");

            //PetFinderDataModel -> Lost_Found
            modelBuilder.Entity<PetFinderDataModel>()
                .ToTable("Lost_Found");

            //VetCentersDataModel -> VetCenters
            modelBuilder.Entity<VetCentersDataModel>()
                .ToTable("VetCenters");

            #endregion



        }
    }
}
