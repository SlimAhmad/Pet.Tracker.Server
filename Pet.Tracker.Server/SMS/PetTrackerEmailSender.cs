using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dna.FrameworkDI;
namespace Pet.Tracker.Server
{
    public static class PetTrackerEmailSender
    {
        /// <summary>
        /// Sends a verification email to the specified user
        /// </summary>
        /// <param name="displayName">The users display name (typically first name)</param>
        /// <param name="email">The users email to be verified</param>
        /// <param name="verificationUrl">The URL the user needs to click to verify their email</param>
        /// <returns></returns>
        public static async Task<SendSMSResponse> SendSMSUserTokenAsync(string firstName, string lastName, string toNumber, string token)
        {
            return await DI.SMSTemplateSender.SendGeneralSMSAsync(new SendSMSDetails
            {
               FromNumber = Configuration["Twillio:Phone"],
               ToNumber = toNumber
               
            },
                firstName,
                lastName,
                token
         
            );
        }
    }
}
