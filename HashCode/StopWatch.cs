namespace HashCode
{
    using System.Diagnostics;

    /// <summary>Format string to the appropriate culture</summary>
    public static class Watch
    {
        private static Stopwatch sw;

        private static Stopwatch stopWatch
        {
            get
            {
                if (sw == null)
                {
                    sw = new Stopwatch();
                    sw.Start();
                }

                return sw;
            }
        }

        public static void StartWatch()
        {
            Write.Print($"starting {stopWatch.ToString()}");
        }

        public static void WriteWatch()
        {
            Write.Print($"elapased mm {stopWatch.ElapsedMilliseconds}");
        }

        public static void WriteWatchThenReset()
        {
            WriteWatch();
            stopWatch.Reset();
        }

        public static void TraceWatch()
        {
            Write.Trace($"elapased mm {stopWatch.ElapsedMilliseconds}");
        }

        public static void TraceWatchThenReset()
        {
            TraceWatch();
            stopWatch.Reset();
        }
    }
}