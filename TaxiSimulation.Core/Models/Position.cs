using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiSimulation.Core.Models
{
    /// <summary>
    /// Инфа о позиции + нужные методы
    /// </summary>
    public readonly struct Position
    {
        public int X { get;  }
        public int Y { get;  }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Дистанция до другой позиции
        /// </summary>
        /// <param name="other">Другая позиция</param>
        /// <returns>Расстояние по теореме Пифагора</returns>
        public double DistanceTo(Position other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
