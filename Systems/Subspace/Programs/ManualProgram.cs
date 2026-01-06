using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Administrator.Utilities.Exceptions;
using Administrator.Utilities.Extensions;

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
            new ParameterInformation(0, "The command name of the program to display information about.", true)
        };


        /// <summary> The format to use for displaying information about named parameters. </summary>
        private const String NAMED_PARAMETER_FORMAT = "[b]--{0} -{1}[/b] : {2}";

        /// <summary> The format to use for displaying information about positional parameters. </summary>
        private const String POSITIONAL_PARAMETER_FORMAT = "[b][{0}][/b] : {1}";


        /// <summary> Display a manual of use for the given program. </summary>
        /// <param name="source"> The server / computer the command originates from. </param>
        public ManualProgram(Server source) : base(source) { }


        /// <inheritdoc/>
        public override String ExecuteLogic(String directoryPath, Dictionary<ParameterInformation, String> parameters)
        {
            String programName = parameters.First(x => x.Key.ShortName == "0").Value;
            TerminalProgram? program = SOURCE.Programs.FirstOrDefault(x => x.Command == programName) ?? null;
            if (program != null)
            {
                StringBuilder result = new StringBuilder();
                result.AppendLine(program.Description);
                foreach (ParameterInformation parameter in program.Parameters)
                {
                    if (parameter.FullName.IsNumericOnly())
                    {
                        result.AppendLine(String.Format(POSITIONAL_PARAMETER_FORMAT, parameter.FullName, parameter.Description));
                    }
                    else
                    {
                        result.AppendLine(String.Format(NAMED_PARAMETER_FORMAT, parameter.FullName, parameter.ShortName, parameter.Description));
                    }
                }

                return result.ToString();
            }
            else
            {
                throw new TerminalException($"'{programName}' is not recognised as the name of an operable program, command, or script.");
            }
        }
    }
}
