using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models.DTO_s
{
    [ExcludeFromCodeCoverage]
    public class ForgotPasswordDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
