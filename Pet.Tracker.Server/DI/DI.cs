using Dna;
using Microsoft.Extensions.DependencyInjection;
using Pet.Tracker.Core;

namespace Pet.Tracker.Server
{
    /// <summary>
    /// A shorthand access class to get DI services with nice clean short code
    /// </summary>
    public static class DI
    {
        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        public static ApplicationDbContext ApplicationDbContext => Framework.Provider.GetService<ApplicationDbContext>();

        /// <summary>
        /// The transient instance of the <see cref="ISMSSender"/>
        /// </summary>
        public static ISMSSender SMSSender => Framework.Provider.GetService<ISMSSender>();

        /// <summary>
        /// The transient instance of the <see cref="ISMSTemplateSender"/>
        /// </summary>
        public static ISMSTemplateSender SMSTemplateSender => Framework.Provider.GetService<ISMSTemplateSender>();
    }
}
