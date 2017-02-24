namespace HashCode
{
    using System.Collections.Generic;

    public class EndPoint
    {
        public List<Latency> CacheServerLatencies;
        public int DataCenterLatency;
        public int ID;

        public EndPoint(int id, int dataCenterLatency)
        {
            this.ID = id;
            this.DataCenterLatency = dataCenterLatency;
            this.CacheServerLatencies = new List<Latency>();
        }
    }
}