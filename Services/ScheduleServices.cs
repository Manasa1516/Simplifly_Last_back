using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;
using Simplifly.Repositories;

namespace Simplifly.Services
{
    public class ScheduleServices : IScheduleFlightOwnerService, IFlightCustomerService
    {
        IRepository<int, Schedule> _scheduleRepository;
        IBookingService _bookingService;
        ILogger<ScheduleServices> _logger;


        /// <summary>
        /// Constructor to initialize the objects
        /// </summary>
        /// <param name="scheduleRepository"></param>
        /// <param name="logger"></param>
        public ScheduleServices(IRepository<int, Schedule> scheduleRepository, ILogger<ScheduleServices> logger)
        {
            _scheduleRepository = scheduleRepository;
            _logger = logger;

        }
        public ScheduleServices(IRepository<int, Schedule> scheduleRepository, IBookingService bookingService, ILogger<ScheduleServices> logger)
        {
            _scheduleRepository = scheduleRepository;
            _bookingService = bookingService;
            _logger = logger;

        }

        /// <summary>
        ///Service class method to add schedule 
        /// </summary>
        /// <param name="schedule">Object of schedule</param>
        /// <returns>Schedule object</returns>
        /// <exception cref="FlightScheduleBusyException">Throw when schedule is already present</exception>
        public async Task<Schedule> AddSchedule(Schedule schedule)
        {

            var existingSchedules = await _scheduleRepository.GetAsync();
            bool isOverlap = existingSchedules.Any(e =>
    e.FlightId == schedule.FlightId &&
    ((schedule.Departure >= e.Departure && schedule.Departure <= e.Arrival) ||
    (schedule.Arrival >= e.Departure && schedule.Arrival <= e.Arrival) ||
    (e.Departure >= schedule.Departure && e.Arrival <= schedule.Arrival)));

            if (!isOverlap)
            {
                // If no overlap then only adding add the new schedule
                schedule = await _scheduleRepository.Add(schedule);
                return schedule;
            }
            throw new FlightScheduleBusyException();
        }

        /// <summary>
        /// Service class method to get all schedule
        /// </summary>
        /// <returns>List of schedules</returns>
        public async Task<List<Schedule>> GetAllSchedules()
        {
            var schedules = await _scheduleRepository.GetAsync();
            return schedules;
        }

        /// <summary>
        /// Service class method to remove schedule
        /// </summary>
        /// <param name="schedule">Schedule object</param>
        /// <returns>Object of schedule</returns>
        /// <exception cref="NoSuchScheduleException">throw when schedule is not present</exception>
        public async Task<Schedule> RemoveSchedule(Schedule schedule)
        {
            var schedules = await _scheduleRepository.GetAsync(schedule.Id);
            if (schedules != null)
            {
                schedules = await _scheduleRepository.Delete(schedules.Id);
                return schedules;
            }
            throw new NoSuchScheduleException();
        }


