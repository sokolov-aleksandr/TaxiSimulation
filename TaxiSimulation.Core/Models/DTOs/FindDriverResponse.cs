using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiSimulation.Core.Models.DTOs
{
    // Объект для передачи данных
    // Ответ с найденым водителем
    public record FindDriverResponse(int DriverId, int X, int Y, double RouteLength, List<Position> Route);
}
