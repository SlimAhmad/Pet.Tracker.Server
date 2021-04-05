using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pet.Tracker.Server
{
    public class PetFinderDataModel : EntityBase
    {
        #region Column properties
        /// <summary>
        /// The pets Name
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// The date it got lost
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// The Time it got lost
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Lost or found
        /// </summary>
        public bool LostFound { get; set; }


        /// <summary>
        /// The current date and time
        /// </summary>
        public DateTime CurrentDate  => DateTime.Now;

        #endregion

        ////////////////////////////////////////////
        ///Navigational Properties
        ///////////////////////////////////////////


        #region ForeignKeys

        /// <summary>
        /// Pet owners id 
        /// </summary>
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }

        /// <summary>
        /// Pets id 
        /// </summary>
        [ForeignKey(nameof(Pet))]
        public int PetId { get; set; }

        #endregion

        #region navigational properties

        /// <summary>
        /// Pet owners id 
        /// </summary>
        /// 1:N
        public ApplicationUser Client { get; set; }


        /// <summary>
        /// Pets id 
        /// </summary>
        /// 
        public Pets Pet { get; set; }

        #endregion

    }
}
