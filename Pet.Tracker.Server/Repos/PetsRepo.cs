using Microsoft.EntityFrameworkCore;
using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    public class PetsRepo : RepoBase<Pets>, IPetsRepo
    {


        /// <summary>
        /// Default constructor
        /// </summary>
        public PetsRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }



        #region Public methods

        public async Task<IEnumerable<Pets>> SearchAsync(string searchString)
        {
            var result = await Table.Where(x => x.Name.Contains(searchString) || x.Breed.Contains(searchString))
                .Select(p => new Pets {  Breed = p.Breed, Name = p.Name, Id = p.Id }).ToListAsync();
            return result;
        }

        public async Task<Pets> GetSinglePetWithDetails(string clientId)
        {
            var result = await Table.FirstOrDefaultAsync(x => x.ClientId == clientId);
            return result;
        }

        public async Task<IEnumerable<Pets>> GetAllPets() =>  await Table.ToListAsync();

      
        

    

        #endregion









    }
}
