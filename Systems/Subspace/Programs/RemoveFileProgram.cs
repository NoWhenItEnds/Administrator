using System;
using System.Collections.Generic;
using System.Linq;
using Administrator.Utilities.Extensions;

namespace Administrator.Subspace.Programs
{
    /// <summary> Remove a file or directory from the filesystem. </summary>
    public class RemoveFileProgram : TerminalProgram
    {
        /// <inheritdoc/>
        public override String Command => "rm";

        /// <inheritdoc/>
        public override HashSet<ParameterInformation> Parameters => new HashSet<ParameterInformation>()
        {
            new ParameterInformation(0, "The name of the directory or file.", true),
            new ParameterInformation("recursive", "r", "Whether files should be recursively removed.", false)
        };

        /// <inheritdoc/>
        public override String Description => "Removes a file or directory from the filesystem.";


        /// <summary> Remove a file or directory from the filesystem. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public RemoveFileProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(User executingUser, Dictionary<ParameterInformation, String> parameters)
        {
            String filename = parameters.First(x => x.Key.ShortName == "0").Value;

            String directoryPath = SOURCE.Files.GetWorkingDirectory(executingUser);
            String newDirectoryPath = StringExtensions.BuildAbsoluteFilepath(directoryPath, filename);
            Boolean isRecursive = parameters.Keys.Any(x => x.ShortName == "r");

            SOURCE.Files.RemoveFile(newDirectoryPath, isRecursive);
            return String.Empty;
        }
    }
}
