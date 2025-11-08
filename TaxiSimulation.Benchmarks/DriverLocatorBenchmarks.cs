using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using TaxiSimulation.Core.Algorithms;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Benchmarks
{
    [MemoryDiagnoser] // показывает потребление памяти
    [RankColumn] // выводит сравнительный рейтинг
    public class DriverLocatorBenchmarks
    {
        private DriverService _driverService;
        private BruteForceLocator _bruteForceLocator;
        private GridLocator _gridLocator;
        private TreeDriverLocator _treeDriverLocator;

        private Position _orderPosition;

        [Params(150, 10000)]
        public int drivers_count;

        [Params(50, 10000)]
        public int grid_size;

        [GlobalSetup]
        public void Setup()
        {
            var grid = new Grid(grid_size, grid_size);
            _driverService = new DriverService(grid);

            // Генерим водителей (сид фиксированный)
            var rand = new Random(42);
            var usedPositions = new HashSet<(int, int)>(); // Позиции не повторяются
            for (int i = 0; i < drivers_count;)
            {
                int x = rand.Next(grid.Width);
                int y = rand.Next(grid.Height);

                if (usedPositions.Add((x, y))) // Не повторяемся
                {
                    _driverService.AddDriver(new Driver(Guid.NewGuid(), new Position(x, y)));
                    i++;
                }
            }

            _bruteForceLocator = new BruteForceLocator(_driverService);
            _gridLocator = new GridLocator(_driverService);
            _treeDriverLocator = new TreeDriverLocator(_driverService);

            _orderPosition = new Position(40, 40);
        }

        [Benchmark(Baseline = true)] // Базовый метод
        public void BruteForce()
        {
            _bruteForceLocator.FindNearestDrivers(_orderPosition, 5).ToList();
        }

        [Benchmark]
        public void GridBased()
        {
            _gridLocator.FindNearestDrivers(_orderPosition, 5).ToList();
        }

        [Benchmark]
        public void TreeBased()
        {
            _treeDriverLocator.FindNearestDrivers(_orderPosition, 5).ToList();
        }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<DriverLocatorBenchmarks>();
        }
    }
}
