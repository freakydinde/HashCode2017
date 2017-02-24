namespace HashCode
{
    using System.Collections.Generic;

    public class CacheServer
    {
        public int CurrentSize;
        public int ID;
        public int MaxSize;

        public List<int> EndPointID;
        public List<int> VideosID;

        public CacheServer(int id, int maxSize)
        {
            this.ID = id;
            this.MaxSize = maxSize;
            this.CurrentSize = 0;

            VideosID = new List<int>();
            EndPointID = new List<int>();
        }
    }
}