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
    public interface IUserService
    {
        /// <summary>
        /// Check does user make valid authentication
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> CheckLogin(string username, string password);

        /// <summary>
        /// Get User.Role by User.Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<string> GetUserRole(string username);

        /// <summary>
        /// Get User by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<User> GetUser(string username);

        /// <summary>
        /// Get User by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetUser(int id);

        /// <summary>
        /// Create new "User"
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateUser(User user);

        /// <summary>
        /// Edit "User"
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task EditUser(User user);

        /// <summary>
        /// Delete user with specific Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        Task DeleteUser(int id);

        /// <summary>
        /// Get list of Users by specific options and roleId
        /// </summary>
        /// <param name="options"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<User>> GetUsers(DatatableOptions options, int roleId);

        /// <summary>
        /// Count all users by roleId and "search" options
        /// </summary>
        /// <param name="options"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<int> CountUsers(DatatableOptions options, int roleId);

        /// <summary>
        /// Update Datatable options, with new data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        DatatableOptions UpdateDatatableOptions(DatatableOptions options);
    }

    public class UserService : RepositoryBase<User>, IRepository<User>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckLogin(string username, string password)
        {
            return await Any(u => u.Username.Equals(username.ToLower()) && u.Password.Equals(password.SHA512Hash()) && !(u.Deleted??false));
        }

        public async Task<string> GetUserRole(string username)
        {
            return await DbContext.Users.AsNoTracking().Where(x => x.Username.Equals(username.ToLower())).Select(x => x.Role.Name).SingleOrDefaultAsync();
        }

        public async Task<User> GetUser(string username)
        {
            return await DbContext.Users.AsNoTracking().Where(x => x.Username.Equals(username.ToLower())).SingleOrDefaultAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await DbContext.Users.AsNoTracking().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task CreateUser(User user)
        {
            //Attach user
            DbContext.Users.Attach(user);

            // Decrypt password
            user.Password = user.Password.SHA512Hash();

            //Create date is now date
            user.CreateDate = DateTime.Now;

            //Set user "deleted" is false
            user.Deleted = false;

            //Set "user" state to "Added"
            DbContext.Entry(user).State = EntityState.Added;

            // Add to database
            await Add(user);

            // Save changes
            await _unitOfWork.Commit();

            // Stop tracking this "User" 
            DbContext.Entry(user).State = EntityState.Detached;
        }

        public async Task EditUser(User user)
        {
            //Attach user
            DbContext.Users.Attach(user);

            //Set "user" state to "Added"
            DbContext.Entry(user).State = EntityState.Modified;

            //Check if user want to change password
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                //Dont change state
                DbContext.Entry(user).Property(u => u.Password).IsModified = false;
            }
            else
            {
                // Decrypt password
                user.Password = user.Password.SHA512Hash();
            }

            //Dont change state
            DbContext.Entry(user).Property(u => u.Deleted).IsModified = false;
            DbContext.Entry(user).Property(u => u.RoleId).IsModified = false;
            DbContext.Entry(user).Property(u => u.CreateDate).IsModified = false;

            // Save changes
            await _unitOfWork.Commit();

            // Stop tracking this "User" 
            DbContext.Entry(user).State = EntityState.Detached;
        }

        public async Task DeleteUser(int id)
        {
            //Catch user from DB
            User user = await DbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            //Set state to "edited"
            DbContext.Entry(user).State = EntityState.Modified;

            //Delete user
            user.Deleted = true;

            // Save changes
            await _unitOfWork.Commit();

            // Stop tracking this "User" 
            DbContext.Entry(user).State = EntityState.Detached;
        }

        public async Task<List<User>> GetUsers(DatatableOptions options, int roleId)
        {
            return await DbContextThreadSafe.Users.AsNoTracking()
                                                  .Where(u => (u.Name.Contains(options.Search)
                                                            || u.Surname.Contains(options.Search)
                                                            || u.Username.Contains(options.Search))
                                                            && u.RoleId == roleId
                                                            && !(u.Deleted??false))
                                                   .Include(u => u.Role)
                                                   .OrderByPage(options.SortBy, options.SortByDirection, options.Page, options.PerPage)
                                                   .ToListAsync();
        }

        public async Task<int> CountUsers(DatatableOptions options, int roleId)
        {
            return await DbContextThreadSafe.Users.AsNoTracking()
                                                  .CountAsync(u => (u.Name.Contains(options.Search)
                                                                 || u.Surname.Contains(options.Search)
                                                                 || u.Username.Contains(options.Search))
                                                                 && u.RoleId == roleId
                                                                 && !(u.Deleted ?? false));
        }

        public DatatableOptions UpdateDatatableOptions(DatatableOptions options)
        {
            // Get number of total pages
            options.TotalPages = (int)(Math.Ceiling(((double)options.TotalRecords / (double)options.PerPage)));

            //Pagination range options
            if (options.TotalPages > 5)
            {
                //First page that shows in pagination
                options.FirstShowPage = options.Page > 2 ? ((options.Page - 2) > (options.TotalPages - 4) ? (options.TotalPages - 4) : (options.Page - 2)) : 1;
                //Last page that shows in pagination
                options.LastShowPage = options.Page > 2 ? ((options.Page + 2) > options.TotalPages ? options.TotalPages : (options.Page + 2)) : 5;
            }
            else
            {
                //First page that shows in pagination
                options.FirstShowPage = 1;
                //Last page that shows in pagination
                options.LastShowPage = options.TotalPages;
            }

            return options;
        }
    }
}
