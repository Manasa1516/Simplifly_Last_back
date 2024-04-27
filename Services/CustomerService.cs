using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;
using Simplifly.Repositories;
using System.Numerics;

namespace Simplifly.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<string, User> _userRepository;
        private readonly IRepository<int, Customer> _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="logger"></param>
        public CustomerService(IRepository<int, Customer> customerRepository, IRepository<string, User> userRepository, ILogger<CustomerService> logger)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _logger = logger;

        }

        /// <summary>
        /// Method to add customer
        /// </summary>
        /// <param name="customer">Object of customer</param>
        /// <returns>Customer Object</returns>
        public async Task<Customer> AddCustomer(Customer customer)
        {
            return await _customerRepository.Add(customer);
        }

        /// <summary>
        /// Method to remove customer
        /// </summary>
        /// <param name="id">customerId in int</param>
        /// <returns></returns>
        public async Task<bool> RemoveCustomer(int id)
        {
            var cust = await _customerRepository.Delete(id);
            if (cust != null)
            {
                var user = await _userRepository.Delete(cust.Username);
                await _userRepository.Delete(cust.Username);
                _logger.LogInformation("Customer removed with id " + id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to get all customer
        /// </summary>
        /// <returns>List of Customer</returns>
        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _customerRepository.GetAsync();
        }

        /// <summary>
        /// Method to update customer email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Customer> UpdateCustomerEmail(int id, string email)
        {
            var cust = await _customerRepository.GetAsync(id);
            if (cust != null)
            {
                cust.Email = email;
                cust = await _customerRepository.Update(cust);
                return cust;
            }
            return null;
        }

        /// <summary>
        /// Method to get customer by Id
        /// </summary>
        /// <param name="id">Id in int</param>
        /// <returns>Object of Customer</returns>
        /// <exception cref="NoSuchCustomerException">throw when no customer with id found</exception>
        public async Task<Customer> GetByIdCustomers(int id)
        {
            var customer= await (_customerRepository.GetAsync(id));
            if(customer != null)
            {
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        /// <summary>
        /// Method to get customer by username
        /// </summary>
        /// <param name="username">username in string</param>
        /// <returns>Object of Customer</returns>
        /// <exception cref="NoSuchCustomerException">throw when no customer with username found</exception>
        public async Task<Customer> GetCustomersByUsername(string username)
        {
            var customers = await _customerRepository.GetAsync();
            var customer=customers.FirstOrDefault(e=>e.Username==username);
            if(customer!= null)
            {
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        /// <summary>
        /// Method to update customer
        /// </summary>
        /// <param name="customer">Object of customer</param>
        /// <returns>Customer object</returns>
        /// <exception cref="NoSuchCustomerException">throw when no customer with username found</exception>
        public async Task<Customer> UpdateCustomer(UpdateCustomerDTO customer)
        {
            var customers = await _customerRepository.GetAsync(customer.UserId);
            if (customer != null)
            {
                customers.Name=customer.Name;
                customers.Email=customer.Email;
                customers.Phone=customer.Phone;
                customers = await _customerRepository.Update(customers);
                return customers;
            }
            throw new NoSuchCustomerException();
        }
    }
}
