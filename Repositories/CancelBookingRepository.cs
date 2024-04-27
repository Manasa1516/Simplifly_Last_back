﻿using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Context;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Microsoft.EntityFrameworkCore;

namespace Simplifly.Repositories
{
    public class CancelBookingRepository : IRepository<int, CancelledBooking>
    {
        private readonly RequestTrackerContext _context;
        private readonly ILogger<CancelBookingRepository> _logger;

        public CancelBookingRepository(RequestTrackerContext context, ILogger<CancelBookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<CancelledBooking> Add(CancelledBooking item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation("Airport added with AirportId " + item.Id);
            return item;
        }

        public Task<CancelledBooking> Delete(int cancelId)
        {
            var cancelBooking = GetAsync(cancelId);
            if (cancelBooking != null)
            {
                _context.Remove(cancelBooking);
                _context.SaveChanges();
                _logger.LogInformation($"Airport removed with id {cancelId}");
                return cancelBooking;
            }
            throw new NoSuchBookingsException();
        }

        public async Task<CancelledBooking> GetAsync(int cancelId)
        {
            var cancelBookings = await GetAsync();
            var cancelBooking = cancelBookings.FirstOrDefault(e => e.Id == cancelId);
            if (cancelBooking != null)
            {
                return cancelBooking;
            }
            throw new NoSuchBookingsException();
        }

        public async Task<List<CancelledBooking>> GetAsync()
        {
            var cancelBookings = _context.CancelledBookings.Include(pb => pb.Booking).Include(pb => pb.Booking.Schedule)
            .Include(pb => pb.Booking.Schedule.Route)
            .Include(e => e.Booking.Schedule.Flight).Include(e => e.Booking.Schedule.Route.SourceAirport)
            .Include(pb => pb.Booking.Schedule.Route.DestinationAirport)
            .ToList();
            return cancelBookings;
        }

        
        public async Task<CancelledBooking> Update(CancelledBooking item)
        {
            var cancelBooking = await GetAsync(item.Id);
            if (cancelBooking != null)
            {
                _context.Entry<CancelledBooking>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"CancelBooking updated with id {item.BookingId}");
                return cancelBooking;
            }
            throw new NoSuchBookingsException();
        }
    }
}