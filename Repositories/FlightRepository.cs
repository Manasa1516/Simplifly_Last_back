using Microsoft.EntityFrameworkCore;
using Simplifly.Context;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Repositories
{
    [ExcludeFromCodeCoverage]
    public class FlightRepository : IRepository<string, Flight>
    {
        private readonly RequestTrackerContext _context;
        private readonly ILogger<FlightRepository> _logger;

        /// <summary>
        /// Default constructor with RequestTrackerContext
        /// </summary>
        /// <param name="context">Database context</param>
        public FlightRepository(RequestTrackerContext context, ILogger<FlightRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Method to add Flight to the database
        /// </summary>
        /// <param name="items">Object of Flight</param>
        /// <returns>Flight object</returns>
        public async Task<Flight> Add(Flight items)
        {            
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("Flight added "+ items.FlightNumber);
            return items;
        }

        /// <summary>
        /// Method to delete Flight from database
        /// </summary>
        /// <param name="items">Object of Flight</param>
        /// <returns>Flight object</returns>
        /// <exception cref="NoSuchFlightException">throws exception if no booking found</exception>
        public async Task<Flight> Delete(string flightNumber)
        {
            var flight = await GetAsync(flightNumber);
            if (flight != null)
            {
                _context.Remove(flight);
                _context.SaveChanges();
                _logger.LogInformation("Flight deleted with flight number" + flightNumber);
                return flight;
            }
            throw new NoSuchFlightException();
        }

        /// <summary>
        /// Method to get Flight data of specific Id
        /// </summary>
        /// <param name="key">key in int</param>
        /// <returns>Flight Object</returns>
        /// <exception cref="NoSuchFlightException">throws exception if no Flight found.</exception>
        public async Task<Flight> GetAsync(string key)
        {
            var flights= await GetAsync();
            var flight= flights.FirstOrDefault(e=>e.FlightNumber==key);
            if (flight != null)
            {
                return flight;
            }
            throw new NoSuchFlightException();
        }

 


        /// <summary>
        /// Method to get list of Flight
        /// </summary>
        /// <returns>Flight objects</returns>
        public async Task<List<Flight>> GetAsync()
        {
            var flights = _context.Flights.ToList();
            return flights;
        }

        /// <summary>
        /// Method to update Flight.
        /// </summary>
        /// <param name="items">Object of Flight</param>
        /// <returns>Flight Object</returns>
        /// <exception cref="NoSuchFlightException">throws exception if no Flight found</exception</exception>
        public async Task<Flight> Update(Flight items)
        {
            var flight = await GetAsync(items.FlightNumber);
            if(flight!= null)
            {
                _context.Entry<Flight>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Flight updated with flight number" + items.FlightNumber);
                return flight;
            }
            throw new NoSuchFlightException();
        }
    }
}
