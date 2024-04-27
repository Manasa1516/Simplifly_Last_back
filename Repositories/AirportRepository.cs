using Microsoft.EntityFrameworkCore;
using Simplifly.Context;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Repositories
{
    [ExcludeFromCodeCoverage]
    public class AirportRepository : IRepository<int, Airport>
    {
        private readonly RequestTrackerContext _context;
        private readonly ILogger<AirportRepository> _logger;

        /// <summary>
        /// Default constructor with RequestTrackerContext
        /// </summary>
        /// <param name="context">Database context</param>
        public AirportRepository(RequestTrackerContext context, ILogger<AirportRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Method to add Airport to the database
        /// </summary>
        /// <param name="items">Object of Airport</param>
        /// <returns>Airport object</returns>
        public async Task<Airport> Add(Airport items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation($"Airport added with id {items.Id}");
            return items;
        }

        /// <summary>
        /// Method to delete Airport from database
        /// </summary>
        /// <param name="items">Object of Airport</param>
        /// <returns>Airport object</returns>
        /// <exception cref="NoSuchAirportException">throws exception if no airport found</exception>
        public Task<Airport> Delete(int airportId)
        {
            var airport = GetAsync(airportId);
            if (airport != null)
            {
                _context.Remove(airport);
                _context.SaveChanges();
                _logger.LogInformation($"Airport removed with id {airportId}");
                return airport;
            }
            throw new NoSuchAirportException();
        }

        /// <summary>
        /// Method to get Airport data of specific Id
        /// </summary>
        /// <param name="key">key in int</param>
        /// <returns>Airport Object</returns>
        /// <exception cref="NoSuchAirportException">throws exception if no airport found.</exception>
        public async Task<Airport> GetAsync(int key)
        {
            var airports = await GetAsync();
            var airport=airports.FirstOrDefault(e=>e.Id==key);
            if(airport != null)
            {
                return airport;
            }
            throw new NoSuchAirportException();
        }

        /// <summary>
        /// Method to get list of Airports
        /// </summary>
        /// <returns></returns>
        public async Task<List<Airport>> GetAsync()
        {
            var airports = _context.Airports.ToList();
            return airports;
        }

        /// <summary>
        /// Method to update airport.
        /// </summary>
        /// <param name="items">Object of Airport</param>
        /// <returns>Airport Object</returns>
        /// <exception cref="NoSuchAirportException">throws exception if no airport found</exception</exception>
        public async Task<Airport> Update(Airport items)
        {
            var airport = await GetAsync(items.Id);
            if (airport != null)
            {
                _context.Entry<Airport>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Airport updated with id {items.Id}");
                return airport;
            }
            throw new NoSuchAirportException();
        }
    }
}
