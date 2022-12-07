using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Controllers.Models;
using VacationRental.Api.Models;
using VacationRental.Api.RentalBooking;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly BookingHandler handler;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingsController(
            BookingHandler handler,
            IDictionary<int, BookingViewModel> bookings)
        {
            this.handler = handler;
            _bookings = bookings;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            BookingInfo bookingInfo = this.handler.GetBooking(bookingId);

            return new BookingViewModel()
            {
                Id = bookingId,
                Nights = bookingInfo.Nights,
                Start = bookingInfo.From,
                RentalId = bookingInfo.RentalId
            };
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            var result  = this.handler.Book(model.RentalId, model.Start, model.Nights);
            if (!result.Success)
            {
                throw new ApplicationException("Not available");
            }

            return new ResourceIdViewModel() {Id = result.Id};
        }
    }
}
