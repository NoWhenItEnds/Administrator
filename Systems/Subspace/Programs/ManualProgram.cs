using System;
using System.Collections.Generic;
using System.Linq;

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
        public override Dictionary<String, Boolean> Parameters => new Dictionary<String, Boolean>();

        /// <inheritdoc/>
        public override Int32[] NumberOfPositionalArguments => [ 1 ];


        /// <summary> Display a manual of use for the given program. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public ManualProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(String directoryPath, Dictionary<string, string?> parameters, string[] positionalArguments)
        {
            String response = $"'{positionalArguments[0]}' is not recognised as the name of an operable program, command, or script.";

            TerminalProgram? program = SOURCE.Programs.FirstOrDefault(x => x.Command == positionalArguments[0]) ?? null;
            if (program != null)
            {
                response = program.Description;
            }

            return response;
        }
    }
}
