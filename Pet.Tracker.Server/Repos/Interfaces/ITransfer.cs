using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public interface ITransfer : IRepo<TransferDataModel>
    {
  
        Task<TransferPetApiModel> GetSinglePetWithTransfer(int id);
        Task<TransferPetApiModel> GetSinglePet(int id);
        Task<IEnumerable<TransferPetApiModel>> GetAllPetsTransferAsync();
     

    }
}
