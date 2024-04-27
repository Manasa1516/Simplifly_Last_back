using Simplifly.Models;

namespace Simplifly.Interfaces
{
    public interface IPassengerService
    {
        Task<Passenger> AddPassenger(Passenger flightOwner);
        Task<bool> RemovePassenger(int id);
        Task<List<Passenger>> GetAllPassengers();
        Task<Passenger> GetByIdPassengers(int id);
    }
}
