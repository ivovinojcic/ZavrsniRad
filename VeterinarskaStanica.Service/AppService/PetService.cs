using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VeterinarskaStanica.Model.Core;
using VeterinarskaStanica.Model.DatabaseConnector;
using VeterinarskaStanica.Service.Extension;
using VeterinarskaStanica.Model.Model.Datatable;

namespace VeterinarskaStanica.Service.AppService
{
    public interface IPetService
    {
        /// <summary>
        /// Get Pet by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Pet> GetPet(int id);

        /// <summary>
        /// Get list of all "PetTypes"
        /// </summary>
        /// <returns></returns>
        Task<List<PetType>> GetPetTypes();

        /// <summary>
        /// Create new "Pet"
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
        Task CreatePet(Pet pet);

        /// <summary>
        /// Edit "Pet"
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
        Task EditPet(Pet pet);

        /// <summary>
        /// Delete pet with specific Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        Task DeletePet(int id);

        /// <summary>
        /// Get list of Pets by specific options and userId
        /// </summary>
        /// <param name="options"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Pet>> GetPets(DatatableOptions options, int userId);

        /// <summary>
        /// Count all pets by userId and "search" options
        /// </summary>
        /// <param name="options"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> CountPets(DatatableOptions options, int userId);
    }

    public class PetService : RepositoryBase<Pet>, IRepository<Pet>, IPetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PetService(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Pet> GetPet(int id)
        {
            return await DbContext.Pets.AsNoTracking().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<List<PetType>> GetPetTypes()
        {
            return await DbContext.PetTypes.AsNoTracking().ToListAsync();
        }

        public async Task CreatePet(Pet pet)
        {
            //Attach pet
            DbContext.Pets.Attach(pet);

            //Set "pet" state to "Added"
            DbContext.Entry(pet).State = EntityState.Added;

            // Add to database
            await Add(pet);

            // Save changes
            await _unitOfWork.Commit();

            // Stop tracking this "Pet" 
            DbContext.Entry(pet).State = EntityState.Detached;
        }

        public async Task EditPet(Pet pet)
        {
            //Attach pet
            DbContext.Pets.Attach(pet);

            //Set "pet" state to "Modified"
            DbContext.Entry(pet).State = EntityState.Modified;

            //Dont change state
            DbContext.Entry(pet).Property(u => u.UserId).IsModified = false;

            // Save changes
            await _unitOfWork.Commit();

            // Stop tracking this "pet" 
            DbContext.Entry(pet).State = EntityState.Detached;
        }

        public async Task DeletePet(int id)
        {
            //Catch pet from DB
            Pet pet = await DbContext.Pets.Where(x => x.Id == id).FirstOrDefaultAsync();

            //Remove all Visit Records for this pet
            DbContext.VisitRecords.RemoveRange(await DbContext.VisitRecords.Where(x => x.PetId == pet.Id).ToListAsync());

            // Save changes
            await _unitOfWork.Commit();

            //Set state to "Modified"
            DbContext.Entry(pet).State = EntityState.Modified;

            DbContext.Remove(pet);

            // Save changes
            await _unitOfWork.Commit();

            // Stop tracking this "Pet" 
            DbContext.Entry(pet).State = EntityState.Detached;
        }

        public async Task<List<Pet>> GetPets(DatatableOptions options, int userId)
        {
            return await DbContextThreadSafe.Pets.AsNoTracking()
                                                  .Where(u => u.Name.Contains(options.Search)
                                                           && u.UserId == userId)
                                                  .Include(x => x.PetType)
                                                  .OrderByPage(options.SortBy, options.SortByDirection, options.Page, options.PerPage)
                                                  .ToListAsync();
        }

        public async Task<int> CountPets(DatatableOptions options, int userId)
        {
            return await DbContextThreadSafe.Pets.AsNoTracking()
                                                  .CountAsync(u => u.Name.Contains(options.Search)
                                                                && u.UserId == userId);
        }
    }
}
