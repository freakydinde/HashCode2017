namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public static class Read
    {
        public static IEnumerable<string[]> CollectionLines(string input, char separator = ';')
        {
            return from i in File.ReadAllLines(input) select (from j in i.TrimEnd().Split(separator) select j).ToArray();
        }

        public static IEnumerable<int[]> NumberLines(string input)
        {
            return from i in File.ReadAllLines(input) select (from j in i.TrimEnd().Split(' ') select Convert.ToInt32(j, CultureInfo.InvariantCulture)).ToArray();
        }
    }

    /// <summary>Format string to the appropriate culture</summary>
    public static class Write
    {
        private static Stopwatch methodWatch;
        private static Stopwatch stopWatch;

        /// <summary>Gets a stopwatch object from Singleton</summary>
        public static Stopwatch StopWatch
        {
            get
            {
                if (stopWatch == null)
                {
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                }

                return stopWatch;
            }
        }

        /// <summary>flatten a collection to string</summary>
        /// <param name="inputs">collection to flat</param>
        /// <param name="separator">string.Join separator</param>
        /// <returns>collection flatten with string.join</returns>
        public static string Collection(IEnumerable<object> inputs, string separator = ";")
        {
            if (inputs == null)
            {
                return "null";
            }
            else if (!inputs.Any())
            {
                return "empty";
            }
            else
            {
                return string.Join(separator, inputs);
            }
        }

        /// <summary>Format string to invariant culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        /// <returns>string formated</returns>
        public static string Current(FormattableString formattable)
        {
            return formattable?.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Print methodWath elapsed milliseconds</summary>
        /// <param name="console">write message to console (default = false)</param>
        public static void EndWatch(bool console = false)
        {
            string elapsed = " " + methodWatch.ElapsedMilliseconds.ToString() + " ms";

            System.Diagnostics.Trace.WriteLine(elapsed);

            if (console)
            {
                Console.WriteLine(elapsed);
            }
        }

        /// <summary>Format string to invariant culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        /// <returns>string formated</returns>
        public static string Invariant(FormattableString formattable)
        {
            return formattable?.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Reset stopwatch singleton</summary>
        public static void ResetWatch()
        {
            Write.StopWatch.Reset();
            Write.StopWatch.Start();
        }

        /// <summary>Write start message without line break, start method watch</summary>
        /// <param name="formattable">initial string as formattable</param>
        /// <param name="console"></param>
        public static void StartWatch(FormattableString formattable, bool console = false)
        { 
            Write.StartWatch(Write.Current(formattable), console);
        }

        /// <summary>Write start message without line break, start method watch</summary>
        /// <param name="message">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void StartWatch(string message, bool console = false)
        {
            System.Diagnostics.Trace.Write(message);

            if (console)
            {
                Console.Write(message);
            }

            if (methodWatch == null)
            {
                methodWatch = new Stopwatch();
            }
            else
            {
                methodWatch.Reset();
            }

            methodWatch.Start();
        }
        
        /// <summary>send message and collection flatten to string to debug output, using current culture</summary>
        /// <param name="message">message to print</param>
        /// <param name="collection">collection to print after message</param>
        /// <param name="separator">string.Join separator</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void Trace(IEnumerable<object> collection, string separator = ";", bool console = false)
        {
            System.Diagnostics.Trace.WriteLine(Write.Current($"{Write.Collection(collection, separator)}"));

            if (console)
            {
                Console.WriteLine(Write.Current($"{Write.Collection(collection, separator)}"));
            }
        }

        /// <summary>send message and collection flatten to string to debug output, using current culture</summary>
        /// <param name="message">message to print</param>
        /// <param name="collection">collection to print after message</param>
        /// <param name="separator">string.Join separator</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void Trace(string message, IEnumerable<object> collection, string separator = ";", bool console = false)
        {
            System.Diagnostics.Trace.WriteLine(Write.Current($"{message}{Environment.NewLine}{Write.Collection(collection, separator)}"));

            if (console)
            {
                Console.WriteLine(Write.Current($"{message}{Write.Collection(collection, separator)}"));
            }
        }

        /// <summary>send message and collection flatten to string to debug output, using current culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        /// <param name="collection">collection to print after message</param>
        /// <param name="separator">string.Join separator</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void Trace(FormattableString formattable, IEnumerable<object> collection, string separator = ";", bool console = false)
        {
            Write.Trace(Write.Current(formattable), collection, separator, console);
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="console">write message to console (default = false)</param>
        public static void TraceLine(bool console = false)
        {
            System.Diagnostics.Trace.Write(Environment.NewLine);

            if (console)
            {
                Console.WriteLine();
            }
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="message">initial string as formattable</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void Trace(string message, bool console = false)
        {
            System.Diagnostics.Trace.WriteLine(message);

            if (console)
            {
                Console.WriteLine(message);
            }
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void Trace(FormattableString formattable, bool console = false)
        {
            Write.Trace(Write.Current(formattable), console);
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="mesage">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void TraceInLine(string message, bool console = false)
        {
            System.Diagnostics.Trace.Write(message);

            if (console)
            {
                Console.Write(message);
            }
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void TraceInLine(FormattableString formattable, bool console = false)
        {
            Write.TraceInLine(Write.Current(formattable), console);
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void TraceVisible(FormattableString formattable, bool console = false)
        {
            Write.Trace($"{Environment.NewLine}{formattable}{Environment.NewLine}", console);
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="message">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void TraceVisible(string message, bool console = false)
        {
            Write.Trace($"{Environment.NewLine}{message}{Environment.NewLine}", console);
        }

        /// <summary>send StopWatch elapsed and message to trace output, using current culture, then reset stopwatch timer</summary>
        /// <param name="formattable">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        /// <param name="resetWatch">should i reset watch after message</param>
        public static void TraceWatch(FormattableString formattable, bool console = false, bool resetWatch = false)
        {
            Write.TraceWatch(Write.Current(formattable), console, resetWatch);
        }

        /// <summary>send StopWatch elapsed and message to trace output, using current culture, then reset stopwatch timer</summary>
        /// <param name="message">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        /// <param name="resetWatch">should i reset watch after message</param>
        public static void TraceWatch(string message, bool console = false, bool resetWatch = false)
        {
            Write.Trace(Write.Current($"{message} {Write.StopWatch.ElapsedMilliseconds} ms"), console);

            if (resetWatch)
            {
                Write.ResetWatch();
            }
        }

        /// <summary>send StopWatch elapsed and message to trace output, with a line before and after using current culture.</summary>
        /// <param name="formattable">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        /// <param name="resetWatch">should i reset watch after message</param>
        public static void TraceWatchVisible(FormattableString formattable, bool console = false, bool resetWatch = false)
        {
            Write.TraceWatchVisible(Write.Current(formattable), console, resetWatch);
        }

        /// <summary>send StopWatch elapsed and message to trace output, with a line before and after using current culture.</summary>
        /// <param name="message">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        /// <param name="resetWatch">should i reset watch after message</param>
        public static void TraceWatchVisible(string message, bool console = false, bool resetWatch = false)
        {
            Write.TraceVisible(Write.Current($"{message} {Write.StopWatch.ElapsedMilliseconds} ms"), console);

            if (resetWatch)
            {
                Write.ResetWatch();
            }
        }

        /// <summary>send StopWatch elapsed and message to trace output, without line break using current culture.</summary>
        /// <param name="formattable">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        /// <param name="resetWatch">should i reset watch after message</param>
        public static void TraceWatchInLine(FormattableString formattable, bool console = false, bool resetWatch = false)
        {
            Write.TraceWatchInLine(Write.Current(formattable), console, resetWatch);
        }

        /// <summary>send StopWatch elapsed and message to trace output, without line break using current culture, then reset stopwatch timer</summary>
        /// <param name="message">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        /// <param name="resetWatch">should i reset watch after message</param>
        public static void TraceWatchInLine(string message, bool console = false, bool resetWatch = false)
        {
            Write.TraceInLine(Write.Current($"{message} {Write.StopWatch.ElapsedMilliseconds} ms"), console);

            if (resetWatch)
            {
                Write.ResetWatch();
            }
        }

        /// <summary>send StopWatch elapsed and message to trace output, using current culture</summary>
        /// <param name="console">write message to console (default = false)</param>
        /// <param name="resetWatch">should i reset watch after message</param>
        public static void TraceWatch(bool console = false, bool resetWatch = false)
        {
            Write.TraceWatch(Write.Current($"{Write.StopWatch.ElapsedMilliseconds} ms"), console, resetWatch);
        }
    }
}