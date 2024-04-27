using Simplifly.Models.DTOs;

namespace Simplifly.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(LoginUserDTO user);
    }
}
