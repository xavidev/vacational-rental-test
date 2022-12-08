using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace VacationRental.Api.Models
{
    public class Rental
    {
        private readonly int id;
        private readonly int preparationDays;
        private readonly List<RentalUnit> rentalUnits;
        public int Units { get; private set; }
        public int Id => this.id;

        public static Rental Create(int id, int units, int preparationTime)
        {
            return new Rental(id, units, preparationTime);
        }

        private Rental(int id, int units, int preparationDays)
        {
            this.id = id;
            this.preparationDays = preparationDays;
            Units = units;
            this.rentalUnits = new List<RentalUnit>();

            for (int i = 0; i < units; i++)
            {
                this.AddUnit(i+1);
            }
        }

        public Booking CreateBooking(DateTime from, int nights)
        {
            return new Booking(this.id, from, nights, this.preparationDays);
        }

        public void Assign(Booking booking)
        {
            foreach (var unit in rentalUnits)
            {
                if (unit.TryBook(booking)) return;
            }
        }

        public void Update(int units, int preparationDays)
        {
            if (this.OnlyAugmentUnits(units, preparationDays))
            {
                this.Units = units;
            }

            var bookings = new List<Booking>();
            foreach (var unit in this.rentalUnits)
            {
                bookings.AddRange(unit.Release());
            }
            
            this.rentalUnits.Clear();
            
            for (int i = 0; i < units; i++)
            {
                this.AddUnit(i+1);
            }

            foreach (var booking in bookings)
            {
                booking.SetPreparationDays(preparationDays);
                
                foreach (var unit in this.rentalUnits)
                {
                    unit.TryBook(booking);
                    if (booking.IsReserved()) break;
                }

                if (!booking.IsReserved())
                {
                    throw new InvalidOperationException($"Can not update rental:{this.id}");
                }
            }
        }

        private bool OnlyAugmentUnits(int units, int preparationDays)
        {
            return units >= this.Units && preparationDays == this.preparationDays;
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
            
            bookingRequest.SetReserved(this.unit);
            bookings.Add(bookingRequest);
            
            return true;
        }

        public IEnumerable<Booking> Release()
        {
            foreach (var booking in bookings)
            {
                booking.Release();
                yield return booking;
            }
        }
    }
}