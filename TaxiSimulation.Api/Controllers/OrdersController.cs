using Microsoft.AspNetCore.Mvc;
using TaxiSimulation.Core.Models.DTOs;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly DriverService _driverService;
        private readonly DriverLocatorService _locatorService;
        private readonly IConfiguration _cfg;
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly Random _random = new();

        public OrdersController(DriverService driverService, DriverLocatorService locatorService,
            IConfiguration cfg, IHttpClientFactory httpClientFactory)
        {
            _driverService = driverService;
            _locatorService = locatorService;
            _cfg = cfg;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("find")]
        public async Task<IActionResult> Find([FromBody] FindDriverRequest req)
        {
            var grid = new Grid(_cfg.GetValue<int>("MapSettings:Width"), _cfg.GetValue<int>("MapSettings:Height"));
            var pos = new Position(req.X, req.Y);
            if (!grid.IsInside(pos))
                return BadRequest(new { message = "Координаты некорректны" });

            var allDrivers = _driverService.GetAll();
            if (allDrivers.Count == 0)
                return BadRequest(new { message = "Свободных водителей нет" });

            var nearest = _locatorService.FindNearestDrivers(pos, Math.Min(10, allDrivers.Count)).ToList();
            if (!nearest.Any())
                return BadRequest(new { message = "Свободных водителей нет" });

            int chosenIndex = await GetRandomIndexAsync(nearest.Count);
            var chosen = nearest[chosenIndex];

            var route = BuildStepRoute(chosen.Position, pos);
            double routeLength = ComputeRouteLength(route);

            var resp = new FindDriverResponse(chosen.Id, chosen.Position.X, chosen.Position.Y, routeLength, route);
            return Ok(resp);
        }

        // Простая реализация поиска маршрута
        private static List<Position> BuildStepRoute(Position from, Position to)
        {
            var route = new List<Position>();
            int x = from.X, y = from.Y;
            route.Add(new Position(x, y));
            while (x != to.X)
            {
                x += Math.Sign(to.X - x);
                route.Add(new Position(x, y));
            }
            while (y != to.Y)
            {
                y += Math.Sign(to.Y - y);
                route.Add(new Position(x, y));
            }
            return route;
        }

        private static double ComputeRouteLength(List<Position> route)
        {
            double sum = 0;
            for (int i = 1; i < route.Count; i++)
                sum += route[i - 1].DistanceTo(route[i]);
            return sum;
        }

        private async Task<int> GetRandomIndexAsync(int maxExclusive)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                // Попробуем вызвать внешний API
                var url = $"http://www.randomnumberapi.com/api/v1.0/random?min=0&max={Math.Max(0, maxExclusive - 1)}&count=1";
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {
                    var text = await resp.Content.ReadAsStringAsync();
                    var arr = System.Text.Json.JsonSerializer.Deserialize<int[]>(text);
                    if (arr != null && arr.Length > 0)
                    {
                        int idx = arr[0];
                        if (idx >= 0 && idx < maxExclusive) return idx;
                    }
                }
            }
            catch { }

            // fallback на .NET Random
            lock (_random) return _random.Next(0, maxExclusive);
        }
    }
}

