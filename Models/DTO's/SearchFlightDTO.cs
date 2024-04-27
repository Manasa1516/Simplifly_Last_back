using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class SearchFlightDTO
    {
        public DateTime DateOfJourney { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Adult {  get; set; }
        public int Child {  get; set; }
        public string SeatClass { get; set;}
    }
}
