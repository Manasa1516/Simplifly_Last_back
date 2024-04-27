using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Repositories;
using System.Numerics;

namespace Simplifly.Services
{
    public class SeatDetailService : ISeatDetailService
    {

        private readonly IRepository<string, SeatDetail> _seatdetailRepository;
        private readonly ILogger<SeatDetailService> _logger;

        /// <summary>
        /// Constructor for SeatDetailService
        /// </summary>
        /// <param name="seatdetailRepository"></param>
        /// <param name="logger"></param>
        public SeatDetailService(IRepository<string, SeatDetail> seatdetailRepository, ILogger<SeatDetailService> logger)
        {
            _seatdetailRepository = seatdetailRepository;
            _logger = logger;

        }

        public async Task<SeatDetail> AddSeatDetail(SeatDetail seatDetail)
        {
            return await _seatdetailRepository.Add(seatDetail);
        }

        public async Task<bool> RemoveSeatDetail(string id)

        {

            var owner = await _seatdetailRepository.GetAsync(id);
            if (owner != null)
            {
                await _seatdetailRepository.Delete(id);
                return true;
            }
            return false;
        }

        public async Task<List<SeatDetail>> GetAllSeatDetails()
        {
            return await _seatdetailRepository.GetAsync();
        }



        public async Task<SeatDetail> GetByIdSeatDetails(string id)
        {
            return await (_seatdetailRepository.GetAsync(id));
        }
    }
}
