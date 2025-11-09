using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiSimulation.Core.Models
{
    /// <summary>
    /// Информация о конкретном водителе
    /// </summary>
    public class Driver
    {
        public Guid Id { get; }    // Уникальный идентификатор
        public Position Position { get; private set; }

        /// <summary>
        /// Создание нового водителя
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="position">Его позиция</param>
        public Driver(Guid id, Position position)
        {
            Id = id;
            Position = position;
        }

        /// <summary>
        /// Обновление позиции водителя
        /// </summary>
        /// <param name="newPosition">Новая позиция</param>
        public void UpdatePosition(Position newPosition)
        {
            Position = newPosition;
        }
    }
}
