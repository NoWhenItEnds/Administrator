using System;
using System.Collections.Generic;
using System.Linq;
using Administrator.Utilities.Exceptions;

namespace Administrator.Subspace.Programs
{
    /// <summary> Display a manual of use for the given program. </summary>
    public class ManualProgram : TerminalProgram
    {
        /// <inheritdoc/>
        public override string Command => "man";

        /// <inheritdoc/>
        public override String Description => "Displays a manual of use for the given command.";

        /// <inheritdoc/>
        public override HashSet<ParameterInformation> Parameters => new HashSet<ParameterInformation>()
        {
            new ParameterInformation(0, "The command name of the program to display information about.")
        };


        /// <summary> Display a manual of use for the given program. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public ManualProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(String directoryPath, Dictionary<ParameterInformation, String> parameters)
        {
            String? programName = parameters.FirstOrDefault(x => x.Key.ShortName == "0").Value ?? null;
            if (programName != null)
            {
                TerminalProgram? program = SOURCE.Programs.FirstOrDefault(x => x.Command == programName) ?? null;
                if (program != null)
                {
                    return program.Description;
                }
                else
                {
                    throw new TerminalException($"'{programName}' is not recognised as the name of an operable program, command, or script.");
                }
            }
            else
            {
                throw new TerminalException($"Expected the name of an operable program, command, or script. Yet one wasn't received.");
            }
        }
    }
}
