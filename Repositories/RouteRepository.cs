using Microsoft.EntityFrameworkCore;
using Simplifly.Context;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Repositories
{
    [ExcludeFromCodeCoverage]
    public class RouteRepository : IRepository<int, Models.Route>
    {
        RequestTrackerContext _context;
        ILogger<RouteRepository> _logger;

        /// <summary>
        /// Default constructor with RequestTrackerContext
        /// </summary>
        /// <param name="context">Database context</param>
        public RouteRepository(RequestTrackerContext context, ILogger<RouteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Method to add Route to the database
        /// </summary>
        /// <param name="items">Object of Route</param>
        /// <returns>Route object</returns>
        public async Task<Models.Route> Add(Models.Route items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("Route added with routeId" + items.Id);
            return items;
        }

        /// <summary>
        /// Method to delete Route from database
        /// </summary>
        /// <param name="items">Object of Route</param>
        /// <returns>Route object</returns>
        /// <exception cref="NoSuchRouteException">throws exception if no Route found</exception>
        public async  Task<Models.Route> Delete(int routeId)
        {
            var route = await GetAsync(routeId);
            if (route != null)
            {
                _context.Remove(route);
                _context.SaveChanges();
                _logger.LogInformation("Route deleted with routeId" + routeId);
                return route;
            }
            throw new NoSuchRouteException();
        }

        /// <summary>
        /// Method to get Route data of specific Id
        /// </summary>
        /// <param name="key">key in int</param>
        /// <returns>Route Object</returns>
        /// <exception cref="NoSuchRouteException">throws exception if no Route found.</exception>
        public async Task<Models.Route> GetAsync(int key)
        {
            var routes= await GetAsync();
            var route = routes.FirstOrDefault(e => e.Id == key);
            if (route != null)
            {
                return route;
            }
            throw new NoSuchRouteException();
        }

        /// <summary>
        /// Method to get list of Route
        /// </summary>
        /// <returns>Route object</returns>
        public async Task<List<Models.Route>> GetAsync()
        {
            var routes = _context.Routes.Include(e=>e.SourceAirport).Include(d=>d.DestinationAirport).ToList();
            return routes;
        }

        /// <summary>
        /// Method to update Route.
        /// </summary>
        /// <param name="items">Object of Route</param>
        /// <returns>Airport Object</returns>
        /// <exception cref="NoSuchRouteException">throws exception if no Route found</exception</exception>
        public async Task<Models.Route> Update(Models.Route items)
        {
            var route= await GetAsync(items.Id);
            if (route != null)
            {
                _context.Entry<Models.Route>(route).State=EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Route updated with routeId" + items.Id);
                return route;
            }
            throw new NoSuchRouteException();
        }
    }
}
