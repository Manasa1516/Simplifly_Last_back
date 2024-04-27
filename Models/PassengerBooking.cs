using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Simplifly.Models
{
    [ExcludeFromCodeCoverage]
    public class PassengerBooking : IEquatable<PassengerBooking>
    {
        [Key]
        public int Id { get; set; }
        public int? BookingId { get; set; }
        //This one is just for navigation and will not be created as an attribute in table
        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        public int? PassengerId { get; set; }
        //This one is just for navigation and will not be created as an attribute in table
        [ForeignKey("PassengerId")]
        public Passenger? Passenger { get; set; }

        public string SeatNumber { get; set; }
        //This one is just for navigation and will not be created as an attribute in table
        [ForeignKey("SeatNumber")]
        public SeatDetail? SeatDetail { get; set; }


        public PassengerBooking()
        {
            Id = 0;
        }
        public PassengerBooking(int id)
        {
            Id = id;
           
        }

        public PassengerBooking(int id, int? bookingId, int? passengerId, string? seatId) : this(id)
        {
            BookingId = bookingId;
            PassengerId = passengerId;
            SeatNumber = seatId;
        }

        public bool Equals(PassengerBooking? other)
        {
            var passengerBooking = other ?? new PassengerBooking();
            return this.Id.Equals(passengerBooking.Id);
        }

    }
}
