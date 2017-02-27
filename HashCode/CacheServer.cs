namespace HashCode
{
    using System.Collections.Generic;

    public class CacheServer
    {
        public int CurrentSize;
        public List<int> EndPointID;
        public List<GainCacheServer> GainCacheServers;
        public int ID;
        public bool IsFull;
        public int MaxSize;
        public bool RefusedLastAssigment;
        public List<int> VideosID;

        public CacheServer(int id, int maxSize)
        {
            this.ID = id;
            this.MaxSize = maxSize;

            Reset();
        }

        public bool AssignVideo(Video video)
        {
            if (video != null)
            {
                if (this.CurrentSize + video.Size <= this.MaxSize)
                {
                    this.CurrentSize += video.Size;
                    this.VideosID.Add(video.ID);

                    return true;
                }
                else
                {
                    this.RefusedLastAssigment = true;

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            VideosID = new List<int>();
            EndPointID = new List<int>();

            GainCacheServers = new List<GainCacheServer>();

            this.CurrentSize = 0;

            this.IsFull = false;
            this.RefusedLastAssigment = false;
        }
    }
}