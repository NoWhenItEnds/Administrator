using System;
using System.Collections.Generic;

namespace Administrator.Subspace.Programs
{
    /// <summary> Prints the given argument to the console. </summary>
    public class EchoProgram : TerminalProgram
    {
        /// <inheritdoc/>
        public override String Command => "echo";

        /// <inheritdoc/>
        public override Dictionary<String, Boolean> Parameters => new Dictionary<String, Boolean>();

        /// <inheritdoc/>
        public override Int32 NumberOfPositionalArguments => 1;

        /// <inheritdoc/>
        public override String Manual => throw new NotImplementedException();


        /// <summary> Prints the given argument to the console. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public EchoProgram(Computer source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(Dictionary<String, String?> parameters, String[] positionalArguments)
        {
            return positionalArguments[0];
        }
    }
}
