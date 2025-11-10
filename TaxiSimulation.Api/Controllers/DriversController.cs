using Microsoft.AspNetCore.Mvc;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Models.DTOs;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly DriverService _driverService;
        private readonly DriverLocatorService _locatorService;

        public DriversController(DriverService driverService, DriverLocatorService locatorService)
        {
            _driverService = driverService;
            _locatorService = locatorService;
        }

        [HttpPost("upsert")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Строка некорректна
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Сервис недоступен (перегружен)
        public  IActionResult Upsert([FromBody] UpsertDriverRequest req)
        {
            var pos = new Position(req.X, req.Y);

            if (_driverService.UpdateDriver(req.Id, pos, out var message))
            {
                _locatorService.Rebuild();
                return Ok(new {message});
            }
            else
            {
                return BadRequest(new {message});
            }
        }
    }
}
