using System;
using NUnit.Framework;
using TaxiSimulation.Core.Models;

namespace TaxiSimulation.Core.Tests
{
    [TestFixture]
    public class DriverTests
    {
        [Test]
        public void Constructor_ShouldSetIdAndPosition()
        {
            var id = Guid.NewGuid();
            var pos = new Position(1, 2);
            var driver = new Driver(id, pos);

            Assert.That(driver.Id, Is.EqualTo(id));
            Assert.That(driver.Position, Is.EqualTo(pos));
        }

        [Test]
        public void UpdatePosition_ShouldChangePosition()
        {
            var driver = new Driver(Guid.NewGuid(), new Position(1, 1));
            var newPos = new Position(2, 3);
            driver.UpdatePosition(newPos);

            Assert.That(driver.Position, Is.EqualTo(newPos));
        }
    }
}
