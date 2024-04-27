using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class FlightScheduleDTO
    {
        public string FlightNumber { get; set; }
        public string SourceAirport { get; set; }
        public string DestinationAirport { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
    }
}

