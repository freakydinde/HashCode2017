namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EndPoint
    {
        public List<Latency> CacheServerLatencies;
        public List<int> CacheServerIds;
        public int DataCenterLatency;
        public int ID;

        public EndPoint(int id, int dataCenterLatency)
        {
            this.ID = id;
            this.DataCenterLatency = dataCenterLatency;
            this.CacheServerLatencies = new List<Latency>();
            this.CacheServerIds = new List<int>();
        }

        public bool IsConnectedToCacheServer(int cacheServerID)
        {
            return this.CacheServerIds.Where(x => x == cacheServerID).Any();
        }
    }
}