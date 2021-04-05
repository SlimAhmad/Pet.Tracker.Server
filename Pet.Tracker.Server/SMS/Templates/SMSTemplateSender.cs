using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public class SMSTemplateSender : ISMSTemplateSender
    {
       

        public async Task<SendSMSResponse> SendGeneralSMSAsync(SendSMSDetails details, string firstName, string lastName, string token)
        {
            var templateText = default(string);

            // Read the general template from file
            // TODO: Replace with IoC Flat data provider
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("Pet.Tracker.Server.SMS.Templates.GeneralTemplate.htm"), Encoding.UTF8))
            {
                // Read file contents
                templateText = await reader.ReadToEndAsync();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--token--", token)
                                               .Replace("--firstname--", firstName)
                                               .Replace("--lastname--", lastName);
                                               


            // Set the details content to this template content
            details.Content = templateText;

            // Send email
            return await DI.SMSSender.SendSMSAsync(details);
        }
    }

}
