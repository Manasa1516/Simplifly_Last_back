using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class FlightSeatsDTO
    {
        public string FlightNumber { get; set; }
        public int TotalSeats { get; set;}
    }
}
