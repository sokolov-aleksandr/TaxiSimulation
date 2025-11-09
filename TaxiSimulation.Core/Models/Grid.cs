using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiSimulation.Core.Models
{
    /// <summary>
    /// Общая Сетка/Карта
    /// </summary>
    public class Grid
    {
        public int Width { get; }
        public int Height { get; }

        /// <summary>
        /// Создание сетки с параметрами
        /// </summary>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <exception cref="ArgumentException">Размер должен быть больше 0</exception>
        public Grid(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Grid size must be positive");
            
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Проверка позиции на нахождение внутри сетки
        /// </summary>
        /// <param name="pos">Позиция для проверки</param>
        /// <returns>True если внутри сетки</returns>
        public bool IsInside(Position pos) =>
            pos.X >= 0 && pos.X < Width &&
            pos.Y >= 0 && pos.Y < Height;
    }
}
