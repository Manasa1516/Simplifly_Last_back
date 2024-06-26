﻿using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public class RegisterFlightOwnerUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "flightOwner";

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Additional Fields for Flight Owner
        public string CompanyName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public string BusinessRegistrationNumber { get; set; } = string.Empty;
    }
}
