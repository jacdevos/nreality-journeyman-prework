namespace TrainRoutes
{
    internal class Edge
    {
        public Edge(string from, string to, int distance)
        {
            From = from;
            To = to;
            Distance = distance;
        }

        public string From { get; private set; }
        public string To { get; private set; }
        public int Distance { get; private set; }
    }
}