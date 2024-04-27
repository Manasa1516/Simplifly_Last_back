using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;

namespace Simplifly.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<int, Admin> _repository;
        private readonly IRepository<string,User> _userRepository;
        private readonly ILogger<AdminService> _logger;

        /// <summary>
        /// Constructor for the AdminService class.
        /// </summary>
        /// <param name="repository">repository interface</param>
        /// <param name="logger">Logger for logging</param>
        public AdminService(IRepository<int, Admin> repository, ILogger<AdminService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public AdminService(IRepository<int, Admin> repository, ILogger<AdminService> logger, IRepository<string, User> userRepository)
        {
            _repository = repository;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<User> DeleteUser(string username)
        {
            var users = await _userRepository.GetAsync();
            var user= users.FirstOrDefault(e=>e.Username==username);
            if(user!=null)
            {
                user= await _userRepository.Delete(username);
                return user;
            }
            throw new NoSuchUserException();
        }

        /// <summary>
        /// Method to get Admin by username
        /// </summary>
        /// <param name="username">username in string</param>
        /// <returns>Admin object</returns>
        /// <exception cref="NoSuchAdminException">Throw when no admin found with given username </exception>
        public async Task<Admin> GetAdminByUsername(string username)
        {
            var admins = await _repository.GetAsync();
            var admin= admins.FirstOrDefault(e=>e.Username==username);
            if (admin!=null) return admin;
            throw new NoSuchAdminException();
        }

        /// <summary>
        /// Method to update admin details
        /// </summary>
        /// <param name="admin">Object of UpdateAdminDTO</param>
        /// <returns>Admin object</returns>
        /// <exception cref="NoSuchAdminException">Throw when no admin found with given username</exception>
        public async Task<Admin> UpdateAdmin(UpdateAdminDTO admin)
        {
            var admins = await _repository.GetAsync(admin.AdminId);
            if (admins != null)
            {
                admins.Name = admin.Name;
                admins.Email = admin.Email;
                admins.Address = admin.Address;
                admins.ContactNumber = admin.ContactNumber;
                admins.Position = admin.Position;
                admins = await _repository.Update(admins);
                return admins;
            }
            throw new NoSuchAdminException();
        }
    }
}
