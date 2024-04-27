using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Simplifly.Models
{
    [ExcludeFromCodeCoverage]
    public class User
    {
        [Key]
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Role { get; set; }
        public byte[] Key { get; set; }

        public Admin? admin { get; set; }
        public FlightOwner? flightOwner { get; set; }

        public Customer? customer { get; set; }

    }
}
