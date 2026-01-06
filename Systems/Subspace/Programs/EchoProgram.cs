using System;
using System.Collections.Generic;
using System.Linq;

namespace Administrator.Subspace.Programs
{
    /// <summary> Prints the given argument to the console. </summary>
    public class EchoProgram : TerminalProgram
    {
        /// <inheritdoc/>
        public override String Command => "echo";

        /// <inheritdoc/>
        public override HashSet<ParameterInformation> Parameters => new HashSet<ParameterInformation>()
        {
            new ParameterInformation(0, "The text to print to the terminal.", true)
        };

        /// <inheritdoc/>
        public override String Description => "Prints the given text to the terminal.";


        /// <summary> Prints the given argument to the console. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public EchoProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(String directoryPath, Dictionary<ParameterInformation, String> parameters)
        {
            return parameters.First(x => x.Key.ShortName == "0").Value;
        }
    }
}
