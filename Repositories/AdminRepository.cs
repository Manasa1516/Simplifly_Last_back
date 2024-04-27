using Microsoft.EntityFrameworkCore;
using Simplifly.Context;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Repositories
{
    [ExcludeFromCodeCoverage]
    public class AdminRepository : IRepository<int, Admin>
    {
        readonly RequestTrackerContext _context;
        ILogger<AdminRepository> _logger;
        /// <summary>
        /// Default constructor with RequestTrackerContext
        /// </summary>
        /// <param name="context">Database context</param>
        public AdminRepository(RequestTrackerContext context, ILogger<AdminRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Repository Method to add admin
        /// </summary>
        /// <param name="items">Object of admin</param>
        /// <returns>Admin object</returns>
        public async Task<Admin> Add(Admin items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("Admin added with id " + items.AdminId);
            return items;
        }

        /// <summary>
        /// Repository method to delete admin
        /// </summary>
        /// <param name="ownerId">Admin Id in int</param>
        /// <returns>Admin object</returns>
        /// <exception cref="NoSuchAdminException">When adminId not found</exception>
        public Task<Admin> Delete(int ownerId)
        {
            var admin = GetAsync(ownerId);
            if (admin != null)
            {
                _context.Remove(admin);
                _context.SaveChanges();
                _logger.LogInformation("Admin deleted with id " + ownerId);
                return admin;
            }
            throw new NoSuchAdminException();
        }

        /// <summary>
        /// Method to get Admin by id
        /// </summary>
        /// <param name="key">Id in int</param>
        /// <returns>Object of admin</returns>
        /// <exception cref="NoSuchAdminException">when admin with given id not found</exception>
        public async Task<Admin> GetAsync(int key)
        {
            var admins = await GetAsync();
            var admin = admins.FirstOrDefault(e => e.AdminId == key);
            if (admin != null)
            {
                return admin;
            }
            throw new NoSuchAdminException();
        }

        /// <summary>
        /// Method to get all admin
        /// </summary>
        /// <returns>List of admins</returns>
        public async Task<List<Admin>> GetAsync()
        {
            var admins = _context.Admins.ToList();
            return admins;
        }

        /// <summary>
        /// Method to update admins
        /// </summary>
        /// <param name="items">Object of admin</param>
        /// <returns>Admin Object</returns>
        /// <exception cref="NoSuchAdminException">When admin object not found</exception>
        public async Task<Admin> Update(Admin items)
        {
            var admin = await GetAsync(items.AdminId);
            if (admin != null)
            {
                _context.Entry<Admin>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Admin updated with id " + items.AdminId);
                return admin;
            }
            throw new NoSuchAdminException();
        }
    }
}
