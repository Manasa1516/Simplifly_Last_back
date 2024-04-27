using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace Simplifly.Models
{
    [ExcludeFromCodeCoverage]
    public class Passenger : IEquatable<Passenger>
    {
        [Key]
        public int PassengerId { get; set; }
        public string Name { get; set; } =string.Empty;
        public int Age { get; set; } = 0;
        public string PassportNo { get; set; } = string.Empty;
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public Customer? Customer { get; set; }

        public Passenger()
        {
            PassengerId = 0;
        }

        public Passenger(string name,int age, string passportNo)

        {
            Name = name;
            Age = age;
            PassportNo = passportNo;
        }

        public Passenger(string name,int passengerId, int age, string passportNo)
        {

            PassengerId = passengerId;
            Name = name;
            Age = age;
            PassportNo = passportNo;
        }

        public bool Equals(Passenger? other)
        {
            var Passenger = other ?? new Passenger();
            return this.PassengerId.Equals(Passenger.PassengerId) && this.PassportNo.Equals(Passenger.PassportNo);
        }
    }
}
