using System;
using System.Linq;
using NUnit.Framework;
using TaxiSimulation.Core.Algorithms;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Tests
{
    [TestFixture]
    public class BruteForceLocatorTests
    {
        [Test]
        public void FindNearestDrivers_ShouldReturnNearestDriversOrdered()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);

            var d1 = new Driver(1, new Position(1, 1));
            var d2 = new Driver(2, new Position(5, 5));
            var d3 = new Driver(3, new Position(2, 2));
            service.AddDriver(d1, out _);
            service.AddDriver(d2, out _);
            service.AddDriver(d3, out _);

            var locator = new BruteForceLocator(service);
            var result = locator.FindNearestDrivers(new Position(0, 0), 2).ToList();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(d1));
            Assert.That(result[1], Is.EqualTo(d3));
        }
    }
}
