using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public class ReceiveRepo : RepoBase<ReceivedDataModel>, IReceive
    {
        

        #region Public methods



        public Task<ReceivedDataModel> GetSinglePetReceived(int id) => Table.FirstOrDefaultAsync(x => x.Id == id);


        public async Task<IEnumerable<ReceivedDataModel>> GetAllPetsReceived(int id) => await Table.Where(x => x.Id == id).ToListAsync();

        public object GetAllPetsReceivedAsync()
        {
            throw new System.NotImplementedException();
        }




        #endregion
    }
}
