using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models
{
    [ExcludeFromCodeCoverage]
    public class SeatDetail : IEquatable<SeatDetail>
    {
        [Key]
        public string SeatNumber { get; set; } = string.Empty;
        public string SeatClass { get; set; } = string.Empty;

        public string? FlightNumber { get; set; }

        [ForeignKey("FlightNumber")]
        public Flight? Flight { get; set; }
        public int isBooked { get; set; } = 0;

        public SeatDetail()
        {

        }

        public SeatDetail(string seatNumber, string seatClass)
        {
            SeatNumber = seatNumber;
            SeatClass = seatClass;

        }

        public bool Equals(SeatDetail? other)
        {
            var seatDetail = other ?? new SeatDetail();
            return this.SeatNumber.Equals(seatDetail.SeatNumber);
        }
    }
}
