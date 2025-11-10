using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiSimulation.Core.Models.DTOs
{
    // Объект для передачи данных
    // Создание или обновление водителя
    public record UpsertDriverRequest(int Id, int X, int Y);
}
