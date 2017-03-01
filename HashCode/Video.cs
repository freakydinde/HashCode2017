namespace HashCode
{
    public class Video
    {
        public int ID;

        public int Size;

        public Video(int id, int size)
        {
            this.ID = id;
            this.Size = size;
        }

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        public override string ToString()
        {
            return Write.Invariant($"ID:{this.ID} Size:{this.Size}");
        }
    }
}