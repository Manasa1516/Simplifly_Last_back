using Microsoft.AspNetCore.Mvc;
using Simplifly.Interfaces;
using System.Threading.Tasks;
using Simplifly.Services;
using Simplifly.Models;
using Simplifly.Exceptions;
using Simplifly.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Controllers
{
    [Route("api/users")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class CustomerDashboardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;
        private readonly ILogger<CustomerDashboardController> _logger;

        public CustomerDashboardController(IUserService userService, ICustomerService customerService, IBookingService bookingService, ILogger<CustomerDashboardController> logger)
        {
            _userService = userService;
            _bookingService = bookingService;
            _customerService = customerService;
            _logger = logger;
        }

        //[HttpGet("{userId}/bookings")]
        //public async Task<IActionResult> GetBookings(int userId)
        //{
        //    var bookings = await _bookingService.GetUserBookingsAsync(userId);
        //    return Ok(bookings);
        //}

        [HttpPost("{userId}/bookings")]
        public async Task<IActionResult> BookTickets(int userId, [FromBody] BookingRequestDto bookingRequestDto)
        {
            try
            {
                bookingRequestDto.UserId = userId; // Set the user ID in the request DTO
                var bookingId = await _bookingService.CreateBookingAsync(bookingRequestDto);
                return Ok(bookingId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{userId}/bookings/{bookingId}")]
        public async Task<IActionResult> GetBooking(int userId, int bookingId)
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            if (booking == null || booking.UserId != userId)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpGet("GetCustomerByUsername")]
        public async Task<ActionResult<Customer>> GetCustomerByUsername(string username)
        {
            try
            {
                var customer = await _customerService.GetCustomersByUsername(username);
                return Ok(customer);
            }
            catch(NoSuchCustomerException nsce)
            {
                _logger.LogInformation(nsce.Message);   
                return NotFound(nsce.Message);
            }
            
        }

        [Route("GetCustomerById")]
        [HttpGet]
        public async Task<ActionResult<Customer>> GetCustomerById(int userId)
        {
            try
            {
                var customer=await _customerService.GetByIdCustomers(userId);
                return Ok(customer);
            }
            catch (NoSuchCustomerException nsce)
            {
                _logger.LogInformation(nsce.Message);
                return NotFound(nsce.Message);
            }
        }


        [HttpDelete("{userId}/bookings/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int userId, int bookingId)
        {
            var success = await _bookingService.CancelBookingAsync(bookingId);
            if (success != null)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpGet("{userId}/GetBookings")]
        public async Task<IActionResult> GetBookingHistory(int userId)
        {
            var bookingHistory = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(bookingHistory);
        }
        [Route("GetBookingByCustomerId")]
        [HttpGet]
        public async Task<ActionResult<List<PassengerBooking>>> GetBookingByCustomerId(int customerId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByCustomerId(customerId);
                return Ok(bookings);
            }
            catch(NoSuchCustomerException nsce)
            {
                _logger.LogError(nsce.Message);
                return NotFound();
            }
            
        }

        [HttpPut("{userId}/bookings/{bookingId}/refund")]
        public async Task<IActionResult> RequestRefund(int userId, int bookingId)
        {
            var success = await _bookingService.RequestRefundAsync(bookingId);
            if (success)
            {
                return Ok("Refund requested successfully.");
            }
            return NotFound();
        }

        [Route("UpdateUser")]
        [Authorize(Roles ="customer")]
        [HttpPut]
        public async Task<ActionResult<Customer>> UpdateCustomer(UpdateCustomerDTO customerDTO)
        {
            try
            {
                var customer = await _customerService.UpdateCustomer(customerDTO);
                return customer;
            }
            catch (NoSuchCustomerException nsce)
            {
                _logger.LogInformation(nsce.Message);
                return NotFound(nsce.Message);
            }
        }

        [Route("CancelBookingByPassenger")]

        [Authorize(Roles = "customer")]
        [HttpDelete]
        public async Task<ActionResult<PassengerBooking>> CancelBookingByPassenger(int passengerId)
        {
            try{
                var passengerBooking = await _bookingService.CancelBookingByPassenger(passengerId);
                return passengerBooking;
            }
            catch(NoSuchBookingsException nsbe)
            {
                _logger.LogError(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }
        [Route("GetAllCancelledBookings")]
        [HttpGet]
        public async Task<ActionResult<List<CancelledBooking>>> GetAllCancelledBookings()
        {
            try
            {
                var cancelledBookings = await _bookingService.GetAllCancelledBookings();
                return cancelledBookings;
            }
            catch (NoSuchBookingsException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
