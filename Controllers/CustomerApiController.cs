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
        public IActionResult SearchFlight(string search)
        {
            return Ok(FlightStorage.SearchAirports(search));
        }
    }
}