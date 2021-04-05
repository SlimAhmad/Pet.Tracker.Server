using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public interface IFinder : IRepo<PetFinderDataModel>
    {
        Task<PetFinderDataModel> GetSinglePetWithDetails(int id);
        Task<IEnumerable<PetFinderDataModel>> GetAllPets(int id);

    }
}
