namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Round : IDisposable
    {
        public List<CacheServer> CacheServers;
        public int CacheServersCapacity;
        public List<EndPoint> EndPoints;
        public List<GainCacheServer> GainCacheServers;
        public List<Request> Requests;
        public double Score;
        public List<Video> Videos;

        /// <summary>flag : has dispose already been called</summary>
        private bool disposed = false;

        public Round(List<CacheServer> cacheServers, List<EndPoint> endPoints, List<Request> requests, List<Video> videos, int cacheServersCapacity)
        {
            this.CacheServers = cacheServers;
            this.Videos = videos;
            this.EndPoints = endPoints;
            this.Requests = requests;
            this.CacheServersCapacity = cacheServersCapacity;
            this.GainCacheServers = new List<GainCacheServer>();
        }

        /// <summary>Finalizes an instance of the <see cref="MyClass"/> class.</summary>
        ~Round()
        {
            this.Dispose(false);
        }

        public static Round RoundFromFile(string input)
        {
            Write.TraceWatch($"start RoundFromFile", true);

            IEnumerable<int[]> inputs = from i in File.ReadAllLines(input) select (from j in i.Split(' ') select Convert.ToInt32(j, CultureInfo.InvariantCulture)).ToArray();

            int videosNumber = inputs.ElementAt(0)[0];
            int endpointNumber = inputs.ElementAt(0)[1];
            int requestNumber = inputs.ElementAt(0)[2];
            int cacheServerNumber = inputs.ElementAt(0)[3];
            int cacheServerCapacity = inputs.ElementAt(0)[4];

            Write.Trace($"videosNumber {videosNumber} endpointNumber {endpointNumber} requestNumber {requestNumber} cacheServerNumber {cacheServerNumber} cacheServerCapacity {cacheServerCapacity}", true);

            Write.TraceWatch($"building objects", true);

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

                    cacheServers[cacheServerId].EndPointIds.Add(endPointId);

                    Write.Trace($"cacheserver {cacheServerId}, latency : {cacheServerLatency}");

                    endpoint.CacheServerIds.Add(cacheServerId);
                    endpoint.CacheServerLatencies.Add(new Latency(cacheServerId, cacheServerLatency));
                }

                endpoints.Add(endpoint);
                endPointId++;
            }

            Write.TraceWatch($"reading requests", true);

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

            return new Round(cacheServers, endpoints, requests, videos, cacheServerCapacity);
        }

        public void AssignVideos()
        {
            Write.TraceWatch("build gain cache servers list", true);

            foreach (Request request in this.Requests)
            {
                EndPoint endPoint = (from i in this.EndPoints where i.ID == request.EndPointID select i).FirstOrDefault();
                Video video = (from i in this.Videos where i.ID == request.VideoID select i).FirstOrDefault();

                if (video.Size <= this.CacheServersCapacity)
                {
                    foreach (Latency latency in endPoint.CacheServerLatencies)
                    {
                        int gain = (endPoint.DataCenterLatency - latency.Time) * request.Occurency;

                        GainCacheServer gainCacheServer = new GainCacheServer(endPoint.ID, latency.CacheServerID, gain, video.ID, video.Size);

                        this.GainCacheServers.Add(gainCacheServer);
                    }
                }
            }

            Write.TraceWatch("assign videos to cache servers");
            double cachedGain = 0;

            foreach (GainCacheServer gainCacheServer in this.GainCacheServers.OrderByDescending(x => x.GainPerMegaByte))
            {
                Write.Trace($"gain cache servers : {gainCacheServer}");

                IEnumerable<CacheServer> endpointCacheServers = from i in this.CacheServers where i.EndPointIds.Where(x => x == endpoint.ID).Any() select i;

                List<int> gainCacheServerIndex = new List<int>();

                foreach (GainCacheServer gainCacheServer in from i in this.GainCacheServers orderby i.GainPerMegaByte descending group i by i.VideoID into grp select grp.First())
                {
                    int index = this.GainCacheServers.IndexOf(gainCacheServer);

                    if (!this.VideoIsCached(endpointCacheServers, gainCacheServer.VideoID))
                    {
                        Video video = this.Videos.Where(x => x.ID == gainCacheServer.VideoID).FirstOrDefault();
                        CacheServer cacheServer = this.CacheServers.Where(x => x.ID == gainCacheServer.CacheServerID).FirstOrDefault();

                        if (cacheServer.AssignVideo(video))
                        {
                            gainCacheServerIndex.Add(index);

                            cachedGain += gainCacheServer.Gain;
                            Write.Trace($"cachedGain += {gainCacheServer.Gain} = {cachedGain}");
                        }
                    }
                    else
                    {
                        gainCacheServerIndex.Add(index);
                    }
                }

                // remove cached videos
                int intendIndex = 0;

                foreach (int index in gainCacheServerIndex.OrderBy(x => x))
                {
                    this.GainCacheServers.RemoveAt(index - intendIndex);
                    intendIndex++;
                }
            }

            this.Score = Math.Round(cachedGain * 1000 / this.Requests.Sum(x => x.Occurency), 0);
        }

        /// <summary>Public implementation of Dispose pattern, release unmanaged & managed resources.</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void PrintAssigment(string outputFile)
        {
            Write.TraceWatch("printing videos assigment", true);

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

        public void TraceScore(bool resetWatch = true, bool console = true)
        {
            Write.TraceWatch($"score : {this.Score}, process time", resetWatch, console);
        }

        public bool VideoIsCached(IEnumerable<CacheServer> endpointCacheServers, int videoID)
        {
            return (from i in endpointCacheServers where i.VideoIsHosted(videoID) select i).Any();
        }

        /// <summary>Protected implementation of Dispose pattern</summary>
        /// <param name="disposing">should i dispose managed object</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Free any managed objects here.
                }
            }

            // Free any unmanaged objects here.
            this.CacheServers = null;
            this.Requests = null;
            this.Videos = null;
            this.EndPoints = null;
            this.GainCacheServers = null;

            // set the has disposed flag to true
            this.disposed = true;
        }
    }
}