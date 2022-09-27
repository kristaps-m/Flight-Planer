using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private static readonly object ObjectLock = new object();
        
        [Route("airports")]
        [HttpGet]
        public IActionResult SearchAirports(string search)
        {
            return Ok(FlightStorage.SearchAirports(search));
        }

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

            lock (ObjectLock)
            {
                var pageResult = FlightStorage.FindFlightByRequest(req);
                return Ok(pageResult);
            }
        }
        
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
        
        [Microsoft.AspNetCore.Mvc.NonAction]
        public override Microsoft.AspNetCore.Mvc.BadRequestResult BadRequest()
        {
            return new BadRequestResult(); // 400;
        }

        private static bool IsItBadRequest(SearchFlightsRequest req)
        {
            return req.From == null || req.To == null || req.DepartureDate == null;
        }
    }
}