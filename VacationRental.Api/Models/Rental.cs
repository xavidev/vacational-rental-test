using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace VacationRental.Api.Models
{
    public class Rental
    {
        private readonly int id;
        private readonly int preparationTime;
        private readonly List<RentalUnit> rentalUnits;
        public int Units { get; }
        public int Id => this.id;

        public static Rental Create(int id, int units, int preparationTime)
        {
            return new Rental(id, units, preparationTime);
        }

        private Rental(int id, int units, int preparationTime)
        {
            this.id = id;
            this.preparationTime = preparationTime;
            Units = units;
            this.rentalUnits = new List<RentalUnit>();

            for (int i = 0; i < units; i++)
            {
                this.AddUnit(i+1);
            }
        }

        public Booking CreateBooking(DateTime from, int nights)
        {
            return new Booking(this.id, from, nights, this.preparationTime);
        }

        public void Assign(Booking booking)
        {
            foreach (var unit in rentalUnits)
            {
                if (unit.TryBook(booking))
                {
                    booking.SetReserved();
                    return;
                }
            }
        }

        private void AddUnit(int unit)
        {
            this.rentalUnits.Add(new RentalUnit(unit));
        }
    }

    internal class RentalUnit
    {
        private readonly int unit;
        private readonly List<Booking> bookings;

        public RentalUnit(int unit)
        {
            this.unit = unit;
            this.bookings = new List<Booking>();
        }
        
        public bool TryBook(Booking bookingRequest)
        {
            foreach (var booking in bookings)
            {
                if (booking.Overlap(bookingRequest)) return false;
            }
            
            bookings.Add(bookingRequest);
            
            return true;
        }
    }
}