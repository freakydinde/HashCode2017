namespace HashCode.Pizza
{
    using System.IO;

    public class PizzInputs
    {
        public static string PizzFolder
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "Pizza");
            }
        }

        public static string PizzInputBig
        {
            get
            {
                return Path.Combine(PizzFolder, "big.in");
            }
        }

        public static string PizzInputExample
        {
            get
            {
                return Path.Combine(PizzFolder, "example.in");
            }
        }

        public static string PizzInputMedium
        {
            get
            {
                return Path.Combine(PizzFolder, "medium.in");
            }
        }

        public static string PizzInputSmall
        {
            get
            {
                return Path.Combine(PizzFolder, "small.in");
            }
        }

        public static string PizzResult
        {
            get
            {
                return Path.Combine(PizzFolder, "ResultPizza.txt");
            }
        }
    }
}