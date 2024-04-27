using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _passengerService;
        public PassengerController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }


        [HttpGet]
        public Task<List<Passenger>> GetAllPassenger()
        {
            var passengers = _passengerService.GetAllPassengers();
            return passengers;
        }
        [HttpGet("ById")]
        public Task<Passenger> GetPassengerById(int id)
        {
            var passengers = _passengerService.GetByIdPassengers(id);
                return passengers;
        }
        [HttpPost]
        public async Task<Passenger> AddPassenger(Passenger passenger)
        {
            passenger = await _passengerService.AddPassenger(passenger);
            return passenger;
        }


       
    }
}
