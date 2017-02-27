namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

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

        public void AssignVideos()
        {
            this.SetVideosList();

            Write.TraceWatch("starting assign videos list");

            Write.TraceVisible("gain cache servers");

            foreach (GainCacheServer gainCacheServer in from i in this.CacheServers.SelectMany(x => x.GainCacheServers).OrderByDescending(y => y.GainPerMegaByte) group i by new { i.EndPointID, i.VideoID } into grp select grp.First())
            {
                Write.TraceWatch($"\r\nprocessing gainCacheServer {gainCacheServer.ToString()}");

                IEnumerable<CacheServer> endpointCacheServers = from i in this.CacheServers where i.EndPointID.Where(x => x == gainCacheServer.EndPointID).Any() select i;

                // test if another cache server linked to the same endpoint already host video
                if (!endpointCacheServers.Where(x => x.VideoIsHosted(gainCacheServer.VideoID)).Any())
                {
                    Write.Trace("\r\ncollection : ");

                    Write.TraceCollection((from i in endpointCacheServers orderby i.EndPointID.Count(), i.RemainingSize descending select i), "\r\n");

                    IEnumerator<CacheServer> cacheServers = (from i in endpointCacheServers orderby i.EndPointID.Count(), i.RemainingSize descending select i).GetEnumerator();

                    Video video = this.Videos.Where(x => x.ID == gainCacheServer.VideoID).FirstOrDefault();

                    while (cacheServers.MoveNext())
                    {
                        if (cacheServers.Current.AssignVideo(video)) break;
                    }
                }
                else
                {
                    Write.Trace($"video id {gainCacheServer.VideoID} is already hosted on a endpoint cache server");
                }

                if (this.CacheServers.Where(x => x.RefusedLastAssigment == false).Count() == 0)
                {
                    break;
                }
            }
        }

        public CacheServer GetCacheServer(int id)
        {
            return (from i in this.CacheServers where i.ID == id select i).FirstOrDefault();
        }

        public void PrintAssigment(string outputFile)
        {
            Write.TraceWatch("starting print videos assigment");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.CacheServers.Where(x => x.VideosID.Any()).Count().ToString());

            foreach (CacheServer cacheServer in this.CacheServers)
            {
                if (cacheServer.VideosID.Any())
                {
                    sb.Append(cacheServer.ID + " ");

                    foreach (int id in cacheServer.VideosID)
                    {
                        sb.Append(id.ToString() + " ");
                    }

                    sb.AppendLine();
                }
            }

            if (File.Exists(outputFile)) File.Delete(outputFile);

            File.WriteAllText(outputFile, sb.ToString());
        }

        public void SetVideosList()
        {
            Write.ResetWatch();
            Write.Trace("starting set videos list");

            // init cache servers
            foreach (CacheServer cacheServer in this.CacheServers)
            {
                cacheServer.Reset();
            }

            Write.TraceWatch("build extended request");

            foreach (Request request in this.Requests)
            {
                EndPoint endPoint = (from i in this.EndPoints where i.ID == request.EndPointID select i).FirstOrDefault();
                Video video = (from i in this.Videos where i.ID == request.VideoID select i).FirstOrDefault();

                foreach (Latency latency in endPoint.CacheServerLatencies)
                {
                    int gain = endPoint.DataCenterLatency - latency.Time;

                    GetCacheServer(latency.CacheServerID).GainCacheServers.Add(new GainCacheServer(endPoint.ID, gain, video.ID, video.Size));
                }
            }

            Write.TraceWatch("end set videos List");
        }
    }
}