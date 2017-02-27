namespace HashCode
{
    public class GainCacheServer
    {
        public int EndPointID;
        public int Gain;
        public int GainPerMegaByte;
        public int VideoID;
        public int VideoSize;

        public GainCacheServer(int endPointID, int gain, int videoID, int videoSize)
        {
            this.EndPointID = endPointID;
            this.Gain = gain;
            this.VideoID = videoID;
            this.VideoSize = videoSize;

            this.GainPerMegaByte = this.Gain / this.VideoSize;
        }

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        public override string ToString()
        {
            return Write.Invariant($"EndPointID:{this.EndPointID} Gain:{this.Gain} VideoID:{this.VideoID} VideoSize:{this.VideoSize} GainPerMegaByte:{this.GainPerMegaByte}");
        }
    }
}