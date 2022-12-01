using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class RentalCatalog
    {
        private static readonly IDictionary<int, Rental> _rentals = new Dictionary<int, Rental>();

        public int CreateRental(int units)
        {
            var id = _rentals.Keys.Count + 1;
            _rentals.Add(id, new Rental(id, units));

            return id;
        }

        public  bool HaveRental(int rentalId)
        {
            return _rentals.ContainsKey(rentalId);
        }

        public Rental Get(int rentalId)
        {
            return _rentals[rentalId];
        }
    }
}