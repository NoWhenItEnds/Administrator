using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Administrator.Utilities.Extensions
{
    /// <summary> Helpful extensions for working with resources. </summary>
    public static class ResourceExtensions
    {
        /// <summary> Search the resource folders for resources of a given type. </summary>
        /// <typeparam name="T"> The type of resource to load. </typeparam>
        /// <returns> An array of the loaded resources. </returns>
        public static T[] GetResources<T>() where T : Resource
        {
            T[] result = GetResources<T>("res://Resources");
            if (DirAccess.DirExistsAbsolute("user://Resources"))
            {
                T[] userResources = GetResources<T>("user://Resources");
                result = result.Concat(userResources).ToArray();
            }
            return result;
        }

        /// <summary> Search the given directory, recursively, and load all the resources of the given type. </summary>
        /// <typeparam name="T"> The type of resource to load. </typeparam>
        /// <param name="directoryPath"> The path of the root directory. </param>
        /// <returns> An array of loaded resources. </returns>
        public static T[] GetResources<T>(String directoryPath) where T : Resource
        {
            String[] resourcePaths = GetResourcePaths(directoryPath, [".tres"]);

            List<T> results = new List<T>();
            foreach (String path in resourcePaths)
            {
                // Check the resource is of the correct type.
                Resource resource = ResourceLoader.Load(path);
                if (resource is T castResource)
                {
                    results.Add(castResource);
                }
            }

            return results.ToArray();
        }


        /// <summary> Search the given directory, recursively, for files. </summary>
        /// <param name="directoryPath"> The Godot filepath to search. </param>
        /// <param name="extensions"> A list of allowed extensions. A null means to accept everything. </param>
        /// <returns> The filepaths of all the found extensions. </returns>
        /// <exception cref="DirectoryNotFoundException"/>
        public static String[] GetResourcePaths(String directoryPath, String[]? extensions = null)
        {
            DirAccess dataDirectory = DirAccess.Open(directoryPath);
            if (dataDirectory == null)
            {
                throw new DirectoryNotFoundException($"The '{directoryPath}' directory does not exist! Something has gone very wrong Captain.");
            }

            List<String> resources = new List<String>();
            dataDirectory.ListDirBegin();
            String current = dataDirectory.GetNext();
            while (!String.IsNullOrEmpty(current))
            {
                String currentPath = directoryPath + '/' + current;
                if (dataDirectory.CurrentIsDir())
                {
                    resources.AddRange(GetResourcePaths(currentPath, extensions));
                }
                else
                {
                    // Only add the file if it's part of the acceptable extensions.
                    if (extensions == null || extensions.Contains(Path.GetExtension(currentPath)))
                    {
                        resources.Add(currentPath);
                    }
                }
                current = dataDirectory.GetNext();
            }
            dataDirectory.ListDirEnd();

            return resources.ToArray();
        }
    }
}
