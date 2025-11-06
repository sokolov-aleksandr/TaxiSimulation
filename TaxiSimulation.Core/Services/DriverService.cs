using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiSimulation.Core.Models;

namespace TaxiSimulation.Core.Services
{
    public class DriverService
    {
        private readonly List<Driver> _drivers = new List<Driver>();  // Список водителей 
        private readonly Grid _grid;                                  // Общая сетка

        public DriverService(Grid grid)
        {
            _grid = grid;
        }

        public void AddDriver(Driver driver)
        {
            if (!_grid.IsInside(driver.Position))
                throw new ArgumentOutOfRangeException("Driver position is outside the grid");

            if (_drivers.Any(d => d.Position.X == driver.Position.X && d.Position.Y == driver.Position.Y))
                throw new InvalidOperationException("Another driver is already at this position");

            _drivers.Add(driver);
        }

        public void UpdateDriver(Guid id, Position newPos)
        {
            if (!_grid.IsInside(newPos))
                throw new ArgumentOutOfRangeException("Driver position is outside the grid");

            if (_drivers.Any(d => d.Id != id && d.Position.X == newPos.X && d.Position.Y == newPos.Y))
                throw new InvalidOperationException("Another driver is already at this position");

            var driver = _drivers.FirstOrDefault(d => d.Id == id);
            driver?.UpdatePosition(newPos);
        }

        public IReadOnlyList<Driver> GetAll() => _drivers;
    }
}
