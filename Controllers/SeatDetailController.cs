using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;
using System.Diagnostics.CodeAnalysis;

namespace Simplifly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SeatDetailController : ControllerBase
    {
        private readonly ISeatDetailService _seatDetailService;
        public SeatDetailController(ISeatDetailService seatDetailService)
        {
            _seatDetailService = seatDetailService;
        }


        [HttpGet]
        public Task<List<SeatDetail>> GetAllSeatDetail()
        {
            var seatDetails = _seatDetailService.GetAllSeatDetails();
            return seatDetails;
        }
        [HttpGet("ById")]
        public Task<SeatDetail> GetSeatDetailById(string id)
        {
            var seatDetails = _seatDetailService.GetByIdSeatDetails(id);
            return seatDetails;
        }
        [HttpPost]
        public async Task<SeatDetail> AddSeatDetail(SeatDetail seatDetail)
        {
            seatDetail = await _seatDetailService.AddSeatDetail(seatDetail);
            return seatDetail;
        }



    }
}
