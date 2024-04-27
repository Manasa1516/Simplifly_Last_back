using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models
{
    [ExcludeFromCodeCoverage]
    public class Airport:IEquatable<Airport>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public Airport()
        {
            Id = 0;
        }

        public Airport(int id, string name, string city, string state, string country)
        {
            Id = id;
            Name = name;
            City = city;
            State = state;
            Country = country;
        }

        public bool Equals(Airport? other)
        {
            var airport = other ?? new Airport();
            return this.Id.Equals(airport.Id);
        }
    }
}
