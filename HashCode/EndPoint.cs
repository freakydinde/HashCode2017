namespace HashCode
{
    using System.Collections.Generic;

    public class EndPoint
    {
        public int ID;
        public List<int> CacheServerIds;
        public List<Latency> CacheServerLatencies;
        public int DataCenterLatency;

        public EndPoint(int id, int dataCenterLatency)
        {
            this.ID = id;
            this.DataCenterLatency = dataCenterLatency;

            this.CacheServerIds = new List<int>();
            this.CacheServerLatencies = new List<Latency>();
        }
    }
}