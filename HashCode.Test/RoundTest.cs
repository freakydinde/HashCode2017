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

        [TestMethod]
        public void RoundSetVideosList()
        {
            Write.TraceWatch("enter RoundCacheVideos");

            RoundTests.round.SetVideosList();

            StringBuilder actual = new StringBuilder();

            foreach (CacheServer cacheServer in RoundTests.round.CacheServers)
            {
                foreach (GainCacheServer gain in cacheServer.GainCacheServers)
                {
                    actual.Append(Write.Invariant($"cacheServerID:{cacheServer.ID} "));
                    actual.AppendLine(gain.ToString());
                }
            }

            StringBuilder expected = new StringBuilder();

            expected.AppendLine("cacheServerID:0 EndPointID:0 Gain:900 VideoID:3 VideoSize:30 GainPerMegaByte:30");
            expected.AppendLine("cacheServerID:0 EndPointID:0 Gain:900 VideoID:4 VideoSize:110 GainPerMegaByte:8");
            expected.AppendLine("cacheServerID:0 EndPointID:0 Gain:900 VideoID:1 VideoSize:50 GainPerMegaByte:18");
            expected.AppendLine("cacheServerID:1 EndPointID:0 Gain:700 VideoID:3 VideoSize:30 GainPerMegaByte:23");
            expected.AppendLine("cacheServerID:1 EndPointID:0 Gain:700 VideoID:4 VideoSize:110 GainPerMegaByte:6");
            expected.AppendLine("cacheServerID:1 EndPointID:0 Gain:700 VideoID:1 VideoSize:50 GainPerMegaByte:14");
            expected.AppendLine("cacheServerID:2 EndPointID:0 Gain:800 VideoID:3 VideoSize:30 GainPerMegaByte:26");
            expected.AppendLine("cacheServerID:2 EndPointID:0 Gain:800 VideoID:4 VideoSize:110 GainPerMegaByte:7");
            expected.AppendLine("cacheServerID:2 EndPointID:0 Gain:800 VideoID:1 VideoSize:50 GainPerMegaByte:16");

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}