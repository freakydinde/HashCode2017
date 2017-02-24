namespace HashCode
{
    public class Latency
    {
        public int CacheServerID;
        public int Time;

        public Latency(int cacheServerID, int time)
        {
            this.CacheServerID = cacheServerID;
            this.Time = time;
        }
    }
}