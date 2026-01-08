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
        public override HashSet<ParameterInformation> Parameters => new HashSet<ParameterInformation>()
        {
            new ParameterInformation(0, "The directory path, relative or absolute, to list the file structure for.")
        };    // TODO - Implement -ls and additional arguments.

        /// <inheritdoc/>
        public override String Description => "Lists the sub-directories and files within a directory.";


        /// <summary> Get the directories and files within the file system. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public ListProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(String directoryPath, Dictionary<ParameterInformation, String> parameters)
        {
            StringBuilder resultDirectories = new StringBuilder();
            StringBuilder resultFiles = new StringBuilder();

            // Build the search path.
            String searchPath = directoryPath;

            foreach (KeyValuePair<ParameterInformation, String> parameter in parameters)
            {
                switch (parameter.Key.FullName)
                {
                    case "0":   // If we specify a filepath, we need to modify the search to include it.
                        String argument = parameter.Value;
                        searchPath = argument[0] == '/' ? argument : searchPath += argument;    // If we're starting from root, overwrite, else append.
                        break;
                }
            }

            // Get the directories.
            IOrderedEnumerable<KeyValuePair<String, File[]>> directories = SOURCE.Files.ListDirectories(searchPath).OrderBy(x => x.Key);

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
