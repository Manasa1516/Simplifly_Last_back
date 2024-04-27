using Simplifly.Models;
using Simplifly.Models.DTO_s;

namespace Simplifly.Interfaces
{
    public interface IAdminService
    {
        public Task<Admin> GetAdminByUsername(string username);
        public Task<Admin> UpdateAdmin(UpdateAdminDTO admin);
        public Task<User> DeleteUser(string username);
    }
}
