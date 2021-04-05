using Microsoft.EntityFrameworkCore;
using Pet.Tracker.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public class TransferRepo : RepoBase<TransferDataModel>, ITransfer
    {


        /// <summary>
        /// Default constructor
        /// </summary>
        public TransferRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        #region Public methods





        public async Task<IEnumerable<TransferDataModel>> GetPetTransfers() => await Table.ToListAsync();

        public async Task<IEnumerable<TransferPetApiModel>> GetAllPetsTransferAsync()
        {
            var result = await Table.Include(p => p.Pet).ThenInclude(x => x.Client)
                .Select(m => new TransferPetApiModel
                {
                    PetId = m.Pet.Id,
                    Height = m.Pet.Height,
                    Description = m.Pet.Description,
                    Gender = m.Pet.Gender,
                    Name = m.Pet.Name,
                    Neutered = m.Pet.Neutered,
                    Age = m.Pet.Age,
                    TransferEmail = m.TransferEmail,
                    TransfererToken = m.TransfererToken,
                    Username = m.Username,
                    TransferPhone = m.TranferPhone,
                    Weight = m.Pet.Weight,
                    Id = m.Id,
                    Email = m.Client.Email,
                    PetOwnerName = m.Client.UserName

                }).ToListAsync();

            return result;
        }

        public async Task<TransferPetApiModel> GetSinglePetWithTransfer(int id)
        {
            var result = await Table.Where(x => x.PetId == id)
               .Include(p => p.Pet).Select(m => new TransferPetApiModel
               {
                   PetId = m.Pet.Id,
                   Height = m.Pet.Height,
                   Description = m.Pet.Description,
                   Gender = m.Pet.Gender,
                   Name = m.Pet.Name,
                   Breed = m.Pet.Breed,
                   Neutered = m.Pet.Neutered,
                   Age = m.Pet.Age,
                   TransferEmail = m.TransferEmail,
                   TransfererToken = m.TransfererToken,
                   Username = m.Username,
                   TransferPhone = m.TranferPhone,
                   Weight = m.Pet.Weight,
                   Id = m.Id
               }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<TransferPetApiModel> GetSinglePet(int id)
        {
            var result = await Table.Where(x => x.PetId == id)
              .Include(p => p.Pet).Select(m => new TransferPetApiModel
              {
                  PetId = m.Pet.Id,
                  Height = m.Pet.Height,
                  Description = m.Pet.Description,
                  Gender = m.Pet.Gender,
                  Name = m.Pet.Name,
                  Breed = m.Pet.Breed,
                  Neutered = m.Pet.Neutered,
                  Age = m.Pet.Age,
                  Weight = m.Pet.Weight,
                  Id = m.Id
              }).FirstOrDefaultAsync();

            return result;
        }






        #endregion
    }
}
