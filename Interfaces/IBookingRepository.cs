using Simplifly.Models;

namespace Simplifly.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
       // Task<bool> CheckSeatsAvailabilityAsync(string flightId, List<int> seatIds);
    }
}