using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class FlightAlreadyPresentException:Exception
    {
        private readonly string message;
        public FlightAlreadyPresentException()
        {
            message = "Flight is already present.";
        }

        public override string Message => message;
    }
}
