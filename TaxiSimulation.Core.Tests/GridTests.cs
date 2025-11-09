using System;
using NUnit.Framework;
using TaxiSimulation.Core.Models;

namespace TaxiSimulation.Core.Tests
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void Constructor_ValidDimensions_ShouldSetProperties()
        {
            var grid = new Grid(10, 20);
            Assert.That(grid.Width, Is.EqualTo(10));
            Assert.That(grid.Height, Is.EqualTo(20));
        }

        [TestCase(0, 10)]
        [TestCase(10, 0)]
        [TestCase(-1, 5)]
        public void Constructor_InvalidDimensions_ShouldThrow(int w, int h)
        {
            Assert.Throws<ArgumentException>(() => new Grid(w, h));
        }

        [TestCase(5, 5, true)]
        [TestCase(-1, 0, false)]
        [TestCase(9, 19, true)]
        [TestCase(10, 10, false)]
        public void IsInside_CheckBoundaries(int x, int y, bool expected)
        {
            var grid = new Grid(10, 20);
            var pos = new Position(x, y);
            Assert.That(grid.IsInside(pos), Is.EqualTo(expected));
        }
    }
}
