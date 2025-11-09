using System;
using System.Collections.Generic;
using System.Linq;
using TaxiSimulation.Core.Interfaces;
using TaxiSimulation.Core.Models;
using TaxiSimulation.Core.Services;

namespace TaxiSimulation.Core.Algorithms
{
    public class TreeDriverLocator : IDriverLocator
    {
        private readonly KDTreeNode _root;

        public TreeDriverLocator(DriverService driverService)
        {
            var drivers = driverService.GetAll().ToList();
            _root = BuildTree(drivers, depth: 0);
        }

        public IEnumerable<Driver> FindNearestDrivers(Position orderPosition, int count = 5)
        {
            var best = new SortedList<double, Driver>(new DuplicateKeyComparer<double>());
            SearchNearest(_root, orderPosition, count, 0, best);
            return best.Values.Take(count);
        }

        // Построение KD-дерева рекурсивно
        private KDTreeNode BuildTree(List<Driver> drivers, int depth)
        {
            if (drivers == null || drivers.Count == 0)
                return null;

            int axis = depth % 2; // 0 = X, 1 = Y
            drivers = axis == 0
                ? drivers.OrderBy(d => d.Position.X).ToList()
                : drivers.OrderBy(d => d.Position.Y).ToList();

            int median = drivers.Count / 2;
            var node = new KDTreeNode(drivers[median]);
            node.Left = BuildTree(drivers.Take(median).ToList(), depth + 1);
            node.Right = BuildTree(drivers.Skip(median + 1).ToList(), depth + 1);
            return node;
        }

        private void SearchNearest(KDTreeNode node, Position target, int count, int depth, SortedList<double, Driver> best)
        {
            if (node == null)
                return;

            double distance = node.Driver.Position.DistanceTo(target);
            if (!best.ContainsKey(distance))
                best.Add(distance, node.Driver);

            if (best.Count > count)
                best.RemoveAt(best.Count - 1);

            int axis = depth % 2;
            double diff = axis == 0 ? target.X - node.Driver.Position.X : target.Y - node.Driver.Position.Y;

            var (first, second) = diff < 0 ? (node.Left, node.Right) : (node.Right, node.Left);

            SearchNearest(first, target, count, depth + 1, best);

            // Проверяем, нужно ли идти в другую ветку
            if (best.Count < count || Math.Abs(diff) < best.Keys.Last())
                SearchNearest(second, target, count, depth + 1, best);
        }

        private class KDTreeNode
        {
            public Driver Driver { get; }
            public KDTreeNode Left { get; set; }
            public KDTreeNode Right { get; set; }

            public KDTreeNode(Driver driver)
            {
                Driver = driver;
            }
        }

        private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return result == 0 ? 1 : result;
            }
        }
    }
}
