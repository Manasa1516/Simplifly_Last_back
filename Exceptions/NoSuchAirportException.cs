using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchAirportException:Exception
    {
        private readonly string message;
        public NoSuchAirportException()
        {
            message = "No airport found with given details";
        }
        public override string Message => message;
    }
}
