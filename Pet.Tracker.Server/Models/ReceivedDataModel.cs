using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public class ReceivedDataModel : EntityBase
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ReceiverPhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int ReceiverToken { get; set; }

        #endregion

        #region ForeignKeys

        /// <summary>
        /// Pets id 
        /// </summary>
        [ForeignKey(nameof(Pet))]
        public int PetId { get; set; }

        /// <summary>
        /// Pets id 
        /// </summary>
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }

        #endregion

        #region navigational properties

        /// <summary>
        /// Pet owners id 
        /// </summary>
        /// 1:N
        public ApplicationUser Client { get; set; }

        /// <summary>
        /// Pet owners id 
        /// </summary>
        /// 1:1
        public Pets Pet { get; set; }

        #endregion
    }
}
