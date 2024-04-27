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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleFlightOwnerService _scheduleFlightOwnerService;
        private readonly ILogger<ScheduleController> _logger;
        public ScheduleController(IScheduleFlightOwnerService scheduleFlightOwnerService, ILogger<ScheduleController> logger)
        {
            _scheduleFlightOwnerService = scheduleFlightOwnerService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<List<Schedule>>> GetAllSchedule()
        {
            try
            {
                var schedules = await _scheduleFlightOwnerService.GetAllSchedules();
                return schedules;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }


        }

        [Route("FlightSchedule")]
        [HttpGet]
        [EnableCors("RequestPolicy")]
        public async Task<ActionResult<List<FlightScheduleDTO>>> GetFlightSchedule([FromQuery] string flightNumber)
        {
            try
            {
                var flightSchedule = await _scheduleFlightOwnerService.GetFlightSchedules(flightNumber);
                return flightSchedule;
            }
            catch (NoSuchScheduleException nsse)

            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<Schedule>> AddSchedule(Schedule schedule)
        {
            try
            {
                schedule = await _scheduleFlightOwnerService.AddSchedule(schedule);
                return schedule;
            }
            catch (FlightScheduleBusyException fsbe)

            {
                _logger.LogInformation(fsbe.Message);
                return NotFound(fsbe.Message);
            }
        }

        [Route("UpdateScheduledFlight")]
        [HttpPut]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<Schedule>> UpdateScheduledFlight(ScheduleFlightDTO scheduleFlightDTO)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService.
                                UpdateScheduledFlight(scheduleFlightDTO.ScheduleId, scheduleFlightDTO.FlightNumber);
                return schedule;
            }
            catch (NoSuchScheduleException nsse)

            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [Route("UpdateScheduledRoute")]
        [HttpPut]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<Schedule>> UpdateScheduledRoute(ScheduleRouteDTO scheduleRouteDTO)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService.
                UpdateScheduledRoute(scheduleRouteDTO.ScheduleId, scheduleRouteDTO.RouteId);
                return schedule;
            }
            catch (NoSuchScheduleException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [Route("UpdateScheduledTime")]
        [HttpPut]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<Schedule>> UpdateScheduledTime(ScheduleTimeDTO scheduleTimeDTO)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService.
                UpdateScheduledTime(scheduleTimeDTO.ScheduleId, scheduleTimeDTO.DepartureTime,
                scheduleTimeDTO.ArrivalTime);
                return schedule;
            }
            catch (NoSuchScheduleException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [Route("DeleteScheduleByFlight")]
        [HttpDelete]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<int>> DeleteScheduleByFlight(string flightNumber)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService.RemoveSchedule(flightNumber);
                return schedule;
            }
            catch (NoSuchScheduleException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }
        [Route("DeleteScheduleByDate")]
        [HttpDelete]
        [Authorize(Roles = "flightOwner")]
        public async Task<ActionResult<int>> DeleteScheduleByDate(RemoveScheduleDateDTO scheduleDTO)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService.RemoveSchedule(scheduleDTO.DateOfSchedule,scheduleDTO.AirportId);
                return schedule;
            }
            catch (NoSuchScheduleException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }
    }
}