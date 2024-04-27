using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchBookingsException:Exception
    {
        private readonly string message;
        public NoSuchBookingsException()
        {
            message = "No Booking found with given details";
        }
        public override string Message => message;
    }
}
