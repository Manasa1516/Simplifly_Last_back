using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchFlightException:Exception
    {
        private readonly string message;
        public NoSuchFlightException()
        {
            message = "No flight found with given details";
        }
        public override string Message => message;
    }
}
