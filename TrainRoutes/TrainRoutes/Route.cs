using System.Collections.Generic;

namespace TrainRoutes
{
    internal class Route
    {
        public Route(int totalDistance, IEnumerable<string> nodes)
        {
            TotalDistance = totalDistance;
            Nodes = nodes;
        }

        public int TotalDistance { get; private set; }
        public IEnumerable<string> Nodes { get; private set; }
    }
}