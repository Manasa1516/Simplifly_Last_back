using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;
using Simplifly.Repositories;

namespace Simplifly.Services
{
    public class FlightService : IFlightFlightOwnerService
    {
        private readonly IRepository<string, Flight> _flightRepository;
        private readonly ILogger<FlightService> _logger;

        /// <summary>
        /// Constructor to initialize the objects
        /// </summary>
        /// <param name="flightRepository"></param>
        /// <param name="logger"></param>
        public FlightService(IRepository<string, Flight> flightRepository, ILogger<FlightService> logger)
        {
            _flightRepository = flightRepository;
            _logger = logger;
        }

        /// <summary>
        ///Service class method to add flight 
        /// </summary>
        /// <param name="flight">Object of flight</param>
        /// <returns>Flight object</returns>
        /// <exception cref="FlightAlreadyPresentException">Throw when flight is already present</exception>
        public async Task<Flight> AddFlight(Flight flight)
        {
            try
            {
                await _flightRepository.GetAsync(flight.FlightNumber);
                throw new FlightAlreadyPresentException();
            }
            catch (NoSuchFlightException)
            {
                flight.Status = 1;
                flight = await _flightRepository.Add(flight);
                _logger.LogInformation("Flight added from service method");
                return flight;
            }

        }

        /// <summary>
        /// Service class method to get all flight
        /// </summary>
        /// <returns>List of flight</returns>
        public async Task<List<Flight>> GetAllFlights()
        {
            var flights = await _flightRepository.GetAsync();
            return flights;
        }

        /// <summary>
        /// Method to get flight by id
        /// </summary>
        /// <param name="id">flightId in string</param>
        /// <returns>Object of flight</returns>
        /// <exception cref="NoSuchFlightException">Throw when flight is not present</exception>
        public async Task<Flight> GetFlightById(string id)
        {
            var flights = await _flightRepository.GetAsync(id);
            if (flights != null)

            {
                return flights;
            }
            throw new NoSuchFlightException();
        }
        ///GEt flights by flightflightOwnerId
        public async Task<List<Flight>> GetAllFlightsById(int flightOwnerId)
        {
            var flights = await _flightRepository.GetAsync();
            List<Flight> ownedFlights = new List<Flight>();

            foreach (var flight in flights)
            {
                if (flight.FlightOwnerOwnerId == flightOwnerId)
                {
                    ownedFlights.Add(flight);
                }
            }

            if (ownedFlights != null)
            {
                return ownedFlights;
            }

            throw new NoSuchFlightException();
        }


        ///get flight by airline name

        public async Task<List<Flight>> GetFlightByAirLine(string Airline)
        {

            var flights = await _flightRepository.GetAsync();
            var flight = flights.Where(e => e.Airline == Airline).ToList();

            if (flight != null)

            {
                return flight;
            }
            throw new NoSuchFlightException();
        }

        /// <summary>
        /// Service class method to remove flight
        /// </summary>
        /// <param name="flightNumber">Flight number in string</param>
        /// <returns>Object of flight</returns>
        /// <exception cref="NoSuchFlightException">throw when flight is not present</exception>
        public async Task<Flight> RemoveFlight(string flightNumber)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if (flight != null)
            {
                flight.Status = 0;
                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight status changed to 0");
                return flight;
            }
            throw new NoSuchFlightException();
        }


        /// <summary>
        /// Service class method to update airline of flight
        /// </summary>
        /// <param name="flightNumber">Flight Number in string</param>
        /// <param name="airline">airline name in string</param>
        /// <returns>Object of flight</returns>
        /// <exception cref="NoSuchFlightException">throw when flight is not present</exception>
        public async Task<Flight> UpdateAirline(string flightNumber, string airline)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if (flight != null)
            {
                flight.Airline = airline;
                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight updated from service method");
                return flight;
            }
            throw new NoSuchFlightException();
        }

        /// <summary>
        /// Service class method to update total seats of flight
        /// </summary>
        /// <param name="flightNumber">Flight Number in string</param>
        /// <param name="totalSeats">total seats in int</param>
        /// <returns>Object of flight</returns>
        /// <exception cref="NoSuchFlightException">throw when flight is not present</exception>
        public async Task<Flight> UpdateTotalSeats(string flightNumber, int totalSeats)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if (flight != null)
            {
                flight.TotalSeats = totalSeats;
                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight updated from service method");
                return flight;
            }
            throw new NoSuchFlightException();
        }
        public async Task<Flight> UpdateStatus(string flightNumber, int status)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if (flight != null)
            {
                flight.Status = status;
                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight updated from service method");
                return flight;
            }
            throw new NoSuchFlightException();
        }
    }
}