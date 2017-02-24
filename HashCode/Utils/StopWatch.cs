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

        public static void StartWatch(string message)
        {
            Write.Print($"{message} {stopWatch.ElapsedMilliseconds}ms");
        }

        public static void StartWatch()
        {
            Write.Print($"starting {stopWatch.ElapsedMilliseconds}ms");
        }

        public static void TraceWatch(string message)
        {
            Write.Trace($"{message} {stopWatch.ElapsedMilliseconds}ms");
        }

        public static void TraceWatch()
        {
            Write.Trace($"elapased mm {stopWatch.ElapsedMilliseconds}ms");
        }

        public static void TraceWatchThenReset(string message)
        {
            TraceWatch(message);
            stopWatch.Reset();
        }

        public static void TraceWatchThenReset()
        {
            TraceWatch();
            stopWatch.Reset();
        }

        public static void WriteWatch(string message)
        {
            Write.Print($"{message}");
        }

        public static void WriteWatch()
        {
            Write.Print($"elapased mm {stopWatch.ElapsedMilliseconds}ms");
        }

        public static void WriteWatchThenReset(string message)
        {
            WriteWatch(message);
            stopWatch.Reset();
        }

        public static void WriteWatchThenReset()
        {
            WriteWatch();
            stopWatch.Reset();
        }
    }
}