using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Pet.Tracker.Server
{
    /// <summary>
    /// The user data and profile for our application
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        #region Public Properties

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; }

        #endregion


        #region navigational properties

        /// <summary>
        /// Pet owners id 
        /// </summary>
        /// N:1
        public ICollection<Pets>  Pets { get; set; }

        /// <summary>
        /// lost pest owner id 
        /// </summary>
        /// N:1
        public ICollection<PetFinderDataModel> PetFinders { get; set; }

        /// <summary>
        /// Transfer Pets
        /// </summary>
        /// N:1
        public ICollection<TransferDataModel> Transfers { get; set; }

        /// <summary>
        /// Received Pets
        /// </summary>
        /// N:1
        public ICollection<ReceivedDataModel>  Received { get; set; }

        #endregion
    }
}