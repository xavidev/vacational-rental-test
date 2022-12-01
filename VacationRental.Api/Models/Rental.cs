namespace VacationRental.Api.Models
{
    public class Rental
    {
        private readonly int id;

        public Rental(int id, int units)
        {
            this.id = id;
            Units = units;
        }

        public int Units { get; }
    }
}