using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public interface IPetsRepo : IRepo<Pets>
    {
        Task<IEnumerable<Pets>> SearchAsync(string searchString);
        Task<Pets> GetSinglePetWithDetails(string clientId);
        Task<IEnumerable<Pets>> GetAllPets();

    }
}
