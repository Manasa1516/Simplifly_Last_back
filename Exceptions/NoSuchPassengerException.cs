using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchPassengerException : Exception
    {
        private readonly string message;
        public NoSuchPassengerException()
        {
            message = "No Passenger found with given details";
        }
        public override string Message => message;
    }
}
