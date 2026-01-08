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


        /// <summary> A server's filesystem. </summary>
        /// <param name="users"> A list of the system's initial usernames. </param>
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
        public void CreateDirectory(String directoryPath, Boolean isRecursive = false)
        {
            ParseFilepath(directoryPath, out String[] files, out String cleanedPath);

            if (isRecursive)    // If directories should be recursively created.
            {
                String currentDirectory = String.Empty;
                for (Int32 i = 0; i < files.Length; i++)
                {
                    currentDirectory += $"/{files[i]}";
                    _directories.TryAdd(currentDirectory, new HashSet<File>());
                }
            }
            else
            {
                String parentDirectory = $"/{String.Join('/', files[..^1])}";
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


        /// <summary> Get all the directory and all sub-directories associated with the given directory path. </summary>
        /// <param name="directoryPath"> The directory path. </param>
        /// <returns> A subset of the filesystem with only the sub-directories associated with the given directory path. </returns>
        public Dictionary<String, File[]> ListDirectories(String directoryPath)
        {
            Boolean isWildcard = directoryPath[^1] == '*';
            String cleanedPath = directoryPath.TrimEnd(['/', '*']);

            // TODO - Finish and use in other methods where searching! DO WE EVEN NEED PARSER?
            Dictionary<String, File[]> subset = _directories.Where(x => x.Key.StartsWith(cleanedPath)).ToDictionary(d => d.Key, d => d.Value.ToArray());
            if (isWildcard) // If we use a wildcard, we only want the sub-directories, not the parent.
            {
                subset.Remove(cleanedPath);
            }

            return subset;
        }


        /// <summary> Create a new file at the given path. </summary>
        /// <param name="filePath"> The absolute filepath. Will assume that the last name is that of the file including extension. i.e. /DIR/DIR/file.txt </param>
        /// <param name="isRecursive"> Whether directories should be created recursively if they don't already exist. </param>
        public void CreateFile(String filePath, Boolean isRecursive = false)
        {
            ParseFilepath(filePath, out String[] files, out String cleanedPath);

            String[] directories = files[..^1];
            String filename = files[^1];

            File newFile = new File(filename, DateTime.UtcNow); // TODO - Pull for server time.

            String directoryName = "/" + String.Join('/', directories);
            if (isRecursive)
            {
                CreateDirectory(directoryName, true); // TODO - Handle permissions.
            }

            _directories[directoryName].Add(newFile);    // TODO - Handle overwrite, permissions, etc.
        }


        /// <summary> Remove a file or directory from the filesystem. </summary>
        /// <param name="filePath"> The filepath of the file to remove. </param>
        /// <param name="isRecursive"> Whether the element should be removed recursively. </param>
        /// <exception cref="TerminalException"/>
        public void RemoveFile(String filePath, Boolean isRecursive = false)
        {
            //Dictionary<String, File[]> relevantDirectories1 = ListDirectories(filePath);

            ParseFilepath(filePath, out String[] files, out String cleanedPath);
            // TODO - Delete only contents if ends with '*'

            String[] directories = files[..^1];
            String filename = files[^1];
            String parentDirectory = '/' + String.Join('/', directories);

            if (String.IsNullOrWhiteSpace(System.IO.Path.GetExtension(filename)))   // Check if the filepath is actually a directory. If so, the extension method will return an empty string.
            {
                Dictionary<String, File[]> relevantDirectories = ListDirectories(cleanedPath);
                if (relevantDirectories.Count == 0)
                {
                    throw new TerminalException($"The directory does not exist within the filesystem.");
                }

                if (isRecursive)    // If recursive, we're deleting everything.
                {
                    foreach (KeyValuePair<String, File[]> directory in relevantDirectories)
                    {
                        _directories.Remove(directory.Key);
                    }
                }
                else                // Else, we want to check there's nothing there first.
                {
                    if (relevantDirectories.Count == 1 && _directories[cleanedPath].Count() == 0)  // There should be no sub-directories, and no files.
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


        /// <summary> Separate a filepath into its directory components. </summary>
        /// <param name="filePath"> The raw string. </param>
        /// <param name="files"> The file components as an array. </param>
        /// <param name="cleanedPath"> The original path without any trailing delineator. </param>
        /// <exception cref="TerminalException"/>
        private void ParseFilepath(String filePath, out String[] files, out String cleanedPath)
        {
            // Clean the filepath by removing any trailing delineates.
            cleanedPath = filePath.TrimEnd('/');
            if (String.IsNullOrWhiteSpace(cleanedPath)) // If the result is empty, we were trying to remove root.
            {
                throw new TerminalException($"Unable to target 'root' within the filesystem.");
            }

            files = cleanedPath.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
