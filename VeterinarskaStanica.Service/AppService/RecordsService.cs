using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VeterinarskaStanica.Model.Core;
using VeterinarskaStanica.Model.DatabaseConnector;
using VeterinarskaStanica.Service.Extension;
using VeterinarskaStanica.Model.Model.Datatable;
using VeterinarskaStanica.Model.Model.Records;

namespace VeterinarskaStanica.Service.AppService
{
    public interface IRecordsService
    {
        /// <summary>
        /// Get "Visit Record" by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<VisitRecord> GetVisitRecord(int id);

        /// <summary>
        /// Get List of all RecordStatuses
        /// </summary>
        /// <returns></returns>
        Task<List<RecordStatus>> GetRecordStatuses();

        /// <summary>
        /// Create new "Visit Record"
        /// </summary>
        /// <param name="visitRecord"></param>
        /// <returns></returns>
        Task CreateVisitRecord(VisitRecord visitRecord);

        /// <summary>
        /// Edit "Visit Record"
        /// </summary>
        /// <param name="visitRecord"></param>
        /// <returns></returns>
        Task EditVisitRecord(VisitRecord visitRecord);

        /// <summary>
        /// Get list of VisitRecords by specific options and userId or employeeId
        /// </summary>
        /// <param name="options"></param>
        /// <param name="userId"></param>
        /// <param name="employeeId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        Task<List<RecordsTable>> GetRecords(DatatableOptions options, int userId, int employeeId, int statusId);

        /// <summary>
        ///  Count all VisitRecords by userId or employeeId and "search" options
        /// </summary>
        /// <param name="options"></param>
        /// <param name="userId"></param>
        /// <param name="employeeId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        Task<int> CountRecords(DatatableOptions options, int userId, int employeeId, int statusId);

        /// <summary>
        /// Set "denied" status to record
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        Task MakeRecordDenied(int recordId);
    }

    public class RecordsService : RepositoryBase<VisitRecord>, IRepository<VisitRecord>, IRecordsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecordsService(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VisitRecord> GetVisitRecord(int id)
        {
            return await DbContextThreadSafe.VisitRecords.AsNoTracking().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<List<RecordStatus>> GetRecordStatuses()
        {
            return await DbContextThreadSafe.RecordStatuses.AsNoTracking().ToListAsync();
        }

        public async Task CreateVisitRecord(VisitRecord visitRecord)
        {
            //Attach visitRecord
            DbContext.VisitRecords.Attach(visitRecord);

            //Set "visitRecord" state to "Added"
            DbContext.Entry(visitRecord).State = EntityState.Added;

            // Add to database
            await Add(visitRecord);

            // Save changes
            await _unitOfWork.Commit();

            // Stop tracking this "Pet" 
            DbContext.Entry(visitRecord).State = EntityState.Detached;
        }

        public async Task EditVisitRecord(VisitRecord visitRecord)
        {
            //Attach visitRecord
            DbContext.VisitRecords.Attach(visitRecord);

            //Set "visitRecord" state to "Modified"
            DbContext.Entry(visitRecord).State = EntityState.Modified;

            // Save changes
            await _unitOfWork.Commit();

            // Stop tracking this "visitRecord" 
            DbContext.Entry(visitRecord).State = EntityState.Detached;
        }

        public async Task<List<RecordsTable>> GetRecords(DatatableOptions options, int userId, int employeeId, int statusId)
        {
            return await DbContextThreadSafe.VisitRecords.AsNoTracking()
                                                         .Where(x => (x.Employee.Name.Contains(options.Search)
                                                                   || x.Employee.Surname.Contains(options.Search)
                                                                   || x.Employee.Username.Contains(options.Search)
                                                                   || x.Pet.User.Name.Contains(options.Search)
                                                                   || x.Pet.User.Username.Contains(options.Search)
                                                                   || x.Pet.Name.Contains(options.Search))
                                                                   && (userId == 0 || x.Pet.UserId == userId)
                                                                   && (employeeId == 0 || x.EmployeeId == employeeId)
                                                                   && (statusId == 0 || x.RecordStatusId == statusId))
                                                         .Include(x => x.Pet).ThenInclude(x => x.User)
                                                         .Include(x => x.Pet).ThenInclude(x => x.PetType)
                                                         .Include(x => x.Employee)
                                                         .Include(x => x.RecordStatus)
                                                         .OrderByPage(options.SortBy, options.SortByDirection, options.Page, options.PerPage)
                                                         .Select(x => new RecordsTable(x))
                                                         .ToListAsync();
        }

        public async Task<int> CountRecords(DatatableOptions options, int userId, int employeeId, int statusId)
        {
            return await DbContextThreadSafe.VisitRecords.AsNoTracking()
                                                          .CountAsync(x => (x.Employee.Name.Contains(options.Search)
                                                                           || x.Employee.Surname.Contains(options.Search)
                                                                           || x.Employee.Username.Contains(options.Search)
                                                                           || x.Pet.User.Name.Contains(options.Search)
                                                                           || x.Pet.User.Username.Contains(options.Search)
                                                                           || x.Pet.Name.Contains(options.Search))
                                                                           && (userId == 0 || x.Pet.UserId == userId)
                                                                           && (employeeId == 0 || x.EmployeeId == employeeId)
                                                                           && (statusId == 0 || x.RecordStatusId == statusId));
        }

        public async Task MakeRecordDenied(int recordId)
        {
            VisitRecord visitRecord = await DbContext.VisitRecords.Where(x => x.Id == recordId).FirstOrDefaultAsync();

            visitRecord.RecordStatusId = 4;

            //Set "visitRecord" state to "Modified"
            DbContext.Entry(visitRecord).State = EntityState.Modified;

            // Save changes
            await _unitOfWork.Commit();

            // Stop tracking this "visitRecord" 
            DbContext.Entry(visitRecord).State = EntityState.Detached;
        }
    }
}
