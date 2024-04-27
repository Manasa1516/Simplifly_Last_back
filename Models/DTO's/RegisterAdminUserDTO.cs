using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Simplifly.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public class RegisterAdminUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "admin";
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
    }
}