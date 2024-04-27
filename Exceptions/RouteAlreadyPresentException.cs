using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class RouteAlreadyPresentException:Exception
    {
        private readonly string _message;
        public RouteAlreadyPresentException()
        {
            _message = "The route is already present";
        }
        public override string Message => _message;
    }
}
