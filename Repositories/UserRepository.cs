using Simplifly.Context;
using Simplifly.Interfaces;
using Simplifly.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Simplifly.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserRepository : IRepository<string, User>
    {
        private readonly RequestTrackerContext _context;
        private readonly ILogger<UserRepository> _logger;

        /// <summary>
        /// Constructor to initialize object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public UserRepository(RequestTrackerContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Method to add user
        /// </summary>
        /// <param name="item">Object of user</param>
        /// <returns>User object</returns>
        public async Task<User> Add(User item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation($"User added with username {item.Username}");
            return item;
        }

        /// <summary>
        /// Method to delete User
        /// </summary>
        /// <param name="key">username in string</param>
        /// <returns>Object of user</returns>
        /// <exception cref="NoSuchUserException">when user with username not found</exception>
        public async Task<User> Delete(string key)
        {
            var user = await GetAsync(key);
            if (user != null)
            {
                _context.Remove(user);
                _context.SaveChanges();
                _logger.LogInformation($"User deleted with username {key}");
                return user;
            }
            throw new NoSuchUserException();
        }

        /// <summary>
        /// Method to get user by username
        /// </summary>
        /// <param name="key">username in string</param>
        /// <returns>Object of user</returns>
        public async Task<User> GetAsync(string key)
        {
            var users = _context.Users.ToList(); // Fetch all users from the database
            var user = users.SingleOrDefault(u => string.Equals(u.Username, key, StringComparison.Ordinal));
            return user;
        }

        /// <summary>
        /// Method to get all user
        /// </summary>
        /// <returns>list of users</returns>
        public async Task<List<User>> GetAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        /// <summary>
        /// Method to update user
        /// </summary>
        /// <param name="item">Object of user</param>
        /// <returns>User Object</returns>
        /// <exception cref="NoSuchUserException">When user with details not found</exception>
        public async Task<User> Update(User item)
        {
            var user = await GetAsync(item.Username);
            if (user != null)
            {
                _context.Entry<User>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"User updated with username {item.Username}");
                return item;
            }
            throw new NoSuchUserException();
        }
    }
}