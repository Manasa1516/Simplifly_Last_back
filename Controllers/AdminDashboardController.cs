using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Simplifly.Models;
using Simplifly.Services;
using Simplifly.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Simplifly.Exceptions;
using Simplifly.Models.DTO_s;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Controllers
{
    [Route("api/admin/dashboard")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class AdminDashboardController : ControllerBase
    {
        private readonly ICustomerService _custService;
        private readonly IFlightOwnerService _flightOwnerService;
        private readonly IFlightFlightOwnerService _flightService;
        private readonly IBookingService _bookingService;
        private readonly IRouteFlightOwnerService _routeService;
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminDashboardController> _logger;  

        public AdminDashboardController(ICustomerService custService, IFlightOwnerService flightownerService, 
            IFlightFlightOwnerService flightService, IBookingService bookingService, IRouteFlightOwnerService routeService, 
            IAdminService adminService, ILogger<AdminDashboardController> logger)
        {
            _custService = custService;
            _flightOwnerService = flightownerService;
            _flightService = flightService;
            _bookingService = bookingService;
            _routeService = routeService;
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet("Bookings/Allbookings")]
        [Authorize(Roles = "admin, flightOwner")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }
        [HttpGet("Users/AllCustomers")]
        [Authorize(Roles = "admin, flightOwner")]
        public async Task<IActionResult> GetAsync()
        {
            var users = await _custService.GetAllCustomers();
            return Ok(users);
        }
        [HttpGet("Users/AllFlightOwners")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetFlightOwnersAsync()
        {
            var users = await _flightOwnerService.GetAllFlightOwners();
            return Ok(users);
        }

        [Route("GetAdminByUsername")]
        [HttpGet]
        public async Task<ActionResult<Admin>> GetAdminByUsername(string username)
        {
            try
            {
                var admin = await _adminService.GetAdminByUsername(username);
                return Ok(admin);
            }catch(NoSuchAdminException nsae)
            {
                _logger.LogInformation(nsae.Message);
                return NotFound(nsae.Message);
            }
        }

        // DELETE: api/admin/dashboard/users/{userId}
        [HttpDelete("customers/{userId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteCustomer(int userId)
        {
            var result = await _custService.RemoveCustomer(userId);
            if (result)
            {
                return Ok("User deleted successfully.");
            }
            return NotFound("User not found.");
        }

        // DELETE: api/admin/dashboard/flightowners/{flightOwnerId}
        [HttpDelete("flightowners/{flightOwnerId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteFlightOwner(int flightOwnerId)
        {
            var result = await _flightOwnerService.RemoveFlightOwner(flightOwnerId);
            if (result)
            {
                return Ok("User deleted successfully.");
            }
            return NotFound("User not found.");


        }

        // DELETE: api/admin/dashboard/bookings/{bookingId}
        [HttpDelete("bookings/{bookingId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> CancelBooking( int bookingId)
        {
            var success = await _bookingService.CancelBookingAsync(bookingId);
            if (success != null)
            {
                return NoContent();
            }
            return NotFound();
        }

        // DELETE: api/admin/dashboard/flightroutes/{flightRouteId}
        [HttpDelete("flightroutes/{flightRouteId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteFlightRoute(int flightRouteId)
        {
            var result = await _routeService.RemoveRouteById(flightRouteId);
            if (result)
            {
                return Ok("Flight route deleted successfully.");
            }
            return NotFound("Flight route not found.");
        }

        [Route("DeleteUserByUsername")]
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> DeleteUserByUsername(string username)
        {
            try
            {
                var user=await _adminService.DeleteUser(username);
                return Ok(user);
            }catch (NoSuchUserException nsue)
            {
                _logger.LogError(nsue.Message);
                return NotFound(nsue.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Admin>> UpdateAdmin(UpdateAdminDTO adminDTO)
        {
            try
            {
                var admin = await _adminService.UpdateAdmin(adminDTO);
                return admin;
            }
            catch (NoSuchAdminException nsae)
            {
                _logger.LogInformation(nsae.Message);
                return NotFound(nsae.Message);
            }
        }
    }
}
