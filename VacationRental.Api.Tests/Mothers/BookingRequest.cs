using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VacationRental.Api.Controllers.Models;

namespace VacationRental.Api.Tests.Mothers
{
    public class BookingRequest
    {
        private readonly int rentalId;
        private DateTime @from;
        private int nights;

        public int RentalId => this.rentalId;
        public DateTime RentalFrom => this.@from;
        public int RentalNights => this.nights;

        private BookingRequest(int rentalId)
        {
            this.rentalId = rentalId;
        }

        public static BookingRequest For(int rentalId)
        {
            return new BookingRequest(rentalId);
        }

        public BookingRequest From(DateTime from)
        {
            this.from = from;
            
            return this;
        }

        public BookingRequest Nights(int nights)
        {
            this.nights = nights;

            return this;
        }

        public static implicit operator BookingBindingModel(BookingRequest request) => new BookingBindingModel
        {
            RentalId = request.rentalId,
            Nights = request.nights,
            Start = request.@from
        };
    }
}