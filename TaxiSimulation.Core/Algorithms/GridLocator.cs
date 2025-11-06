using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiSimulation.Core.Interfaces;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Algorithms
{
    public class GridLocator : IDriverLocator
    {

        public GridLocator(DriverService driverService)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Driver> FindNearestDrivers(Position orderPosition, int count = 5)
        {
            throw new NotImplementedException();
        }
    }
}
