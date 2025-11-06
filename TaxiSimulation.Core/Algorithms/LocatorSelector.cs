using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiSimulation.Core.Interfaces;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Algorithms
{
    public enum LocatorMethod
    {
        BruteForce,
        Grid,
        KdTree
    }


    public class LocatorSelector
    {
        public IDriverLocator GetLocator(DriverService driverService, LocatorMethod method)
        {
            return method switch
            {
                LocatorMethod.BruteForce => new BruteForceLocator(driverService),
                LocatorMethod.Grid => new GridLocator(driverService),
                _ => throw new ArgumentException("Unknown locator method")
            };
        }
    }

}
