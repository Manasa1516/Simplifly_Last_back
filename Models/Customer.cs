using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Models
{
    [ExcludeFromCodeCoverage]
    public class Customer:IEquatable<Customer>
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }= string.Empty;
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string Username { get; set; }
        [ForeignKey("Username")]
        public User User { get; set; }
        //Navigation
        //public List<Booking>? Bookings { get; set; }

        public Customer()
        {
            UserId = 0;
        }
        public Customer( string name, string email, string? phone,string? gender,string password)
        {

 
            Name = name;
            Email = email;
            Phone = phone;
            Gender = gender;
           
        }

        public Customer(int userId, string name, string email, string? phone,string? gender,string password)
        {
            UserId = userId;
            Name = name;
            Email = email;
            Phone = phone;
            Gender = gender;

        }

        public bool Equals(Customer? other)
        {
            var User = other ?? new Customer();
            return this.UserId.Equals(User.UserId) && this.Email.Equals(User.Email);
        }
    }
}
