using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchFlightOwnerException:Exception
    {
        private readonly string message;
        public NoSuchFlightOwnerException()
        {
            message = "No Flight owner found with given details";
        }
        public override string Message => message;
    }
}
