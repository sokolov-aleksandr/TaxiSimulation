using System;
using NUnit.Framework;
using TaxiSimulation.Core.Algorithms;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Tests
{
    [TestFixture]
    public class LocatorSelectorTests
    {
        [Test]
        public void GetLocator_BruteForce_ShouldReturnCorrectType()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);
            var selector = new LocatorSelector();

            var locator = selector.GetLocator(service, LocatorMethod.BruteForce);

            Assert.That(locator, Is.TypeOf<BruteForceLocator>());
        }

        [Test]
        public void GetLocator_UnknownMethod_ShouldThrow()
        {
            var grid = new Grid(10, 10);
            var service = new DriverService(grid);
            var selector = new LocatorSelector();

            Assert.Throws<ArgumentException>(() =>
                selector.GetLocator(service, (LocatorMethod)999));
        }
    }
}