        /// <summary>
        /// Service class method to update flight schedule
        /// </summary>
        /// <param name="scheduleId">scheduleId in int</param>
        /// <param name="flightNumber">flightNumber in string</param>
        /// <returns>Object of schedule</returns>
        /// <exception cref="NoSuchScheduleException">throw when no schedule is found</exception>
        public async Task<Schedule> UpdateScheduledFlight(int scheduleId, string flightNumber)
        {
            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null)
            {
                schedule.FlightId = flightNumber;

                schedule = await _scheduleRepository.Update(schedule);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        /// <summary>
        /// Service class method to update scheduled route
        /// </summary>
        /// <param name="scheduleId">scheduleId in int</param>
        /// <param name="routeId">routeId in string</param>
        /// <returns>Object of schedule</returns>
        /// <exception cref="NoSuchScheduleException">throw when no schedule is found</exception>
        public async Task<Schedule> UpdateScheduledRoute(int scheduleId, int routeId)
        {
            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null)
            {
                schedule.RouteId = routeId;
                schedule = await _scheduleRepository.Update(schedule);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        /// <summary>
        /// Service class method to update scheduled route
        /// </summary>
        /// <param name="scheduleId">scheduleId in int</param>
        /// <param name="departure">departure in Datetime</param>
        /// <param name="arrival">arrival in Datetime</param>
        /// <returns>Object of schedule</returns>
        /// <exception cref="NoSuchScheduleException">throw when no schedule is found</exception>
        public async Task<Schedule> UpdateScheduledTime(int scheduleId, DateTime departure, DateTime arrival)
        {
            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null)
            {
                schedule.Departure = departure;
                schedule.Arrival = arrival;
                schedule = await _scheduleRepository.Update(schedule);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        /// <summary>
        /// Method to search flights based on origin, destination and date of travel.
        /// </summary>
        /// <param name="searchFlight">Object of SearchFlightDTO</param>
        /// <returns>List of flight based on user search</returns>
        /// <exception cref="NoFlightAvailableException">Throw when no flight available for particular search.</exception>
        public async Task<List<SearchedFlightResultDTO>> SearchFlights(SearchFlightDTO searchFlight)
        {            
            List<SearchedFlightResultDTO> searchResult = new List<SearchedFlightResultDTO>();
            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.Departure.Date == searchFlight.DateOfJourney.Date
             && e.Route?.SourceAirport?.City == searchFlight.Origin
             && e.Route.DestinationAirport?.City == searchFlight.Destination
             && (AvailableSeats(e.Flight.TotalSeats,e.Id)>0)).ToList();

            searchResult = schedules.Select(e => new SearchedFlightResultDTO
            {
                FlightNumber = e.FlightId,
                Airline = e.Flight.Airline,
                ScheduleId = e.Id,
                SourceAirport = e.Route.SourceAirport.City,
                DestinationAirport = e.Route.DestinationAirport.City,
                DepartureTime = e.Departure,
                ArrivalTime = e.Arrival,
                TotalPrice = CalculateTotalPrice(searchFlight, e.Flight.BasePrice)

            }).ToList();
            if (searchResult != null)
                return searchResult;
            else
                throw new NoFlightAvailableException();
        }

        /// <summary>
        /// Method to calculate price of booking
        /// </summary>
        /// <param name="searchFlightDto">Object of SearchFlightDTO</param>
        /// <param name="basePrice">basePrice in double</param>
        /// <returns>Total price in double</returns>
        public double CalculateTotalPrice(SearchFlightDTO searchFlightDto, double basePrice)
        {
            double totalPrice = 0;
            double seatPrice = 0;
            double adultSeatCost = 0;
            double childSeatCost = 0;
            if (searchFlightDto.SeatClass == "economy")
                seatPrice = basePrice*0.2;
            else if (searchFlightDto.SeatClass == "premiumEconomy")
                seatPrice = basePrice*0.3;
            else
                seatPrice = basePrice * 0.4;

            adultSeatCost = basePrice + seatPrice + (basePrice*0.3);
            childSeatCost = basePrice + seatPrice + (basePrice * 0.2);
            totalPrice=(adultSeatCost*searchFlightDto.Adult)+(childSeatCost*searchFlightDto.Child);

            return totalPrice;
        }

        /// <summary>
        /// Method to get available seats
        /// </summary>
        /// <param name="totalSeats">total seats in int</param>
        /// <param name="schedule">schedultId in int</param>
        /// <returns></returns>
        public int AvailableSeats(int totalSeats, int schedule)
        {
            var bookedSeatsTask = _bookingService.GetBookedSeatBySchedule(schedule);
            bookedSeatsTask.Wait(); 
            var bookedSeats = bookedSeatsTask.Result;

            int availableSeats = totalSeats - bookedSeats.Count();
            return availableSeats;
        }

        /// <summary>
        /// Service class method to get the schedule of particular flight
        /// </summary>
        /// <param name="flightNumber">Flight Number in string</param>
        /// <returns>Object of flight Schedule</returns>
        /// <exception cref="NoSuchScheduleException">If no schedule found</exception>
        public async Task<List<FlightScheduleDTO>> GetFlightSchedules(string flightNumber)
        {
            List<FlightScheduleDTO> flightSchedule = new List<FlightScheduleDTO>();

            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.FlightId == flightNumber).ToList();

            flightSchedule = schedules.Select(e => new FlightScheduleDTO
            {
                FlightNumber = e.FlightId,

                SourceAirport = e.Route?.SourceAirport?.Name + " ," + e.Route?.SourceAirport?.City,
                DestinationAirport = e.Route?.DestinationAirport?.Name + " ," + e.Route?.DestinationAirport?.City,
                Departure = e.Departure,
                Arrival = e.Arrival
            }).ToList();

            if (flightSchedule != null)
                return flightSchedule;
            else
                throw new NoSuchScheduleException();
        }

        /// <summary>
        /// Method to remove schedule
        /// </summary>
        /// <param name="flightNumber">flightNumber in string</param>
        /// <returns>number of schedule removed in int</returns>
        public async Task<int> RemoveSchedule(string flightNumber)
        {
            int removedScheduleCount = 0;
            var schedules=await _scheduleRepository.GetAsync();
            schedules=schedules.Where(e=>e.FlightId==flightNumber).ToList();
            foreach(var flight in schedules)
            {
                await _scheduleRepository.Delete(flight.Id);
                removedScheduleCount++;
            }
            return removedScheduleCount;
        }

        /// <summary>
        /// Method to remove schedule
        /// </summary>
        /// <param name="departureDate">departureDate in DateTime</param>
        /// <param name="airportId">airportId in int</param>
        /// <returns>number of schedule removed</returns>
        public async Task<int> RemoveSchedule(DateTime departureDate, int airportId)
        {
            int removedScheduleCount = 0;
            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.Departure.Date==departureDate.Date && 
            e.Route.SourceAirportId==airportId).ToList();
            foreach (var flight in schedules)
            {
                await _scheduleRepository.Delete(flight.Id);
                removedScheduleCount++;
            }
            return removedScheduleCount;
        }

    }
}