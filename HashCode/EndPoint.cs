namespace HashCode
{
    using System.Collections.Generic;
    using System.Linq;

    public class EndPoint
    {
        public List<int> CacheServerIds;
        public Dictionary<int, int> CacheServerLatencies;
        public int DataCenterLatency;

        public EndPoint(int dataCenterLatency)
        {
            this.DataCenterLatency = dataCenterLatency;
            this.CacheServerLatencies = new Dictionary<int, int>();
            this.CacheServerIds = new List<int>();
        }

        public bool IsConnectedToCacheServer(int cacheServerID)
        {
            return this.CacheServerIds.Where(x => x == cacheServerID).Any();
        }
    }
}