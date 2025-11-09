using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiSimulation.Core.Models;

namespace TaxiSimulation.Core.Interfaces
{
    public interface IDriverLocator
    {
        /// <summary>
        /// Интерфейс для алгоритмов поиска ближайших водителей
        /// </summary>
        /// <param name="orderPosition">Позиция заказа</param>
        /// <param name="count">Сколько нужно найти ближайших водителей</param>
        /// <returns>Список водителей</returns>
        IEnumerable<Driver> FindNearestDrivers(Position orderPosition, int count = 5);
    }
}
