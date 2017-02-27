namespace HashCode
{
    using System;
    using System.IO;

    /// <summary>inputs used for tests</summary>
    public static class Inputs
    {
        private static string resourcesFolder;

        public static string InExample
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "example.in");
            }
        }

        public static string InKitten
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "kittens.in");
            }
        }

        public static string InMeAtTheZoo
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "me_at_the_zoo.in");
            }
        }

        public static string InTrendingToday
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "trending_today.in");
            }
        }

        public static string InVideosWorthSpreading
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "videos_worth_spreading.in");
            }
        }

        public static string OutExample
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "example.out");
            }
        }

        public static string OutKitten
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "kittens.out");
            }
        }

        public static string OutMeAtTheZoo
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "me_at_the_zoo.out");
            }
        }

        public static string OutTrendingToday
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "trending_today.out");
            }
        }

        public static string OutVideosWorthSpreading
        {
            get
            {
                return Path.Combine(Inputs.ResourcesFolder, "videos_worth_spreading.out");
            }
        }

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
    }
}