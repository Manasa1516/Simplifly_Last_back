using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoDataPresentException:Exception
    {
        private readonly string message;
        public NoDataPresentException()
        {
            message = "No data to show";
        }
        public override string Message => message;
    }
}
