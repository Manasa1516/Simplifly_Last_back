using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public class RegisterCustomerUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "customer";

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Gender { get; set; }
    }
}
