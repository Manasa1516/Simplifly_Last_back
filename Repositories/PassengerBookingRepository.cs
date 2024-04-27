using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Simplifly.Context;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PassengerBookingRepository : IRepository<int,PassengerBooking>,IPassengerBookingRepository
    {
        private readonly RequestTrackerContext _context;
        private readonly ILogger<PassengerBookingRepository> _logger;
       
        public PassengerBookingRepository(RequestTrackerContext context, ILogger<PassengerBookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        public async Task<Models.PassengerBooking> Add(Models.PassengerBooking items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation($"PassengerBooking added with id {items.Id}");
            return items;
        }

        /// <summary>
        /// Method to delete PassengerBooking from database
        /// </summary>
        /// <param name="items">Object of PassengerBooking</param>
        /// <returns>PassengerBooking object</returns>
        /// <exception cref="NoSuchPassengerBookingException">throws exception if no PassengerBooking found</exception>
        public async Task<PassengerBooking> Delete(int passengerBookingId)
        {
            var passengerBooking = await GetAsync(passengerBookingId);
            if (passengerBooking != null)
            {
                _context.Remove(passengerBooking);
                _context.SaveChanges();

                _logger.LogInformation($"PassengerBooking delete with id {passengerBookingId}");
                return passengerBooking;
            }
            throw new NoSuchPassengerBookingException();
        }

        /// <summary>
        /// Method to get PassengerBooking data of specific Id
        /// </summary>
        /// <param name="key">key in int</param>
        /// <returns>PassengerBooking Object</returns>
        /// <exception cref="NoSuchPassengerBookingException">throws exception if no PassengerBooking found.</exception>
        public async Task<Models.PassengerBooking> GetAsync(int key)
        {
            var passengerBookings = await GetAsync();
            var passengerBooking = passengerBookings.FirstOrDefault(e => e.Id == key);
            if (passengerBooking != null)
            {
                return passengerBooking;
            }
            throw new NoSuchPassengerBookingException();
        }

        /// <summary>
        /// Method to get list of PassengerBooking
        /// </summary>
        /// <returns>PassengerBooking object</returns>
        public async Task<List<Models.PassengerBooking>> GetAsync()
        {
            var passengerBookings = _context.PassengerBookings.Include(e=>e.Booking).Include(e=>e.Booking.Schedule)
                .Include(e=>e.Passenger).Include(e=>e.SeatDetail).Include(e=>e.Booking.Schedule.Route)
                .Include(e=>e.Booking.Schedule.Flight).Include(e=>e.Booking.Schedule.Route.SourceAirport)
                .Include(e=>e.Booking.Schedule.Route.DestinationAirport)
                .ToList();
            return passengerBookings;
        }

        /// <summary>
        /// Method to add booking for passenger
        /// </summary>
        /// <param name="passengerBooking">Object of PassengerBooking</param>
        /// <returns></returns>
        public async Task AddPassengerBookingAsync(PassengerBooking passengerBooking)
        {
            _context.PassengerBookings.Add(passengerBooking);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Method to get passenger booking by bookingId
        /// </summary>
        /// <param name="bookingId">bookingId in int</param>
        /// <returns>IEnumerable of passenger booking</returns>
        public async Task<IEnumerable<PassengerBooking>> GetPassengerBookingsAsync(int bookingId)
        {
            return await _context.PassengerBookings
                .Where(pb => pb.BookingId == bookingId)
                .ToListAsync();
        }

        /// <summary>
        /// method to remove passenger booking by passengerId
        /// </summary>
        /// <param name="passengerBookingId">passengerId in int</param>
        /// <returns></returns>
        public async Task RemovePassengerBookingAsync(int passengerBookingId)
        {
            var passengerBooking = await _context.PassengerBookings.FindAsync(passengerBookingId);
            if (passengerBooking != null)
            {
                _context.PassengerBookings.Remove(passengerBooking);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Method to update PassengerBooking.
        /// </summary>
        /// <param name="items">Object of PassengerBooking</param>
        /// <returns>Airport Object</returns>
        /// <exception cref="NoSuchPassengerBookingException">throws exception if no PassengerBooking found</exception</exception>
        public async Task<Models.PassengerBooking> Update(Models.PassengerBooking items)
        {
            var passengerBooking = await GetAsync(items.Id);
            if (passengerBooking != null)
            {
                _context.Entry<Models.PassengerBooking>(passengerBooking).State = EntityState.Modified;
                _context.SaveChanges();
                return passengerBooking;
            }
            throw new NoSuchPassengerBookingException();
        }

        /// <summary>
        /// Method to check if seat is available or not
        /// </summary>
        /// <param name="scheduleId">scheduleId in int</param>
        /// <param name="seatNos">List of string</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> CheckSeatsAvailbilityAsync(int scheduleId, List<string> seatNos)
        {
            // Get all seats for the given seatNos List
            var bookedSeats = await _context.PassengerBookings
                .Where(pb => pb.Booking.ScheduleId == scheduleId && seatNos.Contains(pb.SeatNumber))
                .Select(pb => pb.SeatNumber)
                .ToListAsync();


            foreach (var seatNo in seatNos)
            {
                if (bookedSeats.Contains(seatNo))
                {
                    // Seat is already booked, return false
                    throw new Exception($"Seat {seatNo} is already booked.");

                }
            }


            // All seats are available
            return true;
        }
    }
}
