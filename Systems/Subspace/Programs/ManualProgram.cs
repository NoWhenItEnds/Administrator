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
        public override String Manual => "Display a manual of use for the given command.";

        /// <inheritdoc/>
        public override Dictionary<String, Boolean> Parameters => new Dictionary<String, Boolean>();

        /// <inheritdoc/>
        public override int NumberOfPositionalArguments => 1;


        /// <summary> Display a manual of use for the given program. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public ManualProgram(Computer source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(Dictionary<string, string?> parameters, string[] positionalArguments)
        {
            String response = $"'{positionalArguments[0]}' is not recognised as the name of an operable program, command, or script.";

            TerminalProgram? program = SOURCE.Programs.FirstOrDefault(x => x.Command == positionalArguments[0]) ?? null;
            if (program != null)
            {
                response = program.Manual;
            }

            return response;
        }
    }
}
