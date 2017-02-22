namespace HashCode
{
    using System;
    using System.IO;

    /// <summary>inputs used for tests</summary>
    public static class Inputs
    {
        private static string resourcesFolder;

        public static string ResourcesFolder
        {
            get
            {
                if (resourcesFolder == null)
                {
                    resourcesFolder = AppDomain.CurrentDomain.BaseDirectory;

                    while (!Directory.Exists(Path.Combine(resourcesFolder, "Resources")))
                    {
                        resourcesFolder = Directory.GetParent(resourcesFolder).FullName;
                    }

                    resourcesFolder = Path.Combine(resourcesFolder, "Resources");
                }

                return resourcesFolder;
            }
        }

        public static string ResourcesPizzaFolder
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "Pizza");
            }
        }

        public static string ResourcesPizzaInputBig
        {
            get
            {
                return Path.Combine(ResourcesPizzaFolder, "big.in");
            }
        }

        public static string ResourcesPizzaInputExample
        {
            get
            {
                return Path.Combine(ResourcesPizzaFolder, "example.in");
            }
        }

        public static string ResourcesPizzaInputMedium
        {
            get
            {
                return Path.Combine(ResourcesPizzaFolder, "medium.in");
            }
        }

        public static string ResourcesPizzaInputSmall
        {
            get
            {
                return Path.Combine(ResourcesPizzaFolder, "small.in");
            }
        }
    }
}