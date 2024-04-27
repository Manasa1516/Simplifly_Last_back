using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;
using Simplifly.Repositories;
using System.Numerics;

namespace Simplifly.Services
{
    public class FlightOwnerService : IFlightOwnerService
    {
        private readonly IRepository<string, User> _userRepository;
        private readonly IRepository<int, FlightOwner> _flightownerRepository;
        private readonly ILogger<FlightOwnerService> _logger;
        public FlightOwnerService(IRepository<int, FlightOwner> flightownerRepository, IRepository<string, User> userRepository, ILogger<FlightOwnerService> logger)
        {
            _userRepository = userRepository;
            _flightownerRepository = flightownerRepository;
            _logger = logger;

        }

        /// <summary>
        /// Method to add flight owner
        /// </summary>
        /// <param name="flightOwner">Object of FlightOwner</param>
        /// <returns>FlightOwner</returns>
        public async Task<FlightOwner> AddFlightOwner(FlightOwner flightOwner)
        {
            return await _flightownerRepository.Add(flightOwner);
        }

        /// <summary>
        /// Method to remove flightOwner
        /// </summary>
        /// <param name="id">FlightOwenrId in int</param>
        /// <returns></returns>
        public async Task<bool> RemoveFlightOwner(int id)

        {

            var owner = await _flightownerRepository.GetAsync(id);
            if (owner != null)
            {
                await _flightownerRepository.Delete(id);
                await _userRepository.Delete(owner.Username);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to get all flightOwners
        /// </summary>
        /// <returns>List of FlightOwner</returns>
        public async Task<List<FlightOwner>> GetAllFlightOwners()
        {
            return await _flightownerRepository.GetAsync();
        }

        /// <summary>
        /// Method to update flightOwner address
        /// </summary>
        /// <param name="id">FlightOwnerId in int</param>
        /// <param name="address">address in string</param>
        /// <returns></returns>
        public async Task<FlightOwner> UpdateFlightOwnerAddress(int id, string address)
        {
            var owner = await _flightownerRepository.GetAsync(id);
            if (owner != null)
            {
                owner.Address = address;
                owner = await _flightownerRepository.Update(owner);
                return owner;
            }
            return null;
        }

        /// <summary>
        /// Method to get flightOwner by username
        /// </summary>
        /// <param name="username">username in string</param>
        /// <returns>Object of FlightOwner</returns>
        /// <exception cref="NoSuchFlightOwnerException">Throw when no flightOwner with given username is found</exception>
        public async Task<FlightOwner> GetByUsernameFlightOwners(string username)
        {
            var flightOwners = await _flightownerRepository.GetAsync();
            var flightOwner=flightOwners.FirstOrDefault(e=>e.Username==username);
            if(flightOwner != null)
            {
                return flightOwner;
            }
            throw new NoSuchFlightOwnerException();
        }

        /// <summary>
        /// Method to get flight owner by ID
        /// </summary>
        /// <param name="id">FlightOwnerId in int</param>
        /// <returns>Object of flightOwner</returns>
        /// <exception cref="NoSuchFlightOwnerException">Throw when no flightOwner with given id is found</exception>
        public async Task<FlightOwner> GetFlightOwnerById(int id)
        {
            var flightOwners = await _flightownerRepository.GetAsync();
            var flightOwner= flightOwners.FirstOrDefault(e=>e.OwnerId==id);
            if (flightOwner != null)
            {
                return flightOwner;
            }
            throw new NoSuchFlightOwnerException();
        }

        /// <summary>
        /// Method to update flightOwner
        /// </summary>
        /// <param name="flightOwner">Object of UpdateFlightOwnerDTO</param>
        /// <returns>FlightOwner object</returns>
        /// <exception cref="NoSuchFlightOwnerException">hrow when no flightOwner with given id is found</exception>
        public async Task<FlightOwner> UpdateFlightOwner(UpdateFlightOwnerDTO flightOwner)
        {
            var owner = await _flightownerRepository.GetAsync(flightOwner.OwnerId);
            if(owner != null)
            {
                owner.Name = flightOwner.Name;
                owner.Email=flightOwner.Email;
                owner.ContactNumber=flightOwner.ContactNumber;
                owner.CompanyName=flightOwner.CompanyName;
                owner.Address = flightOwner.Address;
                owner = await _flightownerRepository.Update(owner);
                return owner;
            }
            throw new NoSuchFlightOwnerException();

        }
    }
}