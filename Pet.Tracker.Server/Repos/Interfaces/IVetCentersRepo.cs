using Pet.Tracker.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public interface IVetCentersRepo : IRepo<VetCentersDataModel>
    {
     
        Task<VetCentersDataModel> GetSingleWithDetails(int id);
        Task<IEnumerable<VetCentersDataModel>> SearchAsync(string title, string area);
    }
}
