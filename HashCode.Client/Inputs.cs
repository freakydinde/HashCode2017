﻿namespace HashCode.Client
{
    using System;
    using System.Globalization;
    using System.IO;

    /// <summary>inputs used for tests</summary>
    public static class Inputs
    {
        private static string resourcesFolder;

        private static string ResourcesFolder
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