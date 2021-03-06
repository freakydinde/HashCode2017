﻿namespace HashCode.Test
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
        public void ExampleScoreTest()
        {
            Write.TraceVisible("example", true);

            double actual = 0;
            double expected = 462500;

            string testFile = Path.Combine(Inputs.ResourcesFolder, "example_462500.out");

            RoundTests.round.ComputeScore(testFile);

            actual = RoundTests.round.Score;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MeAtTheZooScoreTest()
        {
            Write.TraceVisible("me_at_the_zoo", true);

            double actual = 0;
            double expected = 484389;

            string testFile = Path.Combine(Inputs.ResourcesFolder, "me_at_the_zoo_484389.out");

            using (Round round = Round.RoundFromFile(Inputs.InMeAtTheZoo))
            {
                round.ComputeScore(testFile);

                actual = round.Score;
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrendingTodayScoreTest()
        {
            Write.TraceVisible("trending today", true);

            double actual = 0;
            double expected = 499237;

            string testFile = Path.Combine(Inputs.ResourcesFolder, "trending_today_499237.out");

            using (Round round = Round.RoundFromFile(Inputs.InTrendingToday))
            {
                round.ComputeScore(testFile);

                actual = round.Score;
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ExampleFullTest()
        {
            RoundTests.round.AssignVideos();

            RoundTests.round.PrintAssigment(Inputs.OutExample);
            RoundTests.round.ComputeScore(Inputs.OutExample);

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

            foreach (EndPoint endpoint in round.EndPoints.Values)
            {
                stringBuilder.AppendLine(Write.Invariant($"{endpoint.DataCenterLatency} {endpoint.CacheServerLatencies.Count}"));

                foreach (int id in endpoint.CacheServerIds)
                {
                    int latency = endpoint.CacheServerLatencies[id];
                    stringBuilder.AppendLine(Write.Invariant($"{id} {latency}"));
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