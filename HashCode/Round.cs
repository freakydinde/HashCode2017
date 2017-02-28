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
        public List<EndPoint> EndPoints;
        public List<Request> Requests;
        public List<Video> Videos;

        public int Score;
        public int CacheServersCapacity;

        /// <summary>flag : has dispose already been called</summary>
        private bool disposed = false;

        public Round(List<CacheServer> cacheServers, List<EndPoint> endPoints, List<Request> requests, List<Video> videos, int cacheServersCapacity)
        {
            this.CacheServers = cacheServers;
            this.Videos = videos;
            this.EndPoints = endPoints;
            this.Requests = requests;
            this.CacheServersCapacity = cacheServersCapacity;
        }

        public void TraceScore(bool resetWatch = true, bool console = true)
        {
            Write.TraceWatch($"score : {this.Score}, process time", resetWatch, console);
        }

        /// <summary>Finalizes an instance of the <see cref="MyClass"/> class.</summary>
        ~Round()
        {
            this.Dispose(false);
        }

        public enum AssignMode
        {
            Standard = 2,
            PreProcessing = 4,
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

                    cacheServers[cacheServerId].EndPointID.Add(endPointId);

                    Write.Trace($"cacheserver {cacheServerId}, latency : {cacheServerLatency}");

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

        public void AssignVideos(AssignMode mode)
        {
            Write.TraceWatch($"Assign Videos mode:{mode.ToString()}", true);

            // init cache servers
            foreach (CacheServer cacheServer in this.CacheServers)
            {
                cacheServer.Reset();
            }

            switch (mode)
            {
                case AssignMode.Standard:

                    AssignStandard();

                    break;

                case AssignMode.PreProcessing:

                    AssignPreProcessing();

                    break;
            }
        }

        private void AssignPreProcessing()
        {
            Write.TraceWatch($"build gain cache servers list", true);

            foreach (Request request in this.Requests)
            {
                EndPoint endPoint = (from i in this.EndPoints where i.ID == request.EndPointID select i).FirstOrDefault();
                Video video = (from i in this.Videos where i.ID == request.VideoID select i).FirstOrDefault();

                GainCacheServer bestGain = new GainCacheServer(0, 0, 0, 0);
                int cacheServerID = 0;

                foreach (Latency latency in endPoint.CacheServerLatencies)
                {
                    int gain = endPoint.DataCenterLatency - latency.Time;

                    GainCacheServer gainCacheServer = new GainCacheServer(endPoint.ID, gain, video.ID, video.Size);

                    if (gainCacheServer.GainPerMegaByte > bestGain.GainPerMegaByte)
                    {
                        bestGain = gainCacheServer;
                        cacheServerID = latency.CacheServerID;
                    }
                }

                GetCacheServer(cacheServerID).GainCacheServers.Add(bestGain);
            }

            Write.TraceWatch($"flatten grouped gain cache servers list", true);

            List<GainCacheServer> gainCacheServers = (from i in this.CacheServers.SelectMany(x => x.GainCacheServers).OrderByDescending(y => y.GainPerMegaByte) group i by new { i.EndPointID, i.VideoID } into grp select grp.First()).ToList();

            Write.TraceWatch($"release gainCacheServer object from cacheServers", true);

            foreach (CacheServer cacheServer in this.CacheServers)
            {
                cacheServer.GainCacheServers.Clear();
            }

            Write.TraceWatch($"build video assigment", true);

            int cachedGain = 0;

            foreach (GainCacheServer gainCacheServer in gainCacheServers)
            {
                IEnumerable<CacheServer> endpointCacheServers = from i in this.CacheServers where i.EndPointID.Where(x => x == gainCacheServer.EndPointID).Any() select i;

                // test if another cache server linked to the same endpoint already host video
                if (!endpointCacheServers.Where(x => x.VideoIsHosted(gainCacheServer.VideoID)).Any())
                {
                    IEnumerator<CacheServer> cacheServers = (from i in endpointCacheServers orderby i.EndPointID.Count(), i.RemainingSize descending select i).GetEnumerator();

                    Video video = this.Videos.Where(x => x.ID == gainCacheServer.VideoID).FirstOrDefault();

                    while (cacheServers.MoveNext())
                    {
                        if (cacheServers.Current.AssignVideo(video))
                        {
                            cachedGain += gainCacheServer.Gain;

                            break;
                        }
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

            this.Score = cachedGain * 1000 / this.Requests.Count();
        }

        private void AssignStandard()
        {
            Write.TraceWatch($"build gain cache servers list", true);

            foreach (Request request in this.Requests)
            {
                EndPoint endPoint = (from i in this.EndPoints where i.ID == request.EndPointID select i).FirstOrDefault();
                Video video = (from i in this.Videos where i.ID == request.VideoID select i).FirstOrDefault();

                if (video.Size <= this.CacheServersCapacity)
                {
                    foreach (Latency latency in endPoint.CacheServerLatencies)
                    {
                        int gain = endPoint.DataCenterLatency - latency.Time;

                        GainCacheServer gainCacheServer = new GainCacheServer(endPoint.ID, gain, video.ID, video.Size);

                        Write.Trace(gainCacheServer.ToString());

                        GetCacheServer(latency.CacheServerID).GainCacheServers.Add(gainCacheServer);
                    }
                }
            }

            Write.TraceWatch($"flatten grouped gain cache servers list", true);

            List<GainCacheServer> gainCacheServers = (from i in this.CacheServers.SelectMany(x => x.GainCacheServers).OrderByDescending(y => y.GainPerMegaByte) group i by new { i.EndPointID, i.VideoID } into grp select grp.First()).ToList();

            Write.Trace("gain cache servers grouped", gainCacheServers, Environment.NewLine);

            Write.TraceWatch($"release gainCacheServer object from cacheServers", true);

            foreach (CacheServer cacheServer in this.CacheServers)
            {
                cacheServer.GainCacheServers.Clear();
            }

            Write.TraceWatch($"build video assigment", true);

            int cachedGain = 0;

            foreach (GainCacheServer gainCacheServer in gainCacheServers)
            {
                IEnumerable<CacheServer> endpointCacheServers = from i in this.CacheServers where i.EndPointID.Where(x => x == gainCacheServer.EndPointID).Any() select i;

                // test if another cache server linked to the same endpoint already host video
                if (!endpointCacheServers.Where(x => x.VideoIsHosted(gainCacheServer.VideoID)).Any())
                {
                    IEnumerator<CacheServer> cacheServers = (from i in endpointCacheServers orderby i.EndPointID.Count(), i.RemainingSize descending select i).GetEnumerator();

                    Video video = this.Videos.Where(x => x.ID == gainCacheServer.VideoID).FirstOrDefault();

                    while (cacheServers.MoveNext())
                    {
                        if (cacheServers.Current.AssignVideo(video))
                        {
                            cachedGain += gainCacheServer.Gain;

                            break;
                        }
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

            this.Score = cachedGain * 1000 / this.Requests.Count;
        }

        /// <summary>Public implementation of Dispose pattern, release unmanaged & managed resources.</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public CacheServer GetCacheServer(int id)
        {
            return (from i in this.CacheServers where i.ID == id select i).FirstOrDefault();
        }

        public void PrintAssigment(string outputFile)
        {
            Write.TraceWatch("printing videos assigment", true, true);

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
            this.EndPoints = null;
            this.Requests = null;
            this.Videos = null;

            // set the has disposed flag to true
            this.disposed = true;
        }
    }
}