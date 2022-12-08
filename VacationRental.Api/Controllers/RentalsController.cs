using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Controllers.Models;
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
                Id = rentalCatalog.CreateRental(model.Units, model.PreparationTime) 
            };
        }
        
        [HttpPut]
        [Route("{rentalId:int}")]
        public ActionResult Put(int rentalId, RentalBindingModel model)
        {
            try
            {
                this.rentalCatalog.UpdateRental(rentalId, model.Units, model.PreparationTime);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return Conflict();
            }
        }
    }
}
