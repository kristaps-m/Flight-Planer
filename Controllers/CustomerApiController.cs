using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        // Search Airport
        [Route("airports")]
        [HttpGet]
        public IActionResult SearchAirports(string search)
        {
            return Ok(FlightStorage.SearchAirports(search));
        }

        // Search Airport   // public FIND FLIGHTS ?
        [Route("flights/search")]
        [HttpGet]
        public IActionResult FindFlight(string req)
        {
            var airports = FlightStorage.SearchAirports(req);
            return Ok(airports);
        }
        
        // Find Flight by ID
        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult FindFlightById(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }
    }
}