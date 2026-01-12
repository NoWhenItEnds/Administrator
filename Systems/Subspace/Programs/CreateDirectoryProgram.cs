using System;
using System.Collections.Generic;
using System.Linq;
using Administrator.Utilities.Extensions;

namespace Administrator.Subspace.Programs
{
    /// <summary> Create a new directory on the filesystem. </summary>
    public class CreateDirectoryProgram : TerminalProgram
    {
        /// <inheritdoc/>
        public override String Command => "mkdir";

        /// <inheritdoc/>
        public override HashSet<ParameterInformation> Parameters => new HashSet<ParameterInformation>()
        {
            new ParameterInformation(0, "The name of the new directory.", true),
            new ParameterInformation("recursive", "r", "Whether directories should be recursively created.", false)
        };

        /// <inheritdoc/>
        public override String Description => "Create a new directory.";


        /// <summary> Create a new directory on the filesystem. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public CreateDirectoryProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(String directoryPath, Dictionary<ParameterInformation, String> parameters)
        {
            String directoryName = parameters.First(x => x.Key.ShortName == "0").Value;
            String newDirectoryPath = StringExtensions.BuildAbsoluteFilepath(directoryPath, directoryName);
            Boolean isRecursive = parameters.Keys.Any(x => x.ShortName == "r");
            SOURCE.Files.CreateDirectory(newDirectoryPath, isRecursive);
            return String.Empty;
        }
    }
}
