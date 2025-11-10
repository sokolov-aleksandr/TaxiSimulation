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
        private readonly Grid _grid;    // Общая сетка
        public Grid Grid => _grid;      // Публичная для чтения
        private readonly object _lock = new();


        public DriverService(Grid grid)
        {
            _grid = grid;
        }

        public bool AddDriver(Driver driver, out string message)
        {
            lock (_lock)
            {
                if (!_grid.IsInside(driver.Position)) { message = "Координаты некорректны"; return false; }
                if (_drivers.Any(d => d.Position.X == driver.Position.X && d.Position.Y == driver.Position.Y))
                { message = "Здесь уже находится другой водитель"; return false; }

                _drivers.Add(driver);
                message = "Координаты успешно добавлены";
                return true;
            }
        }

        public bool UpdateDriver(int id, Position newPos, out string message)
        {
            lock (_lock)
            {
                var existing = _drivers.FirstOrDefault(d => d.Id == id);
                if (existing == null)
                {
                    if (!_grid.IsInside(newPos)) { message = "Координаты некорректны"; return false; }
                    if (_drivers.Any(d => d.Position.X == newPos.X && d.Position.Y == newPos.Y))
                    { message = "Здесь уже находится другой водитель"; return false; }

                    _drivers.Add(new Driver(id, newPos));
                    message = "Координаты успешно добавлены";
                    return true;
                }
                else
                {
                    if (!_grid.IsInside(newPos))
                    {
                        // требование: если водитель пытается изменить свои координаты, но они выходят за пределы карты,
                        // то его предыдущие координаты должны быть удалены
                        _drivers.Remove(existing);
                        message = "Координаты некорректны";
                        return false;
                    }

                    if (_drivers.Any(d => d.Id != id && d.Position.X == newPos.X && d.Position.Y == newPos.Y))
                    {
                        message = "Здесь уже находится другой водитель";
                        return false;
                    }

                    existing.UpdatePosition(newPos);
                    message = "Координаты успешно изменены";
                    return true;
                }
            }
        }

        public bool TryRemoveDriver(int id)
        {
            lock (_lock)
            {
                var existing = _drivers.FirstOrDefault(d => d.Id == id);
                if (existing == null) return false;
                _drivers.Remove(existing);
                return true;
            }
        }

        public IReadOnlyList<Driver> GetAll()
        {
            lock (_lock)
            {
                return _drivers.ToList().AsReadOnly();
            }
        }

        public Driver GetById(int id)
        {
            lock (_lock)
            {
                return _drivers.FirstOrDefault(d => d.Id == id);
            }
        }
    }
}
