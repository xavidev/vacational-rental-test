using System;
using System.Collections.Generic;

namespace VacationRental.Api.Controllers.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
    }
}
