using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class SearchedFlightResultDTO
    {
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public int ScheduleId { get; set; }
        public string SourceAirport { get; set; }
        public string DestinationAirport { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public Double TotalPrice { get; set; }
    }
}