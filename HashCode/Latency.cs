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

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        public override string ToString()
        {
            return Write.Invariant($"CacheServerID:{this.CacheServerID} Time:{this.Time}");
        }
    }
}