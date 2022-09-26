using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController,Authorize]
    public class AdminApiController : ControllerBase
    {
        private static readonly object balanceLock = new object();
        
        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            // lock (balanceLock)
            // {
            //     var flight = FlightStorage.GetFlight(id);
            //     
            //     if (flight == null)
            //     {
            //         return NotFound();
            //     }
            // }
            
            var flight = FlightStorage.GetFlight(id);
                
            if (flight == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [Microsoft.AspNetCore.Mvc.NonAction]
        public override Microsoft.AspNetCore.Mvc.BadRequestResult BadRequest()
        {
            return new BadRequestResult(); // toBe(400);
        }

        [Route("flights")]
        [HttpPut]
        public IActionResult PutFlight(Flight flight)
        {
            if (FlightStorage.IsThereSameFlightInStorage(flight))
            {
                return Conflict();
            }

            if (FlightStorage.DoesFlightHaveWrongValues(flight))
            {
                //Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return BadRequest();
            }

            if (FlightStorage.DoesFlightHaveSameAirport(flight))
            {
                return BadRequest(); 
            }

            if (FlightStorage.DoesPlaneTakeOfAndLandOnRightTime(flight))
            {
                return BadRequest();
            }
            flight = FlightStorage.AddFlight(flight);
            
            // lock (balanceLock)
            // {
            //     if (FlightStorage.IsThereSameFlightInStorage(flight))
            //     {
            //         return Conflict();
            //     }
            //
            //     if (FlightStorage.DoesFlightHaveWrongValues(flight))
            //     {
            //         //Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            //         return BadRequest();
            //     }
            //
            //     if (FlightStorage.DoesFlightHaveSameAirport(flight))
            //     {
            //         return BadRequest(); 
            //     }
            //
            //     if (FlightStorage.DoesPlaneTakeOfAndLandOnRightTime(flight))
            //     {
            //         return BadRequest();
            //     }
            //     flight = FlightStorage.AddFlight(flight);
            // }
            
            //flight = FlightStorage.AddFlight(flight);
            return Created("",flight);
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            // lock (balanceLock)
            // {
            //     FlightStorage.Delete(id);
            // }
            FlightStorage.Delete(id);
            return Ok();
        }
    }
}
