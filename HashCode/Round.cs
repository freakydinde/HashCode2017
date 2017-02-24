namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class Round
    {
        private List<CacheServer> CacheServers;
        private List<EndPoint> EndPoints;
        private List<Request> Requests;
        private List<Video> Videos;

        public Round(List<Video> videos, List<EndPoint> endPoints, List<Request> requests)
        {
            this.Videos = videos;
            this.EndPoints = endPoints;
            this.Requests = requests;
        }

        public static Request RoundFromFile(string input)
        {
            IEnumerable<int[]> inputs = from i in File.ReadAllLines(input) select (from j in i.Split(' ') select Convert.ToInt32(j, CultureInfo.InvariantCulture)).ToArray();

            int videosNumber = inputs.ElementAt(0)[0];
            int endpointNumber = inputs.ElementAt(0)[1];
            int requestNumber = inputs.ElementAt(0)[2];
            int cacheServerNumber = inputs.ElementAt(0)[3];
            int cacheServerCapacity = inputs.ElementAt(0)[4];

            int[] videoSizes = inputs.ElementAt(1);

            int videoIndex = 0;
            List<Video> videos = new List<Video>();
            foreach (int size in videoSizes)
            {
                Video video = new Video(videoIndex, size);
            }

            List<CacheServer> cacheServers = new List<CacheServer>();
            for (int cacheServerIndex = 0; cacheServerIndex < cacheServerNumber; cacheServerIndex++)
            {
                cacheServers.Add(new CacheServer(cacheServerIndex, cacheServerCapacity));
            }

            IEnumerator<int[]> nextLinesEnumerator = inputs.Skip(2).GetEnumerator();

            List<EndPoint> endpoints = new List<EndPoint>();

            int endPointIndex = 0;
            while (nextLinesEnumerator.MoveNext())
            {
                int[] firstLine = nextLinesEnumerator.Current;

                int dataCenterLatency = firstLine[0];
                int dataCenterNumber = firstLine[1];

                EndPoint endpoint = new EndPoint(endPointIndex, dataCenterLatency);

                List<Latency> latencies = new List<Latency>();

                for (int i = 0; i < dataCenterNumber; i++)
                {
                    nextLinesEnumerator.MoveNext();
                    int[] nextLine = nextLinesEnumerator.Current;

                    int cacheServerId = nextLine[0];
                    int cacheServerLatency = nextLine[1];

                    endpoint.CacheServerIds.Add(cacheServerId);
                    endpoint.CacheServerLatencies.Add(new Latency(cacheServerId, cacheServerLatency));
                }

                endpoints.Add(endpoint);
                endPointIndex++;
            }
        }
    }
}