using Simplifly.Models;
using Route = Simplifly.Models.Route;

namespace Simplifly.Interfaces
{
    public interface IRouteFlightOwnerService
    {
        public Task<Airport> AddAirport(Airport airport);
        public Task<Route> AddRoute(Route route);
        public Task<Route> RemoveRoute(int sourceAirportId, int destinationAirportId);
        public Task<List<Route>> GetAllRoutes();
        public Task<Route> GetRouteById(int id);
        public Task<int> GetRouteIdByAirport(int sourceAirportId,int destinationAirportId);
        public Task<List<Airport>> GetAllAirports();

        public Task<bool> RemoveRouteById(int routeId);
    }
}