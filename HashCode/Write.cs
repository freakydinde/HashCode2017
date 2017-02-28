namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    /// <summary>Format string to the appropriate culture</summary>
    public static class Write
    {
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

        /// <summary>start stopwatch</summary>
        public static void StartWatch()
        {
            if (stopWatch == null)
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
            }
            else
            {
                Write.ResetWatch();
            }
        }

        /// <summary>send StopWatch elapsed and message to console output, using current culture</summary>
        /// <param name="message">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void StartWatch(FormattableString message, bool console = false)
        {
            Write.StartWatch(Write.Current(message), console);
        }

        /// <summary>send StopWatch elapsed and message to console output, using current culture</summary>
        /// <param name="message">message to log</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void StartWatch(string message, bool console = false)
        {
            System.Diagnostics.Trace.WriteLine(message);

            if (console)
            {
                Console.WriteLine(message);
            }

            Write.StartWatch();
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
        /// <param name="message">message to print</param>
        /// <param name="collection">collection to print after message</param>
        /// <param name="separator">string.Join separator</param>
        /// <param name="console">write message to console (default = false)</param>
        public static void Trace(FormattableString message, IEnumerable<object> collection, string separator = ";", bool console = false)
        {
            Write.Trace(Write.Current(message), collection, separator, console);
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
        public static void TraceInline(string message, bool console = false)
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
        public static void TraceInline(FormattableString formattable, bool console = false)
        {
            Write.TraceInline(Write.Current(formattable), console);
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
            Write.Trace(Write.Current($"{message} {Write.StopWatch.ElapsedMilliseconds}ms"), console);

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
            Write.TraceWatch(Write.Current($"{Write.StopWatch.ElapsedMilliseconds}ms"), console, resetWatch);
        }
    }
}