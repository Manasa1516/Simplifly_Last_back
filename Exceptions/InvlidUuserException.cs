using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class InvlidUuserException : Exception
    {
        string message;
        public InvlidUuserException()
        {
            message = "Invalid username or password";
        }

        public override string Message => message;
    }
}
