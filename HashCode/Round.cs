namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Round : IDisposable
    {
        public Dictionary<int, CacheServer> CacheServers;
        public int CacheServersCapacity;
        public Dictionary<int, EndPoint> EndPoints;
        public List<Request> Requests;
        public double Score;
        public Dictionary<int, Video> Videos;

        /// <summary>flag : has dispose already been called</summary>
        private bool disposed = false;

        public Round(Dictionary<int, CacheServer> cacheServers, Dictionary<int, EndPoint> endPoints, List<Request> requests, Dictionary<int, Video> videos, int cacheServersCapacity)
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

        public static Round RoundFromFile(string input)
        {
            Write.ResetWatch();
            Write.TraceInline("Read input file", true);

            IEnumerable<int[]> inputs = Read.NumberLines(input);

            int videosNumber = inputs.ElementAt(0)[0];
            int endpointNumber = inputs.ElementAt(0)[1];
            int requestNumber = inputs.ElementAt(0)[2];
            int cacheServerNumber = inputs.ElementAt(0)[3];
            int cacheServerCapacity = inputs.ElementAt(0)[4];

            Write.Trace($"videosNumber {videosNumber} endpointNumber {endpointNumber} requestNumber {requestNumber} cacheServerNumber {cacheServerNumber} cacheServerCapacity {cacheServerCapacity}");

            int[] videoSizes = inputs.ElementAt(1);

            int videoIndex = 0;

            Dictionary<int, Video> videos = new Dictionary<int, Video>();
            foreach (int size in videoSizes)
            {
                videos.Add(videoIndex, new Video(size));

                videoIndex++;
            }

            Dictionary<int, CacheServer> cacheServers = new Dictionary<int, CacheServer>();
            for (int cacheServerIndex = 0; cacheServerIndex < cacheServerNumber; cacheServerIndex++)
            {
                cacheServers.Add(cacheServerIndex, new CacheServer(cacheServerCapacity));
            }

            IEnumerator<int[]> nextLinesEnumerator = inputs.Skip(2).GetEnumerator();

            Dictionary<int, EndPoint> endpoints = new Dictionary<int, EndPoint>();

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

                EndPoint endpoint = new EndPoint(dataCenterLatency);

                Dictionary<int, int> latencies = new Dictionary<int, int>();

                for (int i = 0; i < dataCenterNumber; i++)
                {
                    nextLinesEnumerator.MoveNext();
                    int[] nextLine = nextLinesEnumerator.Current;

                    int cacheServerId = nextLine[0];
                    int cacheServerLatency = nextLine[1];

                    cacheServers[cacheServerId].EndPointIds.Add(endPointId);

                    endpoint.CacheServerIds.Add(cacheServerId);
                    endpoint.CacheServerLatencies.Add(cacheServerId, cacheServerLatency);
                }

                endpoints.Add(endPointId, endpoint);
                endPointId++;
            }

            List<Request> requests = new List<Request>();
            do
            {
                int[] currentLine = nextLinesEnumerator.Current;

                int videoId = currentLine[0];
                int endpointId = currentLine[1];
                int occurency = currentLine[2];

                requests.Add(new Request(endpointId, videoId, occurency));
            }
            while (nextLinesEnumerator.MoveNext());

            Write.TraceWatch(" : ");

            return new Round(cacheServers, endpoints, requests, videos, cacheServerCapacity);
        }

        public void AssignVideos()
        {
            this.AssignVideosBestGain();
            this.AssignVideosLeft();
        }

        public void AssignVideosBestGain()
        {
            Write.ResetWatch();
            Write.TraceInline("AssignVideos BestGain : build gain cache servers list", true);

            List<GainCacheServer> gainCacheServers = new List<GainCacheServer>();

            foreach (Request request in this.Requests)
            {
                EndPoint endPoint = this.EndPoints[request.EndPointID];

                if (endPoint.CacheServerIds.Any())
                {
                    Video video = this.Videos[request.VideoID];

                    GainCacheServer bestGain = new GainCacheServer(0, 0, 0, 0, 0);
                    int bestCacheServerID = 0;

                    if (video.Size <= this.CacheServersCapacity)
                    {
                        foreach (int cacheServerID in endPoint.CacheServerIds)
                        {
                            int gain = (endPoint.DataCenterLatency - endPoint.CacheServerLatencies[cacheServerID]) * request.Occurency;
                            GainCacheServer gainCacheServer = new GainCacheServer(request.EndPointID, cacheServerID, gain, request.VideoID, video.Size);

                            if (gainCacheServer.GainPerMegaByte >= bestGain.GainPerMegaByte)
                            {
                                bestGain = gainCacheServer;
                                bestCacheServerID = cacheServerID;
                            }
                        }

                        gainCacheServers.Add(bestGain);
                    }
                    else
                    {
                        Write.Trace($"{video} size exceed max size {this.CacheServersCapacity}");
                    }
                }
                else
                {
                    Write.Trace($"endpoint {request.EndPointID} is not connected to any cache servers");
                }
            }

            Write.TraceWatch(" : ");

            ProcessAssign(from i in gainCacheServers.OrderByDescending(y => y.GainPerMegaByte) group i by new { i.VideoID, i.EndPointID } into grp orderby grp.Count() select grp.First());
        }

        public void AssignVideosLeft()
        {
            Write.ResetWatch();
            Write.TraceInline("AssignVideos Left : build gain cache servers list", true);

            List<GainCacheServer> gainCacheServers = new List<GainCacheServer>();

            foreach (Request request in this.Requests)
            {
                EndPoint endPoint = this.EndPoints[request.EndPointID];

                IEnumerable<CacheServer> endPointCacheServer = from i in endPoint.CacheServerIds select this.CacheServers[i];

                if (endPointCacheServer.Any() && !endPointCacheServer.Where(x => x.IsVideoHost(request.VideoID)).Any())
                {
                    Video video = this.Videos[request.VideoID];

                    if (video.Size <= this.CacheServersCapacity)
                    {
                        foreach (int cacheServerID in endPoint.CacheServerIds)
                        {
                            int gain = (endPoint.DataCenterLatency - endPoint.CacheServerLatencies[cacheServerID]) * request.Occurency;
                            GainCacheServer gainCacheServer = new GainCacheServer(request.EndPointID, cacheServerID, gain, request.VideoID, video.Size);

                            gainCacheServers.Add(gainCacheServer);
                        }
                    }
                    else
                    {
                        Write.Trace($"{video} size exceed max size {this.CacheServersCapacity}");
                    }
                }
                else
                {
                    if (!endPointCacheServer.Any())
                    {
                        Write.Trace($"endpoint {request.EndPointID} is not connected to any cache servers");
                    }
                    else if (endPointCacheServer.Where(x => x.IsVideoHost(request.VideoID)).Any())
                    {
                        Write.Trace($"video {request.VideoID} is already hosted on another endpoint cache server");
                    }
                }
            }

            Write.TraceWatch(" : ");

            ProcessAssign(gainCacheServers.OrderByDescending(y => y.GainPerMegaByte));
        }

        public bool ComputeScore(string input)
        {
            Write.ResetWatch();
            Write.TraceInline("Compute score", true);

            IEnumerable<int[]> outputs = Read.NumberLines(input);

            this.Score = 0;
            int requestsNumber = this.Requests.Sum(x => x.Occurency);

            int usedCacheServersNumber = outputs.ElementAt(0)[0];

            foreach (CacheServer cacheServer in this.CacheServers.Values)
            {
                cacheServer.Reset();
            }

            foreach (int[] line in outputs.Skip(1))
            {
                int cacheServerID = line[0];

                CacheServer cacheServer = this.CacheServers[cacheServerID];

                foreach (int videoID in line.Skip(1))
                {
                    Video video = this.Videos[videoID];

                    if (cacheServer.AssignVideo(videoID, video.Size))
                    {
#warning optimisation needed
                        foreach (Request request in from i in this.Requests where i.VideoID == videoID select i)
                        {
                            EndPoint endPoint = this.EndPoints[request.EndPointID];

                            if (endPoint.IsConnectedToCacheServer(cacheServerID))
                            {
                                int cacheServerLatency = endPoint.CacheServerLatencies[cacheServerID];
                                int gain = (endPoint.DataCenterLatency - cacheServerLatency) * request.Occurency;

                                this.Score += gain;
                            }
                        }
                    }
                    else
                    {
                        Write.TraceVisible($"ERROR, video {videoID} to datacacheServer {cacheServerID} (alreadyHosted or cacheServer full, ouput is INVALID");
                    }
                }
            }

            this.Score = Math.Round(this.Score * 1000 / requestsNumber);

            Write.TraceWatch(" : ");

            return true;
        }

        /// <summary>Public implementation of Dispose pattern, release unmanaged & managed resources.</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void PrintAssigment(string outputFile)
        {
            Write.ResetWatch();
            Write.TraceInline("printing videos assigment", true);

#warning optimisation needed
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.CacheServers.Values.Where(x => x.VideoIds.Any()).Count().ToString());

            foreach (KeyValuePair<int, CacheServer> cacheServer in this.CacheServers)
            {
                if (cacheServer.Value.VideoIds.Any())
                {
                    sb.Append(cacheServer.Key + " ");

                    foreach (int id in cacheServer.Value.VideoIds)
                    {
                        sb.Append(id.ToString() + " ");
                    }

                    sb.AppendLine();
                }
            }

            if (File.Exists(outputFile)) File.Delete(outputFile);

            File.WriteAllText(outputFile, sb.ToString());

            Write.TraceWatch(" : ");
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

        private void ProcessAssign(IEnumerable<GainCacheServer> gainCacheServers)
        {
            Write.ResetWatch();
            Write.TraceInline("process assign videos to cache servers", true);

            foreach (GainCacheServer gainCacheServer in gainCacheServers)
            {
                CacheServer cacheServer = this.CacheServers[gainCacheServer.CacheServerID];
                cacheServer.AssignVideo(gainCacheServer.VideoID, gainCacheServer.VideoSize);
            }

            Write.TraceWatch(" : ");
        }
    }
}