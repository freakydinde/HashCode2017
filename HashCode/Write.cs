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
        public static string Collection(IEnumerable<object> inputs, string separator)
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

        /// <summary>flatten a inputs to string</summary>
        /// <param name="inputs">inputs to flat</param>
        /// <returns>inputs flatten with string.join</returns>
        public static string Collection(IEnumerable<object> inputs)
        {
            return Write.Collection(inputs, ";");
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

        /// <summary>send string to console output, using current culture</summary>
        /// <param name="message">message to log</param>
        public static void Print(string message)
        {
            Console.WriteLine(Write.Current($"{message}"));
        }

        /// <summary>send string to console output, using current culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        public static void Print(FormattableString formattable)
        {
            Console.WriteLine(Write.Current(formattable));
        }

        /// <summary>flatten a inputs to string and print result</summary>
        /// <param name="inputs">inputs to flat</param>
        public static void PrintCollection(IEnumerable<object> inputs)
        {
            Console.WriteLine(Write.Collection(inputs));
        }

        /// <summary>flatten a inputs to string and print result</summary>
        /// <param name="inputs">inputs to flat</param>
        /// <param name="separator">string.Join separator</param>
        public static void PrintCollection(IEnumerable<object> inputs, string separator)
        {
            Console.WriteLine(Write.Collection(inputs, separator));
        }

        /// <summary>send StopWatch elapsed and message to console output, using current culture</summary>
        /// <param name="message">message to log</param>
        public static void PrintWatch(string message)
        {
            Console.WriteLine(Write.Current($"{message} {Write.StopWatch.ElapsedMilliseconds}ms"));
        }

        /// <summary>send StopWatch elapsed to console output, using current culture</summary>
        public static void PrintWatch()
        {
            Console.WriteLine(Write.Current($"stopwatch : {Write.StopWatch.ElapsedMilliseconds}ms"));
        }

        /// <summary>Reset stopwatch singleton</summary>
        public static void ResetWatch()
        {
            Write.StopWatch.Reset();
            Write.StopWatch.Start();
        }

        /// <summary>send message and collection flatten to string to debug output, using current culture</summary>
        /// <param name="message">message to print</param>
        /// <param name="collection">collection to print after message</param>
        /// <param name="separator">string.Join separator</param>
        public static void Trace(FormattableString message, IEnumerable<object> collection, string separator)
        {
            System.Diagnostics.Trace.WriteLine(Write.Current($"{message?.ToString()}{Write.Collection(collection, separator)}"));
        }

        /// <summary>send message and collection flatten to string to debug output, using current culture</summary>
        /// <param name="message">message to print</param>
        /// <param name="collection">collection to print after message</param>
        public static void Trace(FormattableString message, IEnumerable<object> collection)
        {
            Trace(message, collection, ";");
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        public static void Trace(FormattableString formattable)
        {
            System.Diagnostics.Trace.WriteLine(Write.Current(formattable));
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="message">initial string</param>
        public static void Trace(string message)
        {
            System.Diagnostics.Trace.WriteLine(message?.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>flatten a inputs to string and trace result</summary>
        /// <param name="inputs">inputs to flat</param>
        public static void TraceCollection(IEnumerable<object> inputs)
        {
            System.Diagnostics.Trace.WriteLine(Write.Collection(inputs));
        }

        /// <summary>flatten a inputs to string and trace result</summary>
        /// <param name="inputs">inputs to flat</param>
        /// <param name="separator">string.Join separator</param>
        public static void TraceCollection(IEnumerable<object> inputs, string separator)
        {
            System.Diagnostics.Trace.WriteLine(Write.Collection(inputs, separator));
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        public static void TraceInline(FormattableString formattable)
        {
            System.Diagnostics.Trace.Write(Write.Current(formattable));
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="formattable">initial string as formattable</param>
        public static void TraceVisible(FormattableString formattable)
        {
            System.Diagnostics.Trace.Write(Write.Current($"{Environment.NewLine}{Environment.NewLine}{formattable?.ToString()}{Environment.NewLine}{Environment.NewLine}"));
        }

        /// <summary>send string to debug output, using current culture</summary>
        /// <param name="message">message to log</param>
        public static void TraceVisible(string message)
        {
            System.Diagnostics.Trace.Write(Write.Current($"{Environment.NewLine}{Environment.NewLine}{message}{Environment.NewLine}{Environment.NewLine}"));
        }

        /// <summary>send StopWatch elapsed and message to trace output, using current culture, then reset stopwatch timer</summary>
        /// <param name="formattable">message to log</param>
        public static void TraceWatch(FormattableString formattable)
        {
            System.Diagnostics.Trace.Write(Write.Current($"{formattable?.ToString()} {Write.StopWatch.ElapsedMilliseconds}ms"));
        }

        /// <summary>send StopWatch elapsed and message to trace output, using current culture, then reset stopwatch timer</summary>
        /// <param name="message">message to log</param>
        public static void TraceWatch(string message)
        {
            System.Diagnostics.Trace.Write(Write.Current($"{message} {Write.StopWatch.ElapsedMilliseconds}ms"));
        }

        /// <summary>send StopWatch elapsed and message to trace output, using current culture</summary>
        public static void TraceWatch()
        {
            System.Diagnostics.Trace.Write(Write.Current($"elapased mm {Write.StopWatch.ElapsedMilliseconds}ms"));
        }
    }
}