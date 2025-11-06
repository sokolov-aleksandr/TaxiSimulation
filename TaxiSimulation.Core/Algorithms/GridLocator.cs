using System;
using System.Collections.Generic;
using System.Linq;
using TaxiSimulation.Core.Interfaces;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Algorithms
{
    /// <summary>
    /// Алгоритм поиска ближайших водителей с использованием сетки
    /// </summary>
    public class GridLocator : IDriverLocator
    {
        private readonly DriverService _driverService;
        private readonly Grid _grid;

        // Хранилище индекса: ключ - (x, y), значение - водители в этой ячейке
        private Dictionary<(int, int), List<Driver>> _cellIndex = new();

        public GridLocator(DriverService driverService)
        {
            _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
            _grid = driverService.Grid;
        }

        /// <summary>
        /// Строит индекс по всем водителям: cell => водители
        /// </summary>
        private void BuildIndex()
        {
            _cellIndex = _driverService
                .GetAll()
                .GroupBy(d => (d.Position.X, d.Position.Y))
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public IEnumerable<Driver> FindNearestDrivers(Position orderPosition, int count = 5)
        {
            if (!_grid.IsInside(orderPosition))
                throw new ArgumentOutOfRangeException("Order position is outside grid");

            if (count <= 0)
                return Enumerable.Empty<Driver>();

            BuildIndex(); // обновляем индекс на момент запроса

            var foundDrivers = new List<Driver>();
            int radius = 0;

            // Расширяем радиус поиска, пока не найдём нужное количество
            while (foundDrivers.Count < count)
            {
                foreach (var pos in GetPositionsInRadius(orderPosition, radius))
                {
                    if (_cellIndex.TryGetValue((pos.X, pos.Y), out var drivers))
                        foundDrivers.AddRange(drivers);
                }

                // Если достигли границ — выходим
                if (radius > _grid.Width && radius > _grid.Height)
                    break;

                radius++;
            }

            // Отбираем ближайших из ближайших
            return foundDrivers
                .OrderBy(d => d.Position.DistanceTo(orderPosition))
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Возвращает все координаты в квадратном кольце радиуса r вокруг центра
        /// </summary>
        private IEnumerable<Position> GetPositionsInRadius(Position center, int radius)
        {
            if (radius == 0)
            {
                yield return center;
                yield break;
            }

            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    if (Math.Abs(dx) != radius && Math.Abs(dy) != radius)
                        continue; // только рамка, не заполняем весь квадрат

                    var x = center.X + dx;
                    var y = center.Y + dy;

                    var pos = new Position(x, y);
                    if (_grid.IsInside(pos))
                        yield return pos;
                }
            }
        }
    }
}
