using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Administrator.Subspace.Files;

namespace Administrator.Subspace.Programs
{
    /// <summary> Get the directories and files within the file system. </summary>
    public class ListProgram : TerminalProgram
    {
        /// <inheritdoc/>
        public override String Command => "ls";

        /// <inheritdoc/>
        public override Dictionary<String, Boolean> Parameters => new Dictionary<String, Boolean>();    // TODO - Implement -ls and additional arguments.

        /// <inheritdoc/>
        public override Int32[] NumberOfPositionalArguments => [ 0, 1 ];

        /// <inheritdoc/>
        public override String Description => "Lists the sub-directories and files within a directory.";


        /// <summary> Get the directories and files within the file system. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public ListProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(String directoryPath, Dictionary<String, String?> parameters, String[] positionalArguments)
        {
            StringBuilder resultDirectories = new StringBuilder();
            StringBuilder resultFiles = new StringBuilder();

            // Build the search path.
            String searchPath = directoryPath;
            if (positionalArguments.Length == 1)                                        // If we specify a filepath, we need to modify the search to include it.
            {
                String argument = positionalArguments[0];
                searchPath = argument[0] == '/' ? argument : searchPath += argument;    // If we're starting from root, overwrite, else append.
            }

            // Get the directories.
            Dictionary<String, File[]> directories = SOURCE.Files.ListDirectory(searchPath);

            // Build the strings for directories and files.
            foreach (KeyValuePair<String, File[]> directory in directories)
            {
                resultDirectories.AppendLine(directory.Key);
                foreach (File file in directory.Value.OrderBy(x => x.Name))
                {
                    resultFiles.AppendLine(file.Name);
                }
            }

            return $"{resultDirectories.ToString()}\n{resultFiles.ToString()}";
        }
    }
}
