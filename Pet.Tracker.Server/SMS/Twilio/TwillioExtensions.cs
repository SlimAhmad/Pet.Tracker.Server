using Microsoft.Extensions.DependencyInjection;
using Pet.Tracker.Core;

namespace Pet.Tracker.Server
{
    /// <summary>
    /// Extension methods for any twilio classes
    /// </summary>
    public static class TwillioExtensions
    {
        /// <summary>
        /// Injects the <see cref="TwilioSMSSender"/> into the services to handle the 
        /// <see cref="ISMSSender"/> service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTwilioSMSSender(this IServiceCollection services)
        {
            // Inject the SendGridEmailSender
            services.AddTransient<ISMSSender, TwilioSMSSender>();

            // Return collection for chaining
            return services;
        }
    }
}
