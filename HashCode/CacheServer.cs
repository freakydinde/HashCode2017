namespace HashCode
{
    using System.Collections.Generic;
    using System.Linq;

    public class CacheServer
    {
        public int CurrentSize;
        public List<int> EndPointIds;
        public int ID;
        public int MaxSize;
        public int RemainingSize;
        public List<int> VideosID;

        public CacheServer(int id, int maxSize)
        {
            this.ID = id;
            this.MaxSize = maxSize;
            this.EndPointIds = new List<int>();

            VideosID = new List<int>();

            this.CurrentSize = 0;
            this.RemainingSize = this.MaxSize;
        }

        /// <summary>Assign video to cache server, test if video is already assigned and if server is not full</summary>
        /// <param name="video">video to assign</param>
        /// <returns>true if assigment was possible, otherwise false</returns>
        public bool AssignVideo(int videoId, int videoSize)
        {
            if (!this.IsVideoHost(videoId))
            {
                if (this.CurrentSize + videoSize <= this.MaxSize)
                {
                    this.CurrentSize += videoSize;
                    this.RemainingSize -= videoSize;

                    this.VideosID.Add(videoId);

                    Write.Trace($"SUCCESS assign video : {videoId} to {this.ToString()}");

                    return true;
                }
                else
                {
                    Write.Trace($"FAIL (size) assign video : {videoId} to {this.ToString()}");

                    return false;
                }
            }
            else
            {
                Write.Trace($"FAIL (already hosted) assign video : {videoId} to {this.ToString()}");

                return false;
            }
        }

        public bool IsConnectedToEndPoint(int endPointID)
        {
            return this.EndPointIds.Where(x => x == endPointID).Any();
        }

        public bool IsVideoHost(int videoID)
        {
            return this.VideosID.Where(x => x == videoID).Any();
        }
        
        public override string ToString()
        {
            return Write.Invariant($"cacheServerID:{this.ID} CurrentSize:{this.CurrentSize} RemainingSize:{this.RemainingSize} MaxSize:{this.MaxSize}");
        }
    }
}