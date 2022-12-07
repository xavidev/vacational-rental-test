using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VacationRental.Api.RentalBooking;

namespace VacationRental.Api.Models
{
    public class Rental
    {
        private readonly int id;
        private readonly List<RentalUnit> rentalUnits;
        public int Units { get; }

        public static Rental Create(int id, int units)
        {
            return new Rental(id, units);
        }

        private Rental(int id, int units)
        {
            this.id = id;
            Units = units;
            this.rentalUnits = new List<RentalUnit>();

            for (int i = 0; i < units; i++)
            {
                this.AddUnit();
            }
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

        private void AddUnit()
        {
            this.rentalUnits.Add(new RentalUnit());
        }
    }

    internal class RentalUnit
    {
        private readonly RentalAvailability availability;

        public RentalUnit()
        {
            this.availability = new RentalAvailability();
        }

        public bool TryBook(Booking request)
        {
            return this.availability.TryBook(request);
        }
    }
}