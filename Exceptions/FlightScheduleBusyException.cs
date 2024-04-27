using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class FlightScheduleBusyException:Exception
    {
        private string _message;
        public FlightScheduleBusyException()
        {
            _message = "Flight is busy for this schedule.";
        }
        public override string Message => _message;
    }
}
