using Microsoft.Extensions.DependencyInjection;
using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public static class SMSTemplateSenderExtensions
    {
        /// <summary>
        /// Injects the <see cref="SMSTemplateSender"/> into the services to handle the 
        /// <see cref="ISMSTemplateSender"/> service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSMSTemplateSender(this IServiceCollection services)
        {
            // Inject the SendGridEmailSender
            services.AddTransient<ISMSTemplateSender, SMSTemplateSender>();

            // Return collection for chaining
            return services;
        }
    }
}
