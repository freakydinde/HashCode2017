namespace HashCode.Client
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Pizza();
        }

        public static void Pizza()
        {
            Console.WriteLine(Write.Invariant($"resources folder path : {Inputs.ResourcesFolder}"));
        }
    }
}