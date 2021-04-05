using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public class VetCentersDataModel : EntityBase
    {
    
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Area { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Address { get; set; }
      
    }
}
