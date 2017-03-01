namespace HashCode
{
    public class Request
    {
        public int EndPointID;
        public int Occurency;
        public int VideoID;

        public Request(int endPointID, int videoID, int occurency)
        {
            this.EndPointID = endPointID;
            this.Occurency = occurency;
            this.VideoID = videoID;
        }

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        public override string ToString()
        {
            return Write.Invariant($"EndPointID:{this.EndPointID} Occurency:{this.Occurency} VideoID:{this.VideoID}");
        }
    }
}