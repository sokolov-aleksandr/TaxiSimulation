using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiSimulation.Core.Models
{
    /// <summary>
    /// Информация о заказе такси
    /// </summary>
    public class Order
    {
        public Guid Id { get; } = Guid.NewGuid(); // Уникальный идентификатор заказа
        public Position Position { get; } // Позиция подачи такси (в реальности позиция клиента)

        /// <summary>
        /// Создание заказа
        /// </summary>
        /// <param name="position">Позиция подачи такси</param>
        public Order(Position position)
        {
            Position = position;
        }
    }
}
