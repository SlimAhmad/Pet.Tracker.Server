using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public interface IReceive : IRepo<ReceivedDataModel>
    {
        Task<ReceivedDataModel> GetSinglePetReceived(int id);
        Task<IEnumerable<ReceivedDataModel>> GetAllPetsReceived(int id);
        object GetAllPetsReceivedAsync();
    }
}
