using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchSeatException:Exception
    {
        private readonly string message;
        public NoSuchSeatException()
        {
            message = "No seat found with given details";
        }
        public override string Message => message;
    }
}
