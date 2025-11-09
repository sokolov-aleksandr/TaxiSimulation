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

            var result = service.AddDriver(driver, out var message);

            Assert.IsTrue(result);
            Assert.That(message, Is.EqualTo("Координаты успешно добавлены"));
            Assert.That(service.GetAll(), Has.Count.EqualTo(1));
        }

        [Test]
        public void AddDriver_OutsideGrid_ShouldFail()
        {
            var grid = new Grid(5, 5);
            var service = new DriverService(grid);
            var driver = new Driver(Guid.NewGuid(), new Position(10, 10));

            var result = service.AddDriver(driver, out var message);

            Assert.IsFalse(result);
            Assert.That(message, Is.EqualTo("Координаты некорректны"));
        }

        [Test]
        public void AddDriver_DuplicatePosition_ShouldFail()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);
            var pos = new Position(1, 1);
            service.AddDriver(new Driver(Guid.NewGuid(), pos), out _);

            var result = service.AddDriver(new Driver(Guid.NewGuid(), pos), out var message);

            Assert.IsFalse(result);
            Assert.That(message, Is.EqualTo("Здесь уже находится другой водитель"));
        }

        [Test]
        public void UpdateDriver_ValidUpdate_ShouldChangePosition()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);
            var driver = new Driver(Guid.NewGuid(), new Position(1, 1));
            service.AddDriver(driver, out _);

            var newPos = new Position(2, 2);
            var result = service.UpdateDriver(driver.Id, newPos, out var message);

            Assert.IsTrue(result);
            Assert.That(message, Is.EqualTo("Координаты успешно изменены"));
            Assert.That(driver.Position, Is.EqualTo(newPos));
        }

        [Test]
        public void UpdateDriver_PositionOutsideGrid_ShouldFailAndRemoveDriver()
        {
            var grid = new Grid(5, 5);
            var service = new DriverService(grid);
            var driver = new Driver(Guid.NewGuid(), new Position(1, 1));
            service.AddDriver(driver, out _);

            var result = service.UpdateDriver(driver.Id, new Position(10, 10), out var message);

            Assert.IsFalse(result);
            Assert.That(message, Is.EqualTo("Координаты некорректны"));
            Assert.That(service.GetById(driver.Id), Is.Null);
        }
    }
}
