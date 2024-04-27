using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplifly.Models
{
    public class CancelledBooking
    {
        [Key]
        public int Id { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }
        public string RefundStatus { get; set; } = string.Empty;

        public CancelledBooking()
        {

        }
        public CancelledBooking(int id, int bookingId, Booking? booking, string refundStatus)
        {
            Id = id;
            BookingId = bookingId;
            Booking = booking;
            RefundStatus = refundStatus;
        }
    }
}
