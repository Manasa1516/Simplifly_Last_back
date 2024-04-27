using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchPassengerBookingException : Exception
    {
        private readonly string message;
        public NoSuchPassengerBookingException()
        {
            message = "No PassengerBooking found with given details";
        }
        public override string Message => message;
    }
}
