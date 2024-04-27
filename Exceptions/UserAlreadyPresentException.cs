using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class UserAlreadyPresentException:Exception
    {
        private readonly string message;
        public UserAlreadyPresentException()
        {
            message = "User with username already present";
        }
        public override string Message => message;
    }
}
