﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VeterinarskaStanica.Model.Core;
using VeterinarskaStanica.Model.DatabaseConnector;
using VeterinarskaStanica.Service.Extension;

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
        /// Get User by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<User> GetUser(string username);
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
            return await Any(u => u.Username.Equals(username.ToLower()) && u.Password.Equals(password.SHA512Hash()));
        }

        public async Task<User> GetUser(string username)
        {
            return await DbContext.Users.Where(x => x.Username.Equals(username)).SingleOrDefaultAsync();
        }
    }
}
