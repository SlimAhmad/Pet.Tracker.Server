using Dna;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{

    /// <summary>
    /// Extension methods for the <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
        /// <summary>
        /// Injects the repositories needed for pet tracker application
        /// </summary>
        /// <param name="construction"></param>
        /// <returns></returns>
        public static IServiceCollection AddPetTrackerClientServices(this IServiceCollection services)
        {
            services.AddScoped<IPetsRepo, PetsRepo>();
            services.AddScoped<IFinder, FinderRepo>();
            services.AddScoped<IReceive, ReceiveRepo>();
            services.AddScoped<ITransfer, TransferRepo>();
            services.AddScoped<IVetCentersRepo, CentersRepo>();

            // Return the construction for chaining
            return services;
        }

    }
}