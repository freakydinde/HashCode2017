using System.Collections.Generic;

namespace HashCode
{
    public class Request
    {
        public int EndPointID;

        public List<GainCacheServer> Gains;

        public int Occurency;
        public int VideoID;
        public int VideoSize;

        public bool Assigned;

        public Request(int endPointID, int videoID, int occurency)
        {
            this.EndPointID = endPointID;
            this.Occurency = occurency;
            this.VideoID = videoID;

            this.Gains = new List<GainCacheServer>();

            this.Assigned = false;
        }

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        public override string ToString()
        {
            return Write.Invariant($"EndPointID:{this.EndPointID} Occurency:{this.Occurency} VideoID:{this.VideoID}");
        }
    }
}