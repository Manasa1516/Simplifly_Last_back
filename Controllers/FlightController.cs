using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class FlightController : ControllerBase
    {
        private readonly IFlightFlightOwnerService _flightOwnerService;
        private readonly IFlightCustomerService _flightCustomerService;
        private readonly ILogger<FlightController> _logger;
        public FlightController(IFlightFlightOwnerService flightOwnerService, IFlightCustomerService flightCustomerService, ILogger<FlightController> logger)
        {
            _flightOwnerService = flightOwnerService;
            _flightCustomerService = flightCustomerService;
            _logger = logger;
        }


        [HttpGet]
        [EnableCors("ReactPolicy")]
        [Authorize(Roles ="flightOwner")]
        public async Task<ActionResult<List<Flight>>> GetAllFlight()
        {
            try
            {
                var flights = await _flightOwnerService.GetAllFlights();
                return flights;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("SearchFlight")]
        [HttpGet]
        [EnableCors("ReactPolicy")]
        public async Task<ActionResult<List<SearchedFlightResultDTO>>> GetAllFlights([FromQuery] SearchFlightDTO searchFlightDTO)
        {
            try
            {
                var flights = await _flightCustomerService.SearchFlights(searchFlightDTO);
                return flights;
            }
            catch (NoFlightAvailableException nfae)
            {
                _logger.LogInformation(nfae.Message);
                return NotFound(nfae.Message);
            }

        }

        [Route("GetFlightsByAirlineName")]
        [HttpGet]
        [EnableCors("ReactPolicy")]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<List<Flight>>> GetFlightGetFlightsByAirlineName(string AirlineName)
        {
            try
            {
                var flights = await _flightOwnerService.GetFlightByAirLine(AirlineName);
                return flights;
            }
             catch (NoFlightAvailableException nfae)
            {
                _logger.LogInformation(nfae.Message);
                return NotFound(nfae.Message);
            }

        }

        [HttpGet("GetAllFlights/flightOwnerId")]
        [EnableCors("ReactPolicy")]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<List<Flight>>> GetAllFlightsById(int flightOwnerId)
        {
            try
            {
                var flights = await _flightOwnerService.GetAllFlightsById(flightOwnerId);
                return flights;
            }
            catch (Exception)
            {
                _logger.LogInformation("You are not authorised user");
                return NotFound("You are not authorised user");
            }
        }


        [Route("AddFlight")]
        [HttpPost]
        [EnableCors("ReactPolicy")]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<Flight>> AddFlight(Flight flight)
        {
            try
            {
                flight = await _flightOwnerService.AddFlight(flight);
                return flight;
            }
            catch (FlightAlreadyPresentException fape)
            {
                _logger.LogInformation(fape.Message);
                return NotFound(fape.Message);
            }

        }


        [HttpPut]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<Flight>> UpdateFlightAirline(FlightAirlineDTO flightDTO)
        {

            try
            {
                var flight = await _flightOwnerService.UpdateAirline(flightDTO.FlightNumber, flightDTO.Airline);
                return flight;
            }
            catch (NoSuchFlightException nsfe)

            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }

        }

        [Route("UpdateTotalSeats")]
        [HttpPut]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<Flight>> UpdateTotalSeats(FlightSeatsDTO flightDTO)
        {
            try
            {
                var flight = await _flightOwnerService.UpdateTotalSeats(flightDTO.FlightNumber, flightDTO.TotalSeats);
                return flight;
            }
            catch (NoSuchFlightException nsfe)
            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }


        }

        [Route("UpdateStatus")]
        [HttpPut]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<Flight>> UpdateStatus(FlightStatusDTO flightstatusDTO)
        {
            try
            {
                var flight = await _flightOwnerService.UpdateStatus(flightstatusDTO.FlightNumber, flightstatusDTO.Status);
                return flight;
            }
            catch (NoSuchFlightException nsfe)
            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }


        }

        [HttpDelete]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<Flight>> RemoveFlight(string flightNumber)
        {
            try
            {
                var flight = await _flightOwnerService.RemoveFlight(flightNumber);
                return flight;
            }
            catch (NoSuchFlightException nsfe)
            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }


        }

    }
}
