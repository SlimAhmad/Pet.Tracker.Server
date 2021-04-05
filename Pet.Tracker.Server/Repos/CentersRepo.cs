using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public class CentersRepo : RepoBase<VetCentersDataModel>, IVetCentersRepo
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CentersRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public async Task<VetCentersDataModel> GetSingleWithDetails(int id) => await Table.FirstOrDefaultAsync(x => x.Id == id);



        public async Task<IEnumerable<VetCentersDataModel>> SearchAsync(string title, string area)
        {
            var result = await Table.Where(x => x.Title.Contains(title) || x.Area.Contains(area))
               .Select(v => new VetCentersDataModel { Title = v.Title, Area = v.Area, Id = v.Id }).ToListAsync();
            return result;
        }

    }
}
