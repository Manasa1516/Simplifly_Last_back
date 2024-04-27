using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchRouteException:Exception
    {
        private readonly string message;
        public NoSuchRouteException()
        {
            message = "No route found with given details";
        }
        public override string Message => message;
    }
}
