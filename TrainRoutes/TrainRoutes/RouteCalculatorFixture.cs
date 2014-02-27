using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrainRoutes
{
    [TestClass]
    public class RouteCalculatorFixture
    {
        private TrainRoutes.RouteCalculator _trains;

        [TestInitialize]
        public void TestInitialize()
        {
            //AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7 
            _trains = new TrainRoutes.RouteCalculator(
                new Edge("A", "B", 5),
                new Edge("B", "C", 4),
                new Edge("C", "D", 8),
                new Edge("D", "C", 8),
                new Edge("D", "E", 6),
                new Edge("A", "D", 5),
                new Edge("C", "E", 2),
                new Edge("E", "B", 3),
                new Edge("A", "E", 7));           
        }

        [TestMethod]
        public void Direct_route_ABC_should_be_distance_of_9()
        {
            Assert.AreEqual(9, _trains.DirectRouteDistance("A", "B", "C"), "Direct route ABC should be distance of 9");
        }
        [TestMethod]
        public void Direct_route_AD_should_be_distance_of_5()
        {
            Assert.AreEqual(5, _trains.DirectRouteDistance("A", "D"), "Direct route AD should be distance of 5");
        }
        [TestMethod]
        public void Direct_route_ADC_should_be_distance_of_13()
        {
            Assert.AreEqual(13, _trains.DirectRouteDistance("A", "D", "C"), "Direct route ADC should be distance of 13");
        }
        [TestMethod]
        public void Direct_route_AEBCD_should_be_distance_of_22()
        {
            Assert.AreEqual(22, _trains.DirectRouteDistance("A", "E", "B", "C", "D"), "Direct route AEBCD should be distance of 22");
        }
        [TestMethod]
        public void Direct_route_AED_should_be_distance_of_null()
        {
            Assert.AreEqual(null, _trains.DirectRouteDistance("A", "E", "D"), "Direct route AED should be distance of null (meaning NO SUCH ROUTE)");
        }
        [TestMethod]
        public void There_should_be_2_routes_from_C_to_C_with_max_3_stops()
        {
            Assert.AreEqual(2, _trains.Routes("C", "C", 3).Count, "There should be 2 routes from C to C with max 3 stops");
        }
        [TestMethod]
        public void There_should_be_3_routes_from_A_to_C_with_exactly_4_stops()
        {
            Assert.AreEqual(3, _trains.Routes("A", "C", 4, 4, 1000, true).Count, "There should be 3 routes from A to C with exactly 4 stops");
        }
        [TestMethod]
        public void The_shortest_route_from_A_to_C_should_be_9()
        {
            Assert.AreEqual(9, _trains.ShortestRoute("A", "C").TotalDistance, "The shortest route from A to C should be 9");
        }
        [TestMethod]
        public void The_shortest_route_from_B_to_B_should_be_9()
        {
            Assert.AreEqual(9, _trains.ShortestRoute("B", "B").TotalDistance, "The shortest route from B to B should be 9");
        }

        [TestMethod]
        public void There_should_be_7_routes_from_A_to_C_with_distances_of_less_than_30()
        {
            Assert.AreEqual(7, _trains.Routes("C", "C", 100, 1, 29, true).Count, "There should be 7 routes from A to C with distances of less than 30");
        }
    }
}
