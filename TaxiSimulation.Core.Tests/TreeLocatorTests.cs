using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiSimulation.Core.Algorithms;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Tests
{
    [TestFixture]
    public class TreeLocatorTests
    {
        [Test]
        public void FindNearestDrivers_ShouldReturnNearestDriversOrdered()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);

            var d1 = new Driver(Guid.NewGuid(), new Position(1, 1));
            var d2 = new Driver(Guid.NewGuid(), new Position(5, 5));
            var d3 = new Driver(Guid.NewGuid(), new Position(2, 2));

            service.AddDriver(d1, out _);
            service.AddDriver(d2, out _);
            service.AddDriver(d3, out _);

            var locator = new TreeDriverLocator(service);
            var result = locator.FindNearestDrivers(new Position(0, 0), 2).ToList();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].Id, Is.EqualTo(d1.Id));
            Assert.That(result[1].Id, Is.EqualTo(d3.Id));
        }

        [Test]
        public void FindNearestDrivers_ShouldReturnEmpty_WhenNoDrivers()
        {
            var grid = new Grid(5, 5);
            var service = new DriverService(grid);
            var locator = new TreeDriverLocator(service);

            var result = locator.FindNearestDrivers(new Position(2, 2), 3);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void FindNearestDrivers_ShouldReturnAll_WhenRequestedCountExceedsDrivers()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);

            var d1 = new Driver(Guid.NewGuid(), new Position(1, 1));
            var d2 = new Driver(Guid.NewGuid(), new Position(3, 3));
            service.AddDriver(d1, out _);
            service.AddDriver(d2, out _);

            var locator = new TreeDriverLocator(service);
            var result = locator.FindNearestDrivers(new Position(0, 0), 5).ToList();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].Id, Is.EqualTo(d1.Id));
            Assert.That(result[1].Id, Is.EqualTo(d2.Id));
        }

        [Test]
        public void FindNearestDrivers_ShouldHandleDriversWithSameDistance()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);

            var d1 = new Driver(Guid.NewGuid(), new Position(1, 0)); 
            var d2 = new Driver(Guid.NewGuid(), new Position(0, 1));
            var d3 = new Driver(Guid.NewGuid(), new Position(5, 5));

            service.AddDriver(d1, out _);
            service.AddDriver(d2, out _);
            service.AddDriver(d3, out _);

            var locator = new TreeDriverLocator(service);
            var result = locator.FindNearestDrivers(new Position(0, 0), 2).ToList();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Select(d => d.Id), Does.Contain(d1.Id).And.Contain(d2.Id));
        }

    }
}
