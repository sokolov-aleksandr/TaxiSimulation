using System;
using NUnit.Framework;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Tests
{
    [TestFixture]
    public class DriverServiceTests
    {
        [Test]
        public void AddDriver_ShouldAddValidDriver()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);
            var driver = new Driver(Guid.NewGuid(), new Position(1, 2));

            service.AddDriver(driver);

            Assert.That(service.GetAll(), Has.Count.EqualTo(1));
        }

        [Test]
        public void AddDriver_OutsideGrid_ShouldThrow()
        {
            var grid = new Grid(5, 5);
            var service = new DriverService(grid);
            var driver = new Driver(Guid.NewGuid(), new Position(10, 10));

            Assert.Throws<ArgumentOutOfRangeException>(() => service.AddDriver(driver));
        }

        [Test]
        public void AddDriver_DuplicatePosition_ShouldThrow()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);
            var pos = new Position(1, 1);
            service.AddDriver(new Driver(Guid.NewGuid(), pos));

            Assert.Throws<InvalidOperationException>(() =>
                service.AddDriver(new Driver(Guid.NewGuid(), pos)));
        }

        [Test]
        public void UpdateDriver_ValidUpdate_ShouldChangePosition()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);
            var driver = new Driver(Guid.NewGuid(), new Position(1, 1));
            service.AddDriver(driver);

            var newPos = new Position(2, 2);
            service.UpdateDriver(driver.Id, newPos);

            Assert.That(driver.Position, Is.EqualTo(newPos));
        }

        [Test]
        public void UpdateDriver_PositionOutsideGrid_ShouldThrow()
        {
            var grid = new Grid(5, 5);
            var service = new DriverService(grid);
            var driver = new Driver(Guid.NewGuid(), new Position(1, 1));
            service.AddDriver(driver);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                service.UpdateDriver(driver.Id, new Position(10, 10)));
        }
    }
}
