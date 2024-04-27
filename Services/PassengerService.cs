using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Repositories;
using System.Numerics;

namespace Simplifly.Services
{
    public class PassengerService : IPassengerService
    {
       
        private readonly IRepository<int, Passenger> _passengerRepository;
        private readonly ILogger<PassengerService> _logger;

        /// <summary>
        /// Constructor for PassengerService
        /// </summary>
        /// <param name="passengerRepository"></param>
        /// <param name="logger"></param>
        public PassengerService(IRepository<int, Passenger> passengerRepository, ILogger<PassengerService> logger)
        {
            _passengerRepository = passengerRepository;
            _logger = logger;

        }

        /// <summary>
        /// Method to add Passenger
        /// </summary>
        /// <param name="passenger">Object of passenger</param>
        /// <returns>Object of passenger</returns>
        public async Task<Passenger> AddPassenger(Passenger passenger)
        {
            return await _passengerRepository.Add(passenger);
        }

        /// <summary>
        /// Method to remove passenger
        /// </summary>
        /// <param name="id">PassengerId in int</param>
        /// <returns>true if passenger is removed else false</returns>
        public async Task<bool> RemovePassenger(int id)

        {

            var owner = await _passengerRepository.GetAsync(id);
            if (owner != null)
            {
                await _passengerRepository.Delete(id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to get all passengers
        /// </summary>
        /// <returns>List of Passengers</returns>
        public async Task<List<Passenger>> GetAllPassengers()
        {
            return await _passengerRepository.GetAsync();
        }

        
        /// <summary>
        /// Method to get Passenger by id
        /// </summary>
        /// <param name="id">Passenger id in int</param>
        /// <returns>Object of passenger</returns>
        public async Task<Passenger> GetByIdPassengers(int id)
        {
            return await (_passengerRepository.GetAsync(id));
        }

        
    }
}
