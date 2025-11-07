using NUnit.Framework;
using TaxiSimulation.Core.Models;

namespace TaxiSimulation.Core.Tests
{
    [TestFixture]
    public class PositionTests
    {
        [Test]
        public void DistanceTo_ShouldReturnCorrectValue()
        {
            var a = new Position(0, 0);
            var b = new Position(3, 4);

            var distance = a.DistanceTo(b);
            Assert.That(distance, Is.EqualTo(5.0).Within(1e-5));
        }
    }
}
