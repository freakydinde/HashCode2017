using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashCode.Test
{
    [TestClass]
    public class RoundTests
    {
        private static Round round;

        /// <summary>Inialize a new static instance of delivery</summary>
        /// <param name="context">test context</param>
        [ClassInitialize]
        public static void InitializeDelivery(TestContext context)
        {
            Watch.TraceWatch("starting");

            RoundTests.round = Round.RoundFromFile(Inputs.InExample);
        }

        [TestMethod]
        public void RoundFromExampleTest()
        {
            Watch.TraceWatch("enter RoundFromExampleTest");

            StringBuilder actual = new StringBuilder();

            actual.Append(Write.Invariant($"{round.Videos.Count} {round.EndPoints.Count} {round.Requests.Count} {round.CacheServers.Count()} {round.CacheServers[0].MaxSize}\n"));

            for(int i = 0; i < round.Videos.Count; i++)
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