using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class AirportAlreadyPresentException : Exception

    {
        private string _message;
        public AirportAlreadyPresentException()
        {
            _message = "Airport with given details already exists";
        }

        public override string Message => _message;
    }
}

