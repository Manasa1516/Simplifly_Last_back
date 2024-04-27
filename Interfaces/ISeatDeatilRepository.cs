using Simplifly.Models;

namespace Simplifly.Interfaces
{
    public interface ISeatDeatilRepository
    {
        Task<IEnumerable<SeatDetail>> GetSeatDetailsAsync(List<string> seatNos);
        Task UpdateSeatDetailsAsync(IEnumerable<SeatDetail> seatDetails);
    }
}
