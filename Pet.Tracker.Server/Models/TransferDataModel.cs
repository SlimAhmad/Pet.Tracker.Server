using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pet.Tracker.Server
{
    public class TransferDataModel : EntityBase
    {

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string TranferPhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string TransfererToken { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string TransferEmail { get; set; }

        [Required]
        public bool Transfer { get; set; }


        public string ReceiverToken { get; set; }

        public bool ConfirmToken { get; set; }

        #endregion

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
        /// 1:1
        public Pets Pet { get; set; }

     

        #endregion
    }
}
