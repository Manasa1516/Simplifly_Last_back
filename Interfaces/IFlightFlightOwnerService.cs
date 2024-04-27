using Simplifly.Models;
using Route = Simplifly.Models.Route;

namespace Simplifly.Interfaces
{
    public interface IFlightFlightOwnerService
    {
        public Task<Flight> AddFlight(Flight flight);
        public Task<Flight> RemoveFlight(string flightNumber);
        public Task<List<Flight>> GetAllFlights();
        public Task<Flight> GetFlightById(string id);
        public Task<List<Flight>> GetAllFlightsById(int flightOwnerId);
        public Task<List<Flight>> GetFlightByAirLine(string id);
        public Task<Flight> UpdateAirline(string flightNumber, string airline);
        public Task<Flight> UpdateTotalSeats(string flightNumber, int totalSeats);
        public Task<Flight> UpdateStatus(string flightNumber, int Status);



    }
}