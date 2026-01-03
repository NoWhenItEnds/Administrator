using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Administrator.Subspace.Files
{
    /// <summary> A server's filesystem. </summary>
    public class FileSystem
    {
        /// <summary> The filesystem's directory data structure, mapping the directory path to the files within it. </summary>
        private Dictionary<String, HashSet<File>> _directories = new Dictionary<String, HashSet<File>>();

        public FileSystem(String[] users)
        {
            CreateDirectory($"/etc");
            CreateDirectory($"/var");
            foreach (String user in users)
            {
                CreateDirectory($"/home/{user}");
            }
        }


        /// <summary> Create a new directory at the given path. </summary>
        /// <param name="directoryPath"> The absolute directory path. Will recursively create directories. </param>
        public void CreateDirectory(String directoryPath)
        {
            String[] directories = directoryPath.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String currentDirectory = String.Empty;
            for (Int32 i = 0; i < directories.Length; i++)
            {
                currentDirectory += $"/{directories[i]}";
                _directories.TryAdd(currentDirectory, new HashSet<File>());
            }
        }

        public Dictionary<String, File[]> ListDirectories(String directoryPath) => _directories.Where(x => x.Key.StartsWith(directoryPath)).ToDictionary(d => d.Key, d => d.Value.ToArray());


        /// <summary> Attempt to create a new file at the given path. </summary>
        /// <param name="filePath"> The absolute filepath. Will assume that the last name is that of the file including extension. i.e. /DIR/DIR/file.txt </param>
        /// <param name="isRecursive"> Whether directories should be created recursively if they don't already exist. </param>
        /// <returns> Whether the operation was successful and a file was created. </returns>
        public Boolean TryCreateFile(String filePath, Boolean isRecursive = false)
        {
            String[] names = filePath.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String[] directories = names[..^1];
            String filename = names[^1];

            File newFile = new File(filename, DateTime.UtcNow); // TODO - Pull for server time.

            String directoryName = "/" + String.Join('/', directories);
            if (isRecursive)
            {
                CreateDirectory(directoryName); // TODO - Handle permissions.
            }

            return _directories[directoryName].Add(newFile);    // TODO - Handle overwrite, permissions, etc.
        }
    }
}
