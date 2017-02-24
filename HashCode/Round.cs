namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class Round
    {
        public List<CacheServer> CacheServers;
        public List<EndPoint> EndPoints;
        public List<Request> Requests;
        public List<Video> Videos;

        public Round(List<CacheServer> cacheServers, List<EndPoint> endPoints, List<Request> requests, List<Video> videos)
        {
            this.CacheServers = cacheServers;
            this.Videos = videos;
            this.EndPoints = endPoints;
            this.Requests = requests;
        }

        public static Round RoundFromFile(string input)
        {
            IEnumerable<int[]> inputs = from i in File.ReadAllLines(input) select (from j in i.Split(' ') select Convert.ToInt32(j, CultureInfo.InvariantCulture)).ToArray();

            int videosNumber = inputs.ElementAt(0)[0];
            int endpointNumber = inputs.ElementAt(0)[1];
            int requestNumber = inputs.ElementAt(0)[2];
            int cacheServerNumber = inputs.ElementAt(0)[3];
            int cacheServerCapacity = inputs.ElementAt(0)[4];

            Write.Trace($"videosNumber {videosNumber} endpointNumber {endpointNumber} requestNumber {requestNumber} cacheServerNumber {cacheServerNumber} cacheServerCapacity {cacheServerCapacity}");

            int[] videoSizes = inputs.ElementAt(1);

            int videoIndex = 0;

            List<Video> videos = new List<Video>();
            foreach (int size in videoSizes)
            {
                Write.Trace($"video {videoIndex} size : {size}");

                videos.Add(new Video(videoIndex, size));

                videoIndex++;
            }

            List<CacheServer> cacheServers = new List<CacheServer>();
            for (int cacheServerIndex = 0; cacheServerIndex < cacheServerNumber; cacheServerIndex++)
            {
                Write.Trace($"cache server {cacheServerIndex} capacity : {cacheServerCapacity}");

                cacheServers.Add(new CacheServer(cacheServerIndex, cacheServerCapacity));
            }

            IEnumerator<int[]> nextLinesEnumerator = inputs.Skip(2).GetEnumerator();

            List<EndPoint> endpoints = new List<EndPoint>();

            int endPointId = 0;
            while (nextLinesEnumerator.MoveNext())
            {
                int[] firstLine = nextLinesEnumerator.Current;

                // request contains 3 integer, while endpoint info 2
                if (firstLine.Count() > 2)
                {
                    break;
                }

                int dataCenterLatency = firstLine[0];
                int dataCenterNumber = firstLine[1];

                Write.Trace($"endpoint {endPointId} dataCenterLatency : {dataCenterLatency}");

                EndPoint endpoint = new EndPoint(endPointId, dataCenterLatency);

                List<Latency> latencies = new List<Latency>();

                for (int i = 0; i < dataCenterNumber; i++)
                {
                    nextLinesEnumerator.MoveNext();
                    int[] nextLine = nextLinesEnumerator.Current;

                    int cacheServerId = nextLine[0];
                    int cacheServerLatency = nextLine[1];

                    cacheServers[cacheServerId].EndPointID.Add(endPointId);

                    Write.Trace($"cacheserver {cacheServerId}, latency : {cacheServerLatency}");

                    endpoint.CacheServerLatencies.Add(new Latency(cacheServerId, cacheServerLatency));
                }

                endpoints.Add(endpoint);
                endPointId++;
            }

            List<Request> requests = new List<Request>();
            do
            {
                int[] currentLine = nextLinesEnumerator.Current;

                int videoId = currentLine[0];
                int endpointId = currentLine[1];
                int occurency = currentLine[2];

                Write.Trace($"new request endpointid {endpointId}, videoid : {videoId}, occurency : {occurency}");

                requests.Add(new Request(endpointId, videoId, occurency));
            }
            while (nextLinesEnumerator.MoveNext());

            return new Round(cacheServers, endpoints, requests, videos);
        }
    }
}