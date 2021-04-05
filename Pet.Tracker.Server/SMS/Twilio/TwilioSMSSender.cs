using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using static Dna.FrameworkDI;

namespace Pet.Tracker.Server
{
    public class TwilioSMSSender : ISMSSender
    {
        public async Task<SendSMSResponse> SendSMSAsync(SendSMSDetails details)
        {
            // Find your Account Sid and Token at twilio.com/console

            var authToken = Configuration["Twillio:Auth-token"];

            var accountSid = Configuration["Twillio:Account-sid"];
            // From
            var from = details.FromNumber;

            // To
            var to = details.ToNumber;
            
            // Body
            var content = details.Content;

            TwilioClient.Init(authToken, accountSid);

            try
            {


                var message = await MessageResource.CreateAsync(
                       to: new Twilio.Types.PhoneNumber(to),
                       from: new Twilio.Types.PhoneNumber(from),
                       body: content

                      );


                // If we succeeded...
                if (message.ErrorCode == null)
                    // Return successful response
                    return new SendSMSResponse();

                // Add any errors to the response
                var errorResponse = new SendSMSResponse
                {
                    Errors = new List<string>(new[] { message.ErrorMessage })
                };

                // Make sure we have at least one error
                if (errorResponse.Errors == null || errorResponse.Errors.Count == 0)
                    // Add an unknown error
                    // TODO: Localization
                    errorResponse.Errors = new List<string>(new[] { "Unknown error from sms sending service. Please contact Pet tracker support." });

                // Return the response
                return errorResponse;

            }
            catch(ApiException ex)
            {
                // Break if we are debugging
                if (Debugger.IsAttached)
                {
                    var error = ex;
                    Debugger.Break();
                }


                // If something unexpected happened, return message
                return new SendSMSResponse
                {
                    Errors = new List<string>(new[] { "Unknown error occurred" + ex.Message })
                };
            }
         
        }
        
    }
}