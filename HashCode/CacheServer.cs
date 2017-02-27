namespace HashCode
{
    using System.Collections.Generic;
    using System.Linq;

    public class CacheServer
    {
        public int CurrentSize;
        public List<int> EndPointID;
        public List<GainCacheServer> GainCacheServers;
        public int ID;
        public bool IsFull;
        public int MaxSize;
        public bool RefusedLastAssigment;
        public int RemainingSize;
        public List<int> VideosID;

        public CacheServer(int id, int maxSize)
        {
            this.ID = id;
            this.MaxSize = maxSize;
            this.EndPointID = new List<int>();

            Reset();
        }

        /// <summary>Assign video to cache server, test if video is already assigned and if server is not full</summary>
        /// <param name="video">video to assign</param>
        /// <returns>true if assigment was possible, otherwise false</returns>
        public bool AssignVideo(Video video)
        {
            if (video != null && !this.VideoIsHosted(video.ID))
            {
                if (this.CurrentSize + video.Size <= this.MaxSize)
                {
                    this.CurrentSize += video.Size;
                    this.RemainingSize -= video.Size;

                    this.VideosID.Add(video.ID);

                    Write.Trace($"SUCCESS assign video : {video.ID} to {this.ToString()}");

                    return true;
                }
                else
                {
                    this.RefusedLastAssigment = true;

                    Write.Trace($"FAIL (size) assign video : {video.ID} to {this.ToString()}");

                    return false;
                }
            }
            else
            {
                Write.Trace($"FAIL (already hosted) assign video : {video.ID} to {this.ToString()}");

                return false;
            }
        }

        public bool VideoIsHosted(int videoID)
        {
            return this.VideosID.Where(x => x == videoID).Any();
        }

        public void Reset()
        {
            VideosID = new List<int>();

            GainCacheServers = new List<GainCacheServer>();

            this.CurrentSize = 0;
            this.RemainingSize = this.MaxSize;

            this.IsFull = false;
            this.RefusedLastAssigment = false;
        }

        public override string ToString()
        {
            return Write.Invariant($"cacheServerID:{this.ID} CurrentSize:{this.CurrentSize} RemainingSize:{this.RemainingSize} MaxSize:{this.MaxSize} RefusedLastAssigment:{this.RefusedLastAssigment} IsFull:{this.IsFull}");
        }
    }
}