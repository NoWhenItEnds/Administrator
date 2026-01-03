using System;
using System.Collections.Generic;

namespace Administrator.Subspace.Programs
{
    /// <summary> Gets information about the computer's local time. </summary>
    public class DateProgram : TerminalProgram
    {
        /// <inheritdoc/>
        public override String Command => "date";

        /// <inheritdoc/>
        public override String Description => "Displays the current system time.";

        /// <inheritdoc/>
        public override Dictionary<String, Boolean> Parameters => new Dictionary<String, Boolean>()
        {
            { "u", false },
            { "s", false },
            { "r", true }
        };

        /// <inheritdoc/>
        public override Int32[] NumberOfPositionalArguments => [ 0 ];


        /// <summary> Gets information about the computer's local time. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public DateProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(String directoryPath, Dictionary<String, String?> parameters, String[] positionalArguments)
        {
            DateTime time = DateTime.Now;

            if (parameters.ContainsKey("u"))
            {
                time = time.ToUniversalTime();
            }

            return time.ToString();
        }
    }
}
