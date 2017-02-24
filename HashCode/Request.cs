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
    }
}