using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Algorithms
{
    public class BruteForceLocator
    {
        private readonly DriverService _driverService;

        public BruteForceLocator(DriverService driverService)
        {
            this._driverService = driverService;
        }

        public IEnumerable<Driver> FindNearestDrivers(Position orderPosition, int count = 5)
        {
            return _driverService.GetAll()
                .OrderBy(d => Math.Pow(d.Position.X - orderPosition.X, 2) + Math.Pow(d.Position.Y - orderPosition.Y, 2))
                .Take(count);
        }
    }
}
