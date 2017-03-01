namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Round : IDisposable
    {
        public List<CacheServer> CacheServers;
        public int CacheServersCapacity;
        public List<EndPoint> EndPoints;
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

            this.Score = 0;
        }

        /// <summary>Finalizes an instance of the <see cref="Round"/> class.</summary>
        ~Round()
        {
            this.Dispose(false);
        }

        public bool ComputeScore(string input)
        {
            IEnumerable<int[]> outputs = Read.NumberLines(input);

            this.Score = 0;

            int requestsNumber = this.Requests.Count();
            int usedCacheServersNumber = outputs.ElementAt(0)[0];

            Write.Trace($"requestsNumber:{requestsNumber} usedCacheServersNumber:{usedCacheServersNumber}");

            foreach (int[] line in outputs.Skip(1))
            {
                CacheServer cacheServer = (from i in this.CacheServers where i.ID == line[0] select i).FirstOrDefault();

                foreach (int videoID in line.Skip(1))
                {
                    Video video = (from i in this.Videos where i.ID == videoID select i).FirstOrDefault();

                    if (cacheServer.AssignVideo(videoID, video.Size))
                    {
                        foreach (Request request in from i in this.Requests where i.VideoID == videoID select i)
                        {
                            EndPoint endPoint = (from i in this.EndPoints where i.ID == request.EndPointID select i).FirstOrDefault();

                            int latency = (from i in endPoint.CacheServerLatencies where i.CacheServerID == cacheServer.ID select i.Time).FirstOrDefault();

                            this.Score += latency * request.Occurency;
                        }
                    }
                    else
                    {
                        if (cacheServer.IsVideoHost(videoID))
                        {
                            Write.Trace($"Error: Video {videoID} is already hosted on cache server {cacheServer.ID}", true);
                        }
                        else
                        {
                            Write.Trace($"Error: Video {videoID} does not fit in the cache server {cacheServer.ID}", true);
                        }

                        return false;
                    }
                }
            }

            this.Score = Math.Round(this.Score * 1000 / requestsNumber);

            return true;
        }

        public static Round RoundFromFile(string input)
        {
            Write.TraceWatch("start RoundFromFile", true);

            IEnumerable<int[]> inputs = Read.NumberLines(input);

            int videosNumber = inputs.ElementAt(0)[0];
            int endpointNumber = inputs.ElementAt(0)[1];
            int requestNumber = inputs.ElementAt(0)[2];
            int cacheServerNumber = inputs.ElementAt(0)[3];
            int cacheServerCapacity = inputs.ElementAt(0)[4];

            Write.Trace($"videosNumber {videosNumber} endpointNumber {endpointNumber} requestNumber {requestNumber} cacheServerNumber {cacheServerNumber} cacheServerCapacity {cacheServerCapacity}", true);

            Write.TraceWatch("building objects", true);

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

            Write.TraceWatch("reading requests", true);

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
            this.AssignVideosBestGain();
            this.AssignVideosLeft();
        }

        public void AssignVideosBestGain()
        {
            Write.TraceWatch("AssignVideosBestGain : build gain cache servers list", true);

            List<GainCacheServer> gainCacheServers = new List<GainCacheServer>();

            foreach (Request request in this.Requests)
            {
                EndPoint endPoint = (from i in this.EndPoints where i.ID == request.EndPointID select i).FirstOrDefault();
                Video video = (from i in this.Videos where i.ID == request.VideoID select i).FirstOrDefault();

                GainCacheServer bestGain = new GainCacheServer(0, 0, 0, 0, 0);
                int cacheServerID = 0;

                if (video.Size <= this.CacheServersCapacity)
                {
                    foreach (Latency latency in endPoint.CacheServerLatencies)
                    {
                        int gain = (endPoint.DataCenterLatency - latency.Time) * request.Occurency;
                        GainCacheServer gainCacheServer = new GainCacheServer(endPoint.ID, latency.CacheServerID, gain, video.ID, video.Size);

                        if (gainCacheServer.GainPerMegaByte > bestGain.GainPerMegaByte)
                        {
                            bestGain = gainCacheServer;
                            cacheServerID = latency.CacheServerID;
                        }
                    }

                    gainCacheServers.Add(bestGain);
                }
            }

            ProcessAssign(gainCacheServers);
        }

        public void AssignVideosLeft()
        {
            Write.TraceWatch("AssignVideos : build gain cache servers list", true);

            List<GainCacheServer> gainCacheServers = new List<GainCacheServer>();

            foreach (Request request in this.Requests)
            {
                IEnumerable<CacheServer> endPointVideoHost = from i in this.CacheServers where i.IsConnectedToEndPoint(request.EndPointID) && i.IsVideoHost(request.VideoID) select i;

                if (!endPointVideoHost.Any())
                {
                    EndPoint endPoint = (from i in this.EndPoints where i.ID == request.EndPointID select i).FirstOrDefault();
                    Video video = (from i in this.Videos where i.ID == request.VideoID select i).FirstOrDefault();

                    if (video.Size <= this.CacheServersCapacity)
                    {
                        foreach (Latency latency in endPoint.CacheServerLatencies)
                        {
                            int gain = (endPoint.DataCenterLatency - latency.Time) * request.Occurency;

                            GainCacheServer gainCacheServer = new GainCacheServer(endPoint.ID, latency.CacheServerID, gain, video.ID, video.Size);

                            gainCacheServers.Add(gainCacheServer);
                        }
                    }
                }
            }

            ProcessAssign(gainCacheServers);
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

            // set the has disposed flag to true
            this.disposed = true;
        }

        private void ProcessAssign(List<GainCacheServer> gainCacheServers)
        {
            Write.TraceWatch("assign videos to cache servers");

            foreach (GainCacheServer gainCacheServer in from i in gainCacheServers.OrderByDescending(y => y.GainPerMegaByte) group i by new { i.EndPointID, i.VideoID } into grp select grp.First())
            {
                Write.TraceWatch($"processing gainCacheServer {gainCacheServer.ToString()}");

                IEnumerable<CacheServer> endpointCacheServers = from i in this.CacheServers where i.IsConnectedToEndPoint(gainCacheServer.EndPointID) select i;

                IEnumerable<CacheServer> endPointVideoHost = endpointCacheServers.Where(x => x.IsVideoHost(gainCacheServer.VideoID));

                if (!endPointVideoHost.Any())
                {
                    IEnumerator<CacheServer> cacheServers = (from i in endpointCacheServers orderby i.RemainingSize descending select i).GetEnumerator();

                    while (cacheServers.MoveNext())
                    {
                        if (cacheServers.Current.AssignVideo(gainCacheServer.VideoID, gainCacheServer.VideoSize))
                        {
                            this.Score += gainCacheServer.Gain;

                            Write.TraceWatch($"score += {gainCacheServer.Gain} = {this.Score}");

                            break;
                        }
                    }
                }
                else
                {
                    Write.Trace($"video id {gainCacheServer.VideoID} is already hosted on a endpoint cache server");
                }
            }
        }
    }
}