﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;
using Simplifly.Services;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;
        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [Authorize(Roles ="flight owner")]
        [Route("GetBookingByFlight")]
        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetBookingByFlight(string flightNumber)
        {
            try
            {
                var bookings = await _bookingService.GetBookingByFlight(flightNumber);
                return bookings;
            }
            catch(NoSuchBookingsException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }

        [Route("GetBookedSeats")]
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetBookedSeats(int scheduleId)
        {
            try
            {
                var bookedSeats=await _bookingService.GetBookedSeatBySchedule(scheduleId);
                return bookedSeats;
            }
            catch(NoSuchBookingsException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }
        [Route("UpdateRefundStatus")]
        [HttpPut]
        [Authorize(Roles = "flightOwner, admin")]
        public async Task<IActionResult> UpdateRefundStatus(BookingStatusDTO refundStatusDTO)
        {
            try
            {
                await _bookingService.UpdateRefundStatus(refundStatusDTO.Id, refundStatusDTO.RefundStatus);
                return Ok();
            }
            catch (NoSuchBookingsException nscbe)
            {
                _logger.LogInformation(nscbe.Message);
                return NotFound(nscbe.Message);
            }
        }


    }
}
