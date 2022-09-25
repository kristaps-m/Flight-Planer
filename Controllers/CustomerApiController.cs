using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        // Search Airport // should search by incomplete phrases
        [Route("airports")]
        [HttpGet]
        public IActionResult SearchAirports(string search)
        {
            return Ok(FlightStorage.SearchAirports(search)); // This one works
        }

        // Search Airport   // public FIND FLIGHTS ?
        [Route("flights/search")]
        [HttpPost]
        public IActionResult FindFlight(SearchFlightsRequest req)
        {
            if (IsItBadRequest(req))
            {
                return BadRequest();
            }
            
            if (req.From == req.To)
            {
                return BadRequest();
            }

            var pageResult = FlightStorage.FindFlightByRequest(req);

            return Ok(pageResult);
        }
        
        // Find Flight by ID
        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult FindFlightById(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if (flight == null)
            {
                return NotFound(); // Nr 3-should not find anything when non existing flight id passed
            }
            return Ok(flight); // Nr 2-should be able to find flight by id
        }
        
        [Microsoft.AspNetCore.Mvc.NonAction]
        public override Microsoft.AspNetCore.Mvc.BadRequestResult BadRequest()
        {
            return new BadRequestResult(); // toBe(400);
        }

        private static bool IsItBadRequest(SearchFlightsRequest req)
        {
            return req.From == null || req.To == null || req.DepartureDate == null;
        }
    }
}