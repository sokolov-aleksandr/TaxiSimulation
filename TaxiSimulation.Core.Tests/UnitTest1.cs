using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using TaxiSimulation.Core.Algorithms;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Tests
{
    [TestFixture]
    public class DriverServiceTests
    {
        private Grid _grid;
        private DriverService _service;

        [SetUp]
        public void Setup()
        {
            _grid = new Grid(10, 10);
            _service = new DriverService(_grid);
        }

        [Test]
        public void AddDriver_ValidPosition_AddsDriver()
        {
            var driver = new Driver(Guid.NewGuid(), new Position(2, 3));
            _service.AddDriver(driver);

            Assert.AreEqual(1, _service.GetAll().Count);
            Assert.AreEqual(driver.Position.X, _service.GetAll()[0].Position.X);
            Assert.AreEqual(driver.Position.Y, _service.GetAll()[0].Position.Y);
        }

        [Test]
        public void AddDriver_PositionOutsideGrid_ThrowsException()
        {
            var driver = new Driver(Guid.NewGuid(), new Position(11, 3));

            Assert.Throws<ArgumentOutOfRangeException>(() => _service.AddDriver(driver));
        }

        [Test]
        public void AddDriver_PositionOccupied_ThrowsException()
        {
            var driver1 = new Driver(Guid.NewGuid(), new Position(1, 1));
            var driver2 = new Driver(Guid.NewGuid(), new Position(1, 1));

            _service.AddDriver(driver1);
            Assert.Throws<InvalidOperationException>(() => _service.AddDriver(driver2));
        }

        [Test]
        public void UpdateDriver_ValidPosition_UpdatesPosition()
        {
            var driver = new Driver(Guid.NewGuid(), new Position(0, 0));
            _service.AddDriver(driver);

            _service.UpdateDriver(driver.Id, new Position(5, 5));
            var updated = _service.GetAll().First();

            Assert.AreEqual(5, updated.Position.X);
            Assert.AreEqual(5, updated.Position.Y);
        }

        [Test]
        public void UpdateDriver_PositionOutsideGrid_ThrowsException()
        {
            var driver = new Driver(Guid.NewGuid(), new Position(0, 0));
            _service.AddDriver(driver);

            Assert.Throws<ArgumentOutOfRangeException>(() => _service.UpdateDriver(driver.Id, new Position(20, 20)));
        }

        [Test]
        public void UpdateDriver_PositionOccupiedByOther_ThrowsException()
        {
            var driver1 = new Driver(Guid.NewGuid(), new Position(2, 2));
            var driver2 = new Driver(Guid.NewGuid(), new Position(3, 3));
            _service.AddDriver(driver1);
            _service.AddDriver(driver2);

            Assert.Throws<InvalidOperationException>(() => _service.UpdateDriver(driver2.Id, new Position(2, 2)));
        }
    }

    [TestFixture]
    public class BruteForceLocatorTests
    {
        private DriverService _service;
        private BruteForceLocator _locator;

        [SetUp]
        public void Setup()
        {
            var grid = new Grid(10, 10);
            _service = new DriverService(grid);

            _service.AddDriver(new Driver(Guid.NewGuid(), new Position(0, 0)));
            _service.AddDriver(new Driver(Guid.NewGuid(), new Position(1, 1)));
            _service.AddDriver(new Driver(Guid.NewGuid(), new Position(2, 2)));
            _service.AddDriver(new Driver(Guid.NewGuid(), new Position(5, 5)));
            _service.AddDriver(new Driver(Guid.NewGuid(), new Position(9, 9)));

            _locator = new BruteForceLocator(_service);
        }

        [Test]
        public void FindNearestDrivers_ReturnsCorrectCount()
        {
            var orderPos = new Position(0, 0);
            var nearest = _locator.FindNearestDrivers(orderPos, 3).ToList();

            Assert.AreEqual(3, nearest.Count);
        }

        [Test]
        public void FindNearestDrivers_ReturnsClosestDrivers()
        {
            var orderPos = new Position(0, 0);
            var nearest = _locator.FindNearestDrivers(orderPos, 2).ToList();

            // первые два ближайших водителя должны быть на (0,0) и (1,1)
            Assert.IsTrue(nearest.Any(d => d.Position.X == 0 && d.Position.Y == 0));
            Assert.IsTrue(nearest.Any(d => d.Position.X == 1 && d.Position.Y == 1));
        }

        [Test]
        public void FindNearestDrivers_RequestMoreThanAvailable_ReturnsAll()
        {
            var orderPos = new Position(0, 0);
            var nearest = _locator.FindNearestDrivers(orderPos, 10).ToList();

            Assert.AreEqual(_service.GetAll().Count, nearest.Count);
        }
    }

}