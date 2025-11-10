using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiSimulation.Core.Models.DTOs
{
    // Объект для передачи данных
    // Поиск водителя на заказ
    public record FindDriverRequest(int OrderId, int X, int Y);
}
