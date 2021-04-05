using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public class FinderRepo : RepoBase<PetFinderDataModel>, IFinder
    {
        


        public async Task<PetFinderDataModel> GetSinglePetWithDetails(int id)
        {
            var result = await Table.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<IEnumerable<PetFinderDataModel>> GetAllPets(int id) => await Table.Where(x => x.Id == id).ToListAsync();

    
    }
}
