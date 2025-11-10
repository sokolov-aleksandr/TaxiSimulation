using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiSimulation.Core.Algorithms;
using TaxiSimulation.Core.Models;

namespace TaxiSimulation.Core.Services
{
    public class DriverLocatorService
    {
        private readonly DriverService _driverService;
        private TreeDriverLocator _locator;
        private readonly object _lock = new();

        public DriverLocatorService(DriverService driverService)
        {
            _driverService = driverService;
            Rebuild();
        }

        public void Rebuild()
        {
            lock (_lock)
            {
                _locator = new TreeDriverLocator(_driverService);
            }
        }

        public IEnumerable<Driver> FindNearestDrivers(Position pos, int count = 5)
        {
            lock (_lock)
            {
                return _locator.FindNearestDrivers(pos, count).ToList();
            }
        }
    }
}
