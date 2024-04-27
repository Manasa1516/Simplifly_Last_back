using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchPaymentException : Exception
    {
        private readonly string message;
        public NoSuchPaymentException()
        {
            message = "No Payment found with given details";
        }
        public override string Message => message;
    }
}
