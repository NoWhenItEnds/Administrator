using System;
using System.Collections.Generic;
using System.Linq;
using Administrator.Utilities.Extensions;

namespace Administrator.Subspace.Programs
{
    /// <summary> Change the user's current working directory on the filesystem. </summary>
    public class ChangeDirectoryProgram : TerminalProgram
    {
        /// <inheritdoc/>
        public override String Command => "cd";

        /// <inheritdoc/>
        public override HashSet<ParameterInformation> Parameters => new HashSet<ParameterInformation>()
        {
            new ParameterInformation(0, "The name of the directory to move to.", true)
        };

        /// <inheritdoc/>
        public override String Description => "Change the current working directory.";


        /// <summary> Change the user's current working directory on the filesystem. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public ChangeDirectoryProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(User executingUser, Dictionary<ParameterInformation, String> parameters)
        {
            String directoryName = parameters.First(x => x.Key.ShortName == "0").Value;

            String directoryPath = SOURCE.Files.GetWorkingDirectory(executingUser);
            String newDirectoryPath = StringExtensions.BuildAbsoluteFilepath(directoryPath, directoryName);

            SOURCE.Files.SetUserWorkingDirectory(executingUser, newDirectoryPath);
            return String.Empty;
        }
    }
}
