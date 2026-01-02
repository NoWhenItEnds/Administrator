using System;
using System.Collections.Generic;

namespace Administrator.Subspace.Programs
{
    /// <summary> Gets information about the computer's local time. </summary>
    public class TimeProgram : TerminalProgram
    {
        /// <inheritdoc/>
        public override String Command => "date";

        /// <inheritdoc/>
        public override String Manual => throw new NotImplementedException();

        /// <inheritdoc/>
        public override Dictionary<String, Boolean> Parameters => new Dictionary<String, Boolean>()
        {
            { "u", false },
            { "s", false },
            { "r", true }
        };

        /// <inheritdoc/>
        public override Int32 NumberOfPositionalArguments => 0;


        /// <summary> Gets information about the computer's local time. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public TimeProgram(Computer source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(Dictionary<string, string?> parameters, string[] positionalArguments)
        {
            throw new NotImplementedException();
        }
    }
}
