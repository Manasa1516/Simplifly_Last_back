using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class GetRouteIdDTO
    {
        public int  SourceAirportId { get; set; }
        public int DestinationAirportId { get; set;}
    }
}
