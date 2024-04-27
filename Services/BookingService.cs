using Microsoft.AspNetCore.Mvc;
using Simplifly.Controllers;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Models.DTO_s;
using Simplifly.Repositories;

namespace Simplifly.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepository<string, Flight> _flightRepository;
        private readonly IRepository<string, SeatDetail> _seatRepository;
        private readonly IRepository<int, PassengerBooking> _passengerBookingRepository;
        private readonly ISeatDeatilRepository _seatDetailRepository;
        private readonly IRepository<int, CancelledBooking> _cancelBookingRepository;
        private readonly IRepository<int, Schedule> _scheduleRepository;
        private readonly IPassengerBookingRepository _passengerBookingsRepository;
        private readonly IBookingRepository _bookingsRepository;

        private readonly IRepository<int, Payment> _paymentRepository;

        private readonly ILogger<BookingService> _logger;

        /// <summary>
        /// Constructor to initialize objects
        /// </summary>
        /// <param name="bookingRepository"></param>
        /// <param name="scheduleRepository"></param>
        /// <param name="passengerBookingRepository"></param>
        /// <param name="flightRepository"></param>
        /// <param name="bookingsRepository"></param>
        /// <param name="seatDetailRepository"></param>
        /// <param name="passengerBookingRepository1"></param>
        /// <param name="paymentRepository"></param>
        /// <param name="logger"></param>
        public BookingService(IRepository<int, CancelledBooking> cancelBookingRepository,IRepository<string,SeatDetail> seatRepository,IRepository<int, Booking> bookingRepository, IRepository<int, Schedule> scheduleRepository, IRepository<int, PassengerBooking> passengerBookingRepository, IRepository<string, Flight> flightRepository, IBookingRepository bookingsRepository, ISeatDeatilRepository seatDetailRepository, IPassengerBookingRepository passengerBookingRepository1, IRepository<int, Payment> paymentRepository, ILogger<BookingService> logger)
        {
            _flightRepository = flightRepository;
            _bookingsRepository = bookingsRepository;
            _passengerBookingsRepository = passengerBookingRepository1;
            _bookingRepository = bookingRepository;
            _passengerBookingRepository = passengerBookingRepository;
            _seatDetailRepository = seatDetailRepository;
            _scheduleRepository = scheduleRepository;
            _seatRepository = seatRepository;
            _cancelBookingRepository = cancelBookingRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }
        #region CreateBooking
        public async Task<bool> CreateBookingAsync(BookingRequestDto bookingRequest)
        {
            if (bookingRequest == null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            List<SeatDetail>? seatsList = await _seatRepository.GetAsync();
            var _schedule = await _scheduleRepository.GetAsync(bookingRequest.ScheduleId);

            var flightNo = _schedule.FlightId;

            var isSeatsAvailable = await _passengerBookingsRepository.CheckSeatsAvailbilityAsync(bookingRequest.ScheduleId, bookingRequest.SelectedSeats);
            //If selected seats are not available 
            if (!isSeatsAvailable)
            {
                return false;
            }

            int count = 0;

            if (seatsList != null)
            {
                foreach (var seat in bookingRequest.SelectedSeats)
                {
                    foreach (var item in seatsList)
                    {
                        if (item.FlightNumber == flightNo && item.SeatNumber == seat && item.isBooked == 1)
                        {
                            throw new Exception("Invalid seat selection, please try selecting correctly.");
                        }
                        else if (item.FlightNumber == flightNo && item.SeatNumber == seat && item.isBooked == 0)
                        {
                            count++;
                        }
                    }
                }
            }

            if (count != bookingRequest.SelectedSeats.Count)
            {
                throw new Exception("Invalid seat selection, please try selecting correctly.");
            }


            var schedudle = await _scheduleRepository.GetAsync(bookingRequest.ScheduleId);
            if (schedudle == null)
            {
                throw new NoSuchScheduleException();
            }

            //Calculate total prices based on booked tickets
            var totalPrice = CalculateTotalPrice(bookingRequest.SelectedSeats.Count, await _flightRepository.GetAsync(schedudle.FlightId));
            //Getting the seatClass type 
            var seatClass = bookingRequest.SelectedSeats[0][0];

            //Creating payment
            var payment = new Payment
            {
                Amount = bookingRequest.Price,
                PaymentDate = DateTime.Now,
                Status = PaymentStatus.Successful,
                PaymentDetails = bookingRequest.PaymentDetails,
            };
            var addedPayment = await _paymentRepository.Add(payment);

            //Creating booking
            var booking = new Booking
            {
                ScheduleId = bookingRequest.ScheduleId,
                UserId = bookingRequest.UserId,
                BookingTime = DateTime.Now,
                TotalPrice = bookingRequest.Price,
                bookingStatus = Booking.BookingStatus.Successful,

                PaymentId = addedPayment.PaymentId,
            };


            var addedBooking = await _bookingRepository.Add(booking);

            //fetching the seats only for given seatNos
            var seatDetails = await _seatDetailRepository.GetSeatDetailsAsync(bookingRequest.SelectedSeats);

            if (seatDetails == null || seatDetails.Count() != bookingRequest.SelectedSeats.Count())
            {
                throw new Exception("Invalid seat selection, please try selecting correctly.");
            }



            //creating PassengerBooking, assigning seatNumbers
            int index = 0;
            foreach (var passengerId in bookingRequest.PassengerIds)
            {
                //get seatDetail for current index
                var seatDetail = seatDetails.ElementAtOrDefault(index);
                if (seatDetail != null)
                {
                    var passengerBooking = new PassengerBooking
                    {
                        BookingId = addedBooking.Id,
                        PassengerId = passengerId,
                        SeatNumber = seatDetail.SeatNumber // Assign a unique seat to each passenger
                    };

                    if (seatsList != null)
                    {
                        foreach (var item in seatsList)
                        {
                            if (item.FlightNumber == flightNo && item.SeatNumber == seatDetail.SeatNumber)
                            {
                                item.isBooked = 1;
                            }
                        }
                    }

                    await _passengerBookingRepository.Add(passengerBooking);
                    // Move to the next seat for the next passenger
                    index++;
                }

                //when no of seats < no of passengers
                else
                {
                    throw new Exception("Not enough seats available for the passengers");
                }
            }

            return addedBooking != null && addedPayment != null;
        }
        #endregion
        #region Getallbooking

        /// <summary>
        /// Method to get all bookings
        /// </summary>
        /// <returns>Collection of Booking</returns>
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAsync();
        }

        /// <summary>
        /// Method to get booking by Id
        /// </summary>
        /// <param name="bookingId">Id in int</param>
        /// <returns>Object of booking</returns>
        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepository.GetAsync(bookingId);
        }
        #endregion
        #region cancelbooking

        public async Task<Booking> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);

            if (booking.ScheduleId == -1)
            {
                throw new NoSuchBookingsException();
            }

            Booking? bookingsList = await _bookingRepository.GetAsync(bookingId);
            List<SeatDetail>? seatsList = await _seatRepository.GetAsync();
            Schedule? _schedule = await _scheduleRepository.GetAsync(bookingsList.ScheduleId);
            List<PassengerBooking>? passBookings = await _passengerBookingRepository.GetAsync();
            string? flightNo = "";

            if (_schedule != null)
            {
                flightNo = _schedule.FlightId;
            }

            CancelledBooking _cancelBooking = new CancelledBooking();
            _cancelBooking.BookingId = bookingId;
            _cancelBooking.Booking = bookingsList;
            _cancelBooking.RefundStatus = "Refund Requested";
            await _cancelBookingRepository.Add(_cancelBooking);




            if (passBookings != null && flightNo != null)
            {
                foreach (var item in passBookings)
                {
                    if (seatsList != null)
                    {
                        foreach (var seat in seatsList)
                        {
                            if (seat.FlightNumber == flightNo && seat.SeatNumber == item.SeatNumber)
                            {
                                seat.isBooked = 0;
                            }
                        }
                    }
                }
            }

            // Delete passenger booking
            var passengerBookings = await _passengerBookingsRepository.GetPassengerBookingsAsync(bookingId);
            foreach (var passengerBooking in passengerBookings)
            {
                await _passengerBookingRepository.Delete(passengerBooking.Id);
            }

            // Delete payment
            await _paymentRepository.Delete(booking.PaymentId);


            // Delete booking(paymentId is FK, so auto delete happening)
            // return await _bookingRepository.Delete(booking.BookingId);

            return booking;
        }
        #endregion
        #region cancelBookingByPassenger

        public async Task<PassengerBooking> CancelBookingByPassenger(int passengerId)
        {
            var passengerBookings = await _passengerBookingRepository.GetAsync();
            PassengerBooking? passengerBooking = passengerBookings?.FirstOrDefault(pb => pb.PassengerId == passengerId);

            if (passengerBooking != null)
            {
                List<SeatDetail>? seatsList = await _seatRepository.GetAsync();
                PassengerBooking? passBookings = await _passengerBookingRepository.GetAsync(passengerBooking.Id);
                List<Booking>? _bookingsList = await _bookingRepository.GetAsync();

                Booking? booking = _bookingsList.FirstOrDefault(b => b.Id == passBookings.BookingId);
                Schedule? _schedule = await _scheduleRepository.GetAsync(booking.ScheduleId);

                //adding to cancelBooking
                CancelledBooking _cancelBooking = new CancelledBooking();
                _cancelBooking.BookingId = (int)passengerBooking.BookingId;
                _cancelBooking.Booking = booking;
                _cancelBooking.RefundStatus = "Pending";
                await _cancelBookingRepository.Add(_cancelBooking);

                //updating booking status
                booking.bookingStatus = Booking.BookingStatus.Cancelled;
                await _bookingRepository.Update(booking);

                //refund request
                RequestRefundAsync(booking.Id);


                var flightNo = _schedule.FlightId;

                if (passBookings != null)
                {
                    if (seatsList != null)
                    {
                        foreach (var seat in seatsList)
                        {
                            if (seat.FlightNumber == flightNo && seat.SeatNumber == passBookings.SeatNumber)
                            {
                                seat.isBooked = 0;
                            }
                        }
                    }
                }

                passengerBooking = await _passengerBookingRepository.Delete(passengerBooking.Id);
                return passengerBooking;
            }

            throw new NoSuchPassengerException();
        }
        #endregion


        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
        {
            var bookings= await _bookingsRepository.GetBookingsByUserIdAsync(userId);
            if (bookings != null)
            {
                bookings.Where(e => e.Schedule.Departure < DateTime.Now);
            }
            return await _bookingsRepository.GetBookingsByUserIdAsync(userId);
        }

      
        public double CalculateTotalPrice(int numberOfSeats, Flight flight)
        {
            double totalPrice = numberOfSeats * (flight?.BasePrice ?? 0); 
            return totalPrice;

        }

        
        public async Task<bool> RequestRefundAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            var cancelBookings = await _cancelBookingRepository.GetAsync();
            var cancelBooking = cancelBookings.FirstOrDefault(cb => cb.BookingId == bookingId);

            if (booking == null)
            {
                throw new NoSuchBookingsException();
            }

            // Check if payment exists
            var payment = await _paymentRepository.GetAsync(booking.PaymentId);

            if (payment == null)
            {
                throw new Exception("Payment not found for the booking.");
            }

            // Check payment status
            if (payment.Status != PaymentStatus.Successful)
            {
                throw new Exception("Refund cannot be requested for unsuccessful payments.");
            }

            // Updating paymentStatus to "RefundRequested" for refund request

            payment.Status = PaymentStatus.RefundRequested;
            cancelBooking.RefundStatus = "RefundRequested";

            await _paymentRepository.Update(payment);
            await _cancelBookingRepository.Update(cancelBooking);



            // Update payment status to "Pending" for refund request
            
            await _paymentRepository.Update(payment);

            // Perform refund process here (e.g., communicate with payment gateway)

            return true;
        }

       
        public async Task<List<Booking>> GetBookingBySchedule(int scheduleId)
        {
            var bookings = await _bookingRepository.GetAsync();
            bookings = bookings.Where(e => e.ScheduleId == scheduleId).ToList();


            if (bookings != null)
            {
                return bookings;
            }
            throw new NoSuchBookingsException();
        }

       
        public async Task<List<Booking>> GetBookingByFlight(string flightNumber)
        {
            var bookings = await _bookingRepository.GetAsync();
            bookings = bookings.Where(e => e.Schedule.FlightId == flightNumber).ToList();
            if (bookings != null)
            {
                return bookings;
            }
            throw new NoSuchBookingsException();
        }

        
        public async Task<List<string>> GetBookedSeatBySchedule(int scheduleID)
        {
            var bookings=await _passengerBookingRepository.GetAsync();
            var bookedSeats= bookings.Where(e=>e.Booking.ScheduleId==scheduleID)
                .Select(e=>e.SeatNumber).ToList();
            if(bookedSeats != null)
            {
                return bookedSeats;
            }
            throw new NoSuchBookingsException();
        }

       
        public async Task<List<PassengerBooking>> GetBookingsByCustomerId(int customerId)
        {
            var bookings = await _passengerBookingRepository.GetAsync();
            bookings = bookings.Where(e => e.Booking.UserId == customerId).ToList();
            if(bookings!= null)
            {
                return bookings;
            }
            throw new NoSuchCustomerException();
        }

       
    }
}
