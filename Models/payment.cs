using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models
{
    [ExcludeFromCodeCoverage]
    public class Payment : IEquatable<Payment>
    {
        [Key]
        public int PaymentId { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }

        // Payment details (e.g., card number, expiry date, CVV)
        public PaymentDetails PaymentDetails { get; set; } = new PaymentDetails();



        public Payment()
        {
            PaymentId = 0;
        }

        public Payment(int paymentId, double amount, DateTime paymentDate, PaymentStatus status, PaymentDetails paymentDetails, Booking booking)
        {
            PaymentId = paymentId;
            Amount = amount;
            PaymentDate = paymentDate;
            Status = status;
            PaymentDetails = paymentDetails;

        }
        public Payment( double amount, DateTime paymentDate, PaymentStatus status, PaymentDetails paymentDetails, Booking booking)
        {
            Amount = amount;
            PaymentDate = paymentDate;
            Status = status;
            PaymentDetails = paymentDetails;
          
        }
        public bool Equals(Payment? other)
        {
            var Payment = other ?? new Payment();
            return this.PaymentId.Equals(Payment.PaymentId) ;
        }
    }

    public enum PaymentStatus
    {
        Pending,
        Successful,
        Failed,
        RefundRequested,
        RefundIssued
    }

    public class PaymentDetails
    {
        public int PaymentDetailsId { get; set; }

        public string CardNumber { get; set; } =string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;

    }

}
