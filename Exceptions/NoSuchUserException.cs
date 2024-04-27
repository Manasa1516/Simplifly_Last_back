using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchUserException:Exception
    {
        private readonly string message;
        public NoSuchUserException()
        {
            message = "No user with given username";
        }
        public override string Message => message;
    }
}
