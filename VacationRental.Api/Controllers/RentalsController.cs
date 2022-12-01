using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly RentalCatalog rentalCatalog;

        public RentalsController(RentalCatalog rentalCatalog)
        {
            this.rentalCatalog = rentalCatalog;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!rentalCatalog.HaveRental(rentalId))
                throw new ApplicationException("Rental not found");

            Rental rental = rentalCatalog.Get(rentalId);
            
            return new RentalViewModel()
            {
                Id = rentalId,
                Units = rental.Units
            };
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            return new ResourceIdViewModel()
            {
                Id = rentalCatalog.CreateRental(model.Units) 
            };
        }
    }
}
