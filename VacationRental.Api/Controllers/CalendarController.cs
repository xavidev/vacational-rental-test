using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Controllers.Models;
using VacationRental.Api.Models;
using VacationRental.Api.RentalBooking;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly BookingCalendarHandler calendarHandler;

        public CalendarController(BookingCalendarHandler calendarHandler)
        {
            this.calendarHandler = calendarHandler;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            var result = new CalendarViewModel 
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>() 
            };

            var calendar = this.calendarHandler.GetBookingCalendarFor(rentalId, start, nights);
            
            result.Dates.AddRange(calendar.Dates.Select( x => new CalendarDateViewModel()
            {
                Bookings = x.Bookings.Select(b => new CalendarBookingViewModel()
                {
                    Id = b.Id
                }).ToList(),
                Date = x.Date
            }).ToList());

            return result;
        }
    }
}
