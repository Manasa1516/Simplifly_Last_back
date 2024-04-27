using Simplifly.Models;
using Simplifly.Models.DTO_s;

namespace Simplifly.Interfaces
{
    public interface IScheduleFlightOwnerService
    {
        public Task<Schedule> AddSchedule(Schedule schedule);
        public Task<Schedule> RemoveSchedule(Schedule schedule);
        public Task<int> RemoveSchedule(string flightNumber);
        public Task<int> RemoveSchedule(DateTime departureDate,int airportId);
        public Task<List<Schedule>> GetAllSchedules();
        public Task<List<FlightScheduleDTO>> GetFlightSchedules(string flightNumber);
        public Task<Schedule> UpdateScheduledFlight(int scheduleId, string flightNumber);

        public Task<Schedule> UpdateScheduledRoute(int scheduleId, int routeId);
        public Task<Schedule> UpdateScheduledTime(int scheduleId, DateTime departure, DateTime arrival);
    }
}