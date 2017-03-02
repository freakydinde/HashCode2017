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

        [TestMethod]
        public void MeAtTheZooTest()
        {
            Write.TraceVisible("me_at_the_zoo", true);

            using (Round round = Round.RoundFromFile(Inputs.InMeAtTheZoo))
            {
                round.AssignVideos();
                round.PrintAssigment(Inputs.OutMeAtTheZoo);
                if (round.ComputeScore(Inputs.OutMeAtTheZoo)) round.TraceScore();
            }
        }

        [TestMethod()]
        public void AssignVideosListTest()
        {
            RoundTests.round.AssignVideos();

            Write.TraceWatch("videos assigned");

            RoundTests.round.PrintAssigment(Inputs.OutExample);

            if (RoundTests.round.ComputeScore(Inputs.OutExample)) RoundTests.round.TraceScore();

            string actual = File.ReadAllText(Inputs.OutExample) + RoundTests.round.Score.ToString();
            string expected = "1\r\n0 3 1 \r\n562500";

            Write.Trace(actual);

            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void RoundFromExampleTest()
        {
            Write.TraceWatch("enter RoundFromExampleTest");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(Write.Invariant($"{round.Videos.Count} {round.EndPoints.Count} {round.Requests.Count} {round.CacheServers.Count()} {round.CacheServers[0].MaxSize}"));

            for (int i = 0; i < round.Videos.Count; i++)
            {
                stringBuilder.Append(round.Videos[i].Size);

                if (i + 1 < round.Videos.Count)
                {
                    stringBuilder.Append(" ");
                }
                else
                {
                    stringBuilder.AppendLine();
                }
            }

            foreach (EndPoint endpoint in round.EndPoints)
            {
                stringBuilder.AppendLine(Write.Invariant($"{endpoint.DataCenterLatency} {endpoint.CacheServerLatencies.Count}"));

                foreach (Latency latency in endpoint.CacheServerLatencies)
                {
                    stringBuilder.AppendLine(Write.Invariant($"{latency.CacheServerID} {latency.Time}"));
                }
            }

            foreach (Request request in round.Requests)
            {
                stringBuilder.AppendLine(Write.Invariant($"{request.VideoID} {request.EndPointID} {request.Occurency}"));
            }

            string expected = File.ReadAllText(Inputs.InExample);
            string actual = stringBuilder.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}