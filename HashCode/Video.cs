namespace HashCode
{
    public class Video
    {
        public int Size;

        public Video(int size)
        {
            this.Size = size;
        }

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        public override string ToString()
        {
            return Write.Invariant($"Size:{this.Size}");
        }
    }
}