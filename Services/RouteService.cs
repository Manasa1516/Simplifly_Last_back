using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Route = Simplifly.Models.Route;

namespace Simplifly.Services
{
    public class RouteService : IRouteFlightOwnerService
    {
        private readonly IRepository<int, Route> _routeRepository;
        private readonly ILogger<RouteService> _logger;
        private readonly IRepository<int, Airport> _airportRepository;
        /// <summary>
        /// Constructor to initialize the objects
        /// </summary>
        /// <param name="routeRepository"></param>
        /// <param name="logger"></param>
        public RouteService(IRepository<int, Route> routeRepository, ILogger<RouteService> logger)
        {
            _routeRepository = routeRepository;
            _logger = logger;

        }
        public RouteService(IRepository<int, Route> routeRepository, ILogger<RouteService> logger, IRepository<int, Airport> airportRepository)
        {
            _airportRepository = airportRepository;
            _routeRepository = routeRepository;
            _logger = logger;

        }

        /// <summary>
        /// Method to add airport
        /// </summary>
        /// <param name="airport">Object of airport</param>
        /// <returns>Airport Object</returns>
        /// <exception cref="AirportAlreadyPresentException">Throw when airport is already present</exception>
        public async Task<Airport> AddAirport(Models.Airport airport)
        {
            var airports = await _airportRepository.GetAsync();
            var existingAirport = airports.FirstOrDefault(e => e.Name == airport.Name && e.City == airport.City);
            if (existingAirport == null)

            {
                airport = await _airportRepository.Add(airport);
                return airport;
            }
            throw new AirportAlreadyPresentException();
        }

        /// <summary>
        ///Service class method to add route 
        /// </summary>
        /// <param name="route">Object of route</param>
        /// <returns>route object</returns>
        /// <exception cref="RouteAlreadyPresentException">Throw when route is already present</exception>
        public async Task<Route> AddRoute(Route route)
        {
            var existingRoutes = await GetAllRoutes();
            var existingRoute = existingRoutes.FirstOrDefault(s => s.SourceAirportId == route.SourceAirportId
             && s.DestinationAirportId == route.DestinationAirportId);

            if (existingRoute == null)
            {
                route.Status = 1;
                route = await _routeRepository.Add(route);
                return route;
            }
            throw new RouteAlreadyPresentException();
        }

        /// <summary>
        /// Method to get all airports
        /// </summary>
        /// <returns>List of Airports</returns>
        public async Task<List<Airport>> GetAllAirports()
        {
            var airports = await _airportRepository.GetAsync();
            return airports;
        }


        /// <summary>
        /// Service class method to get all Routes
        /// </summary>
        /// <returns>List of route</returns>
        public async Task<List<Route>> GetAllRoutes()
        {
            var routes = await _routeRepository.GetAsync();
            return routes;
        }

        /// <summary>
        /// Method to get route by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NoSuchRouteException"></exception>
        public async Task<Route> GetRouteById(int id)
        {
            var route = await _routeRepository.GetAsync(id);

            if (route != null)
            {
                return route;
            }
            throw new NoSuchRouteException();
        }

        /// <summary>
        /// Method to get routeId by airports
        /// </summary>
        /// <param name="sourceAirportId">sourceAirportId in int</param>
        /// <param name="destinationAirportId">destinationAirportId in int</param>
        /// <returns>RouteId in int</returns>
        /// <exception cref="NoSuchRouteException">throw when no route found</exception>
        public async Task<int> GetRouteIdByAirport(int sourceAirportId, int destinationAirportId)
        {
            var routes = await _routeRepository.GetAsync();
            var route = routes.FirstOrDefault(e => e.SourceAirportId == sourceAirportId && 
            e.DestinationAirportId == destinationAirportId);
            if(route != null)
            {
                return (int)route.Id;
            }
            throw new NoSuchRouteException();

        }

        /// <summary>
        /// Service class method to remove route
        /// </summary>
        /// <param name="sourceAirportId">Source airport id in int</param>
        /// <param name="destinationAirportId">Destination airport id in int</param>
        /// <returns>Object of route</returns>
        /// <exception cref="NoSuchRouteException">throw when route is not present</exception>
        public async Task<Route> RemoveRoute(int sourceAirportId, int destinationAirportId)
        {
            var routes = await _routeRepository.GetAsync();
            var route = routes.FirstOrDefault(e => e.SourceAirportId == sourceAirportId
            && e.DestinationAirportId == destinationAirportId);

            if (route != null)
            {
                int routeId = route.Id;
                route.Status = 0;
                route = await _routeRepository.Update(route);
                return route;
            }
            throw new NoSuchRouteException();

        }

        /// <summary>
        /// Method to remove route by Id
        /// </summary>
        /// <param name="routeId">RouteId in int</param>
        /// <returns>true if route is removed else false</returns>
        public async Task<bool> RemoveRouteById(int routeId)
        {
            if (await _routeRepository.Delete(routeId) != null)
            {
                return true;
            };
            return false;
        }

        
    }
}