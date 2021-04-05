using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pet.Tracker.Server
{
    public class Pets : EntityBase
    {

        /// <summary>
        /// Pets name
        /// </summary>
        [Required, StringLength(50)]
        public string Name { get; set; }


        /// <summary>
        /// Pets gender
        /// </summary>
        [Required, StringLength(20)]
        public string Gender { get; set; }
        /// <summary>
        /// Pets Weight
        /// </summary>
        [Required]
        public double Weight { get; set; }
        /// <summary>
        /// Pets Height
        /// </summary>
        public decimal Height { get; set; }
        /// <summary>
        /// Pets age
        /// </summary>
        [Required]
        public int Age { get; set; }
        /// <summary>
        /// Pets Neutered or not
        /// </summary>
        [Required]
        public bool Neutered { get; set; }
        /// <summary>
        /// Pets Description
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// Pets Breed
        /// </summary>
        [Required]
        public string Breed { get; set; }

        /// <summary>
        /// The current date and time
        /// </summary>
        public DateTime CurrentDate => DateTime.Now;

        ///Navigation Properties
        ///



        /// <summary>
        /// Pet owners id 
        /// </summary>
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }



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
        public PetFinderDataModel Pet { get; set; }



        /// <summary>
        /// Pets transfer id 
        /// </summary>
        /// 1:1
        public TransferDataModel Transfer { get; set; }

        /// <summary>
        /// Pets Received id 
        /// </summary>
        /// 1:1
        public ReceivedDataModel Received { get; set; }

        #endregion

    }
}
