using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainRoutes
{
    internal class RouteCalculator
    {   /// <summary>
    /// Forward connecting node names and the distances
    /// </summary>
    private class NodeConnections : Dictionary<string, int>{}
        private readonly Dictionary<string, NodeConnections> _nodesAndTheirConnections = new Dictionary<string, NodeConnections>();

        public RouteCalculator(params Edge[] edges)
        {
            if (edges == null)
                throw new ArgumentNullException("edges", "Edges not specified");
            if (edges.Count() == 0)
                throw new ArgumentException("Edges not specified", "edges");

            BuildNodeConnections(edges);
        }

        private void BuildNodeConnections(IEnumerable<Edge> edges)
        {
            foreach (var edge in edges)
            {
                if (!_nodesAndTheirConnections.ContainsKey(edge.From))
                {
                    _nodesAndTheirConnections[edge.From] = new NodeConnections();
                }
                _nodesAndTheirConnections[edge.From][edge.To] = edge.Distance;
            }
        }

        public List<Route> Routes(string from, string to, int maxHops = 100, int minHops = 1, int maxDistance = 1000, bool allowCircular = false)
        {
            ValidateThatNodeExists(from);
            ValidateThatNodeExists(to);

            var completedRoutes = new List<Route>();
            RouteRecursively(completedRoutes, to, from, new List<string> { from }, 0, 0, maxHops, minHops, maxDistance, allowCircular);
            return completedRoutes;
        }

        public Route ShortestRoute(string from, string to)
        {
            ValidateThatNodeExists(from);
            ValidateThatNodeExists(to);

            var completedRoutes = new List<Route>();
            RouteRecursively(completedRoutes, to, from, new List<string> { from }, 0, 0);
            return completedRoutes.OrderBy(r => r.TotalDistance).FirstOrDefault();
        }

        void RouteRecursively(ICollection<Route> completedRoutes, string endDestination, string from, IEnumerable<string> nodesOnRouteSoFar, int distanceSoFar, int hopsSoFar, int maxHops = 100, int minHops = 1, int maxDistance = 1000, bool allowCircular = false)
        {
            var nodesToTravelTo = _nodesAndTheirConnections[from].Keys;

            if (!nodesToTravelTo.Any())
            {
                return;//dead end!
            }

            //travel to all furter nodes
            foreach (var to in nodesToTravelTo)
            {
                var newDistanceSoFar = distanceSoFar + _nodesAndTheirConnections[from][to];
                var newNodesOnRouteSoFar = new List<string>(nodesOnRouteSoFar) { to };
                var newHopsSoFar = hopsSoFar + 1;

                if (maxHops < newHopsSoFar || maxDistance < newDistanceSoFar)
                    return;//over max, end

                if (to == endDestination)
                {
                    if (minHops <= newHopsSoFar)//if too few hops, this route doesn't count as complete
                    {
                        completedRoutes.Add(new Route (newDistanceSoFar, newNodesOnRouteSoFar));
                    }

                    if (!allowCircular)
                        return;//end completed route
                }

                //if we haven't reached the destination or a dead end  recursively search down each route
                RouteRecursively(completedRoutes, endDestination, to, newNodesOnRouteSoFar, newDistanceSoFar, newHopsSoFar, maxHops, minHops, maxDistance, allowCircular);
            }
        }

        public int? DirectRouteDistance(params string[] nodeNames)
        {
            ValidateThatNodesExist(nodeNames);

            //use a stack to pop through the route
            var nodesStack = new Stack<string>(nodeNames.Reverse());
            int distance = 0;

            while (nodesStack.Count > 1)
            {
                var from = nodesStack.Pop();
                var to = nodesStack.Peek();

                if (!_nodesAndTheirConnections[from].ContainsKey(to))
                {
                    return null;//NO ROUTE
                }

                distance += _nodesAndTheirConnections[from][to];
            }

            return distance;
        }

        private void ValidateThatNodeExists(string nodeName)
        {
            if (!_nodesAndTheirConnections.ContainsKey(nodeName))
                throw new ArgumentException(string.Format("Node {0} does not exist", nodeName), "nodeName");
        }

        private void ValidateThatNodesExist(IEnumerable<string> nodeNames)
        {
            foreach (var nodeName in nodeNames)
            {
                ValidateThatNodeExists(nodeName);
            }
        }
    }
}