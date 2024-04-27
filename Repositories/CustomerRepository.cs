using Microsoft.EntityFrameworkCore;
using Simplifly.Context;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CustomerRepository : IRepository<int, Customer>
    {
        private readonly RequestTrackerContext _context;
        private readonly ILogger<CustomerRepository> _logger;
        /// <summary>
        /// Default constructor with RequestTrackerContext
        /// </summary>
        /// <param name="context">Database context</param>
        public CustomerRepository(RequestTrackerContext context,ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Method to add Customer
        /// </summary>
        /// <param name="items">Object of Customer</param>
        /// <returns>Customer object</returns>
        public async Task<Customer> Add(Customer items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation($"Customer added with id {items.UserId}");
            return items;
        }

        /// <summary>
        /// Method to delete Customer
        /// </summary>
        /// <param name="ownerId">OwnerId in int</param>
        /// <returns>Object of Customer</returns>
        /// <exception cref="NoSuchCustomerException">when customer with given id not found</exception>
        public async Task<Customer> Delete(int ownerId)
        {
            var customer = await GetAsync(ownerId);
            if (customer != null)
            {
                _context.Remove(customer);
                _context.SaveChanges();
                _logger.LogInformation($"Customer deleted with id {ownerId}");
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        /// <summary>
        /// method to get customer by id
        /// </summary>
        /// <param name="key">id in int</param>
        /// <returns>Object of customer</returns>
        /// <exception cref="NoSuchCustomerException">when customer with given id not found</exception>
        public async Task<Customer> GetAsync(int key)
        {
            var customers = await GetAsync();
            var customer = customers.FirstOrDefault(e => e.UserId == key);
            if (customer != null)
            {
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        /// <summary>
        /// Method to get all customer
        /// </summary>
        /// <returns>List of customer</returns>
        public async Task<List<Customer>> GetAsync()
        {
            var customers = _context.Customers.ToList();
            return customers;
        }


        /// <summary>
        /// Method to update customer 
        /// </summary>
        /// <param name="items">Object of customer</param>
        /// <returns>Object of customer</returns>
        /// <exception cref="NoSuchCustomerException">if customer not found</exception>
        public async Task<Customer> Update(Customer items)
        {
            var customer = await GetAsync(items.UserId);
            if (customer != null)
            {
                _context.Entry<Customer>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Customer updated with id {items.UserId}");
                return customer;
            }
            throw new NoSuchCustomerException();
        }
    }
}
