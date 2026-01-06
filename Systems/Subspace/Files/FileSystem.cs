using System;
using System.Collections.Generic;
using System.Linq;
using Administrator.Utilities.Exceptions;

namespace Administrator.Subspace.Files
{
    /// <summary> A server's filesystem. </summary>
    public class FileSystem
    {
        /// <summary> The filesystem's directory data structure, mapping the directory path to the files within it. </summary>
        private Dictionary<String, HashSet<File>> _directories = new Dictionary<String, HashSet<File>>();

        public FileSystem(String[] users)
        {
            CreateDirectory($"/etc", true);
            CreateDirectory($"/var", true);
            foreach (String user in users)
            {
                CreateDirectory($"/home/{user}", true);
            }
        }


        /// <summary> Create a new directory at the given path. </summary>
        /// <param name="directoryPath"> The absolute directory path. Will recursively create directories. </param>
        /// <param name="isRecursive"> Whether directories should be created recursively if they don't already exist. </param>
        public void CreateDirectory(String directoryPath, Boolean isRecursive)
        {
            String[] directories = directoryPath.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (isRecursive)    // If directories should be recursively created.
            {
                String currentDirectory = String.Empty;
                for (Int32 i = 0; i < directories.Length; i++)
                {
                    currentDirectory += $"/{directories[i]}";
                    _directories.TryAdd(currentDirectory, new HashSet<File>());
                }
            }
            else
            {
                String parentDirectory = $"/{String.Join('/', directories[..^1])}";
                if (_directories.Keys.Contains(parentDirectory))    // Check to see if the parent directory already exists.
                {
                    _directories.TryAdd(directoryPath, new HashSet<File>());
                }
                else
                {
                    throw new TerminalException($"Unable to create directory as its parent directory doesn't exist.");
                }
            }
        }

        public Dictionary<String, File[]> ListDirectories(String directoryPath) => _directories.Where(x => x.Key.StartsWith(directoryPath)).ToDictionary(d => d.Key, d => d.Value.ToArray());


        /// <summary> Create a new file at the given path. </summary>
        /// <param name="filePath"> The absolute filepath. Will assume that the last name is that of the file including extension. i.e. /DIR/DIR/file.txt </param>
        /// <param name="isRecursive"> Whether directories should be created recursively if they don't already exist. </param>
        public void CreateFile(String filePath, Boolean isRecursive = false)
        {
            String[] names = filePath.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String[] directories = names[..^1];
            String filename = names[^1];

            File newFile = new File(filename, DateTime.UtcNow); // TODO - Pull for server time.

            String directoryName = "/" + String.Join('/', directories);
            if (isRecursive)
            {
                CreateDirectory(directoryName, true); // TODO - Handle permissions.
            }

            _directories[directoryName].Add(newFile);    // TODO - Handle overwrite, permissions, etc.
        }


        public void RemoveFile(String filePath, Boolean isRecursive = false)
        {
            // Clean the filepath by removing any trailing delineates.
            String cleanedPath = filePath.TrimEnd('/');
            if (String.IsNullOrWhiteSpace(cleanedPath)) // If the result is empty, we were trying to remove root.
            {
                throw new TerminalException($"Unable to remove 'root' from the filesystem.");
            }

            String[] names = cleanedPath.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String[] directories = names[..^1];
            String filename = names[^1];
            String parentDirectory = '/' + String.Join('/', directories);

            if (String.IsNullOrWhiteSpace(System.IO.Path.GetExtension(filename)))   // Check if the filepath is actually a directory. If so, the extension method will return an empty string.
            {
                KeyValuePair<String, HashSet<File>>[] relevantDirectories = _directories.Where(x => x.Key.StartsWith(filePath)).OrderByDescending(x => x.Key.Length).ToArray();
                if (relevantDirectories.Length == 0)
                {
                    throw new TerminalException($"The directory does not exist within the filesystem.");
                }

                if (isRecursive)    // If recursive, we're deleting everything.
                {
                    foreach (KeyValuePair<String, HashSet<File>> directory in relevantDirectories)
                    {
                        _directories.Remove(directory.Key);
                    }
                }
                else                // Else, we want to check there's nothing there first.
                {
                    if (relevantDirectories.Length == 1 && _directories[cleanedPath].Count() == 0)  // There should be no sub-directories, and no files.
                    {
                        _directories.Remove(cleanedPath);
                    }
                    else
                    {
                        throw new TerminalException($"The directory contains files or additional directories. Remove them first, or use --recursive.");
                    }
                }
            }
            else                                                                                // If the filepath is a file.
            {
                _directories[parentDirectory].RemoveWhere(x => x.Name == filename);
            }
        }
    }
}
