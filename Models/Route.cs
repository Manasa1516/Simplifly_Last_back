using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models
{
    [ExcludeFromCodeCoverage]
    public class Route:IEquatable<Route>
    {
        [Key]
        public int Id { get; set; }        
        public int SourceAirportId { get; set; }

        //This one is just for navigation and will not be created as an attribute in table
        [ForeignKey("SourceAirportId")]
        public Airport? SourceAirport { get; set; }        
        public int DestinationAirportId { get; set;}

        //This one is just for navigation and will not be created as an attribute in table
        [ForeignKey("DestinationAirportId")]
        public Airport? DestinationAirport { get; set; }
        public double Distance { get; set; }
        public int Status { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }


        public Route()
        {
            Id = 0;
        }

        public Route(int id, int sourceAirportId, Airport? sourceAirport, int destinationAirportId, Airport? destinationAirport)
        {
            Id = id;
            SourceAirportId = sourceAirportId;
            SourceAirport = sourceAirport;
            DestinationAirportId = destinationAirportId;
            DestinationAirport = destinationAirport;
        }

        public bool Equals(Route? other)
        {
            var route= other ?? new Route();
            return this.Id.Equals(route.Id);
        }
    }
}
