using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class RemoveRouteDTO
    {
        public int sourceAirportId { get; set; }
        public int destinationAirportId { get;set; }
    }
}
