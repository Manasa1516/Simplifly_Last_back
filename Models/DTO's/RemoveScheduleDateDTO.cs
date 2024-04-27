using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class RemoveScheduleDateDTO
    {
        public DateTime DateOfSchedule { get; set; }
        public int AirportId { get; set; }
    }
}
