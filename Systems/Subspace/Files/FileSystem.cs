using System;
using System.Collections.Generic;
using System.Linq;

namespace Administrator.Subspace.Files
{
    /// <summary> A server's filesystem. </summary>
    public class FileSystem
    {
        /// <summary> The filesystem's directory data structure, mapping the directory path to the files within it. </summary>
        private Dictionary<String, HashSet<File>> _directory = new Dictionary<String, HashSet<File>>();

        public FileSystem(String[] users)
        {
            CreateDirectory($"/etc");
            CreateDirectory($"/var");
            foreach (String user in users)
            {
                CreateDirectory($"/home/{user}");
            }
        }

        public void CreateDirectory(String directoryPath)
        {
            String[] directories = directoryPath.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String currentDirectory = String.Empty;
            for (Int32 i = 0; i < directories.Length; i++)
            {
                currentDirectory += $"/{directories[i]}";
                _directory.TryAdd(currentDirectory, new HashSet<File>());
            }
        }

        public Dictionary<String, File[]> ListDirectory(String directoryPath) => _directory.Where(x => x.Key.StartsWith(directoryPath)).ToDictionary(d => d.Key, d => d.Value.ToArray());
    }
}
