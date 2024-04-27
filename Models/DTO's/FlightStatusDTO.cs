using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class FlightStatusDTO
    {
        public string FlightNumber { get; set; }
        public int Status { get; set; }
    }
}
