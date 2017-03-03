namespace HashCode
{
    public class GainCacheServer
    {
        public int CacheServerID;
        public int EndPointID;
        public int GainPerMegaByte;

        public GainCacheServer(int endPointID, int cacheServerID, int gainPerMegaByte)
        {
            this.EndPointID = endPointID;
            this.CacheServerID = cacheServerID;
            this.GainPerMegaByte = gainPerMegaByte;
        }
    }
}