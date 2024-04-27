
using Simplifly.Models;
using Simplifly.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Security.Cryptography;
using System.Text;
using Simplifly.Models.DTO_s;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Mappers
{
    [ExcludeFromCodeCoverage]
    public class RegisterToUser
    {
        User user;
        public RegisterToUser(RegisterAdminUserDTO register)
        {
            
            user = new User();
            user.Username = register.Username;
            user.Role = register.Role;
            
            GetPassword(register.Password);
        }
        public RegisterToUser(RegisterFlightOwnerUserDTO register)
        {

            user = new User();
            user.Username = register.Username;
            user.Role = register.Role;

            GetPassword(register.Password);
        }
        public RegisterToUser(RegisterCustomerUserDTO register)
        {

            user = new User();
            user.Username = register.Username;
            user.Role = register.Role;

            GetPassword(register.Password);
        }
        public RegisterToUser(ForgotPasswordDTO userDTO)
        {

            user = new User();
            user.Username = userDTO.Username;

            GetPassword(userDTO.Password);
        }
        void GetPassword(string password)
        {
            HMACSHA512 hmac = new HMACSHA512();
            user.Key = hmac.Key;
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public User GetUser()
        {
            return user;
        }
    }
}