using Simplifly.Models.DTO_s;

namespace Simplifly.Interfaces
{
    public interface IFlightCustomerService
    {
        public Task<List<SearchedFlightResultDTO>> SearchFlights(SearchFlightDTO searchFlight);
    }
}
