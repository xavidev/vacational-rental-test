using System;
using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class RentalCatalog
    {
        private static readonly IDictionary<int, Rental> _rentals = new Dictionary<int, Rental>();

        public int CreateRental(int units)
        {
            var id = _rentals.Keys.Count + 1;
            _rentals.Add(id, Rental.Create(id, units));

            return id;
        }
        
        public Rental Get(int rentalId)
        {
            if(!HaveRental(rentalId)) throw new ApplicationException("Rental not found");
 
            return _rentals[rentalId];
        }
        
        public  bool HaveRental(int rentalId)
        {
            return _rentals.ContainsKey(rentalId);
        }
    }
}