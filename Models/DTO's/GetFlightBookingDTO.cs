using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class GetFlightBookingDTO
    {
        public string FlightId { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}