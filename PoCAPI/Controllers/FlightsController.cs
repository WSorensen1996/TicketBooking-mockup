using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoCAPI.Data;
using PoCAPI.Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace PoCAPI.Controllers
{
    [Route("api/flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly flightsDbContext _db;

        public FlightsController(flightsDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FlightsDTO>> GetFlights()
        {
            return Ok(_db.flight.ToList());
        }

        [HttpGet("{FlightNumber}", Name = "flight")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<FlightsDTO> GetFlight(string FlightNumber)
        {
            if (string.IsNullOrEmpty(FlightNumber))
            {
                return BadRequest();
            }

            var flight = _db.flight.FirstOrDefault(u => u.FlightNumber == FlightNumber.ToString());

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
