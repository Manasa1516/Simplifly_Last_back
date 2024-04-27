using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using static Simplifly.Models.Booking;

namespace Simplifly.Models
{
    [ExcludeFromCodeCoverage]
    public class Booking:IEquatable<Booking>
    {
        [Key]
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        //This one is just for navigation and will not be created as an attribute in table
        [ForeignKey("ScheduleId")]
        public Schedule? Schedule { get; set; }

        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment? Payment { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public DateTime BookingTime { get; set; }

        public  string bookingStatus { get; set; }
        public double TotalPrice { get; set; }

        public Booking()   
        {
            Id = 0;

        }
        public Booking( int scheduleId,int paymentid, int userId, DateTime bookingTime, double totalPrice,string bookingstatus )
        {
            ScheduleId = scheduleId;
            UserId = userId;
            BookingTime = bookingTime;
            TotalPrice = totalPrice;
            PaymentId = paymentid;
            bookingStatus = bookingstatus;

            
        }
        

        public Booking(int id,int scheduleId, int paymentid, int userId, DateTime bookingTime, double totalPrice, string bookingstatus)
        {
            Id = id;
            ScheduleId = scheduleId;
            UserId = userId;
            BookingTime = bookingTime;
            TotalPrice = totalPrice;
            PaymentId = paymentid;
            bookingStatus = bookingstatus;
        }

        public bool Equals(Booking? other)
        {
            var booking = other ?? new Booking();
            return this.Id.Equals(booking.Id);
        }
    }
}
