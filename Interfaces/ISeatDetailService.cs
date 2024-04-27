using Simplifly.Models;

namespace Simplifly.Interfaces
{
    public interface ISeatDetailService
    {
        Task<SeatDetail> AddSeatDetail(SeatDetail flightOwner);
        Task<bool> RemoveSeatDetail(string id);
        Task<List<SeatDetail>> GetAllSeatDetails();
        Task<SeatDetail> GetByIdSeatDetails(string id);
    }
}
