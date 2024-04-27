using Microsoft.EntityFrameworkCore;
using Simplifly.Context;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Repositories
{
    [ExcludeFromCodeCoverage]
    public class FlightOwnerRepository: IRepository<int, FlightOwner>
    {
        private readonly RequestTrackerContext _context;
        private readonly ILogger<FlightOwnerRepository> _logger;

        /// <summary>
        /// Default constructor with RequestTrackerContext
        /// </summary>
        /// <param name="context">Database context</param>
        public FlightOwnerRepository(RequestTrackerContext context, ILogger<FlightOwnerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Method to add flightOwner
        /// </summary>
        /// <param name="items">Object of FlightOwner</param>
        /// <returns>FlightOwner object</returns>
        public async Task<FlightOwner> Add(FlightOwner items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("FlightOwner added " + items.OwnerId);
            return items;
        }

        /// <summary>
        /// Method to delete FlightOwner
        /// </summary>
        /// <param name="Id">Id in int</param>
        /// <returns>Object of flight owner</returns>
        /// <exception cref="NoSuchFlightOwnerException">when flightowner with given id nnot found</exception>
        public async Task<FlightOwner> Delete(int Id)
        {
            var owner = await GetAsync( Id);
            if (owner != null)
            {
                _context?.Remove(owner);
                _context.SaveChanges();
                _logger.LogInformation("FlightOwner deleted with id" + Id);
                return owner;
            }
            throw new NoSuchFlightOwnerException();
        }

        /// <summary>
        /// method to get flightOwner by id
        /// </summary>
        /// <param name="key">Id in int</param>
        /// <returns>Object of flightOWner</returns>
        /// <exception cref="NoSuchFlightOwnerException"></exception>
        public async Task<FlightOwner> GetAsync(int key)
        {
            var flightOwners = await GetAsync();
            var flightOwner = flightOwners.FirstOrDefault(e => e.OwnerId == key);
            if (flightOwner != null)
            {
                return flightOwner;
            }
            throw new NoSuchFlightOwnerException();
        }

        /// <summary>
        /// method to get all flightOwner
        /// </summary>
        /// <returns>List of flightOwner</returns>
        public async Task<List<FlightOwner>> GetAsync()
        {
            var flightOwners = _context.FlightsOwners.ToList();
            return flightOwners;
        }

        /// <summary>
        /// Method to update flight owner
        /// </summary>
        /// <param name="items">Object of flight Owner</param>
        /// <returns>Flight owner object</returns>
        public async Task<FlightOwner> Update(FlightOwner items)
        {
            var flightOwner = await GetAsync(items.OwnerId);
            
                _context.Entry<FlightOwner>(items).State = EntityState.Modified;
                _context.SaveChanges();
            _logger.LogInformation("FlightOwner updated with id" + items.OwnerId);
            return flightOwner;

        }
    }
}
