namespace HashCode.Test
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RoundTests
    {
        private static Round round;

        /// <summary>Inialize a new static instance of delivery</summary>
        /// <param name="context">test context</param>
        [ClassInitialize]
        public static void InitializeDelivery(TestContext context)
        {
            Write.TraceWatch("starting");

            RoundTests.round = Round.RoundFromFile(Inputs.InExample);
        }

        [TestMethod()]
        public void AssignVideosListTest()
        {
            RoundTests.round.AssignVideos(Round.AssignMode.Standard);

            Write.TraceWatch("videos assigned");

            RoundTests.round.PrintAssigment(Inputs.OutExample);

            string actual = File.ReadAllText(Inputs.OutExample);
            string expected = "2\r\n0 3 \r\n1 1 \r\n";

            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void RoundFromExampleTest()
        {
            Write.TraceWatch("enter RoundFromExampleTest");

            StringBuilder actual = new StringBuilder();

            actual.Append(Write.Invariant($"{round.Videos.Count} {round.EndPoints.Count} {round.Requests.Count} {round.CacheServers.Count()} {round.CacheServers[0].MaxSize}\n"));

            for (int i = 0; i < round.Videos.Count; i++)
            {
                actual.Append(round.Videos[i].Size);

                if (i + 1 < round.Videos.Count)
                {
                    actual.Append(" ");
                }
                else
                {
                    actual.Append('\n');
                }
            }

            foreach (EndPoint endpoint in round.EndPoints)
            {
                actual.Append(Write.Invariant($"{endpoint.DataCenterLatency} {endpoint.CacheServerLatencies.Count}\n"));

                foreach (Latency latency in endpoint.CacheServerLatencies)
                {
                    actual.Append(Write.Invariant($"{latency.CacheServerID} {latency.Time}\n"));
                }
            }

            foreach (Request request in round.Requests)
            {
                actual.Append(Write.Invariant($"{request.VideoID} {request.EndPointID} {request.Occurency}\n"));
            }

            string expected = File.ReadAllText(Inputs.InExample);

            Assert.AreEqual(expected, actual.ToString());
        }
    }
}