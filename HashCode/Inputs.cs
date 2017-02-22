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
    }
}