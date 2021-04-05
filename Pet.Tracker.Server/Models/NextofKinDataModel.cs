using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public class NextofKinDataModel
    {
        /// <summary>
        /// The users next of kin Email
        /// </summary>
        public string NextofKinEmail { get; set; }

        /// <summary>
        /// The users next of kin Name
        /// </summary>
        public string NextofKinName { get; set; }

        /// <summary>
        /// The users next of kin Phone number
        /// </summary>
        public string NextofKinPhone { get; set; }
    }
}
