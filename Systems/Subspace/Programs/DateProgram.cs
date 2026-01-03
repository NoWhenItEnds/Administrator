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
        public override HashSet<ParameterInformation> Parameters => new HashSet<ParameterInformation>()
        {
            { new ParameterInformation("utc", "u", "Convert local system time to UTC.", false) }
        };


        /// <summary> Gets information about the computer's local time. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public DateProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(String directoryPath, Dictionary<ParameterInformation, String> parameters)
        {
            DateTime time = DateTime.Now;

            foreach (KeyValuePair<ParameterInformation, String> parameter in parameters)
            {
                switch (parameter.Key.FullName)
                {
                    case "utc":
                        time = time.ToUniversalTime();
                        break;
                }
            }

            return time.ToString();
        }
    }
}
