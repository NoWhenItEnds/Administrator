using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Administrator.Subspace.Programs;
using Administrator.Utilities.Exceptions;
using Administrator.Utilities.Extensions;
using Godot;

namespace Administrator.Subspace
{
    /// <summary> A personal computer or server on a network. </summary>
    public class Computer
    {
        /// <summary> An array of programs available to the computer. </summary>
        public HashSet<TerminalProgram> Programs { get; private set; }  = new HashSet<TerminalProgram>();

        /// <summary>
        /// "[^"]*" : Matches a double quote, followed by any number of non-double quote characters, followed by a double quote.
        /// | : OR
        /// [^ ]+ : Matches one or more characters that are NOT a space.
        /// </summary>
        private const String INPUT_PATTERN = @"(""[^""]*""|[^ ]+)";

        public Computer()
        {
            Programs.Add(new ManualProgram(this));
        }

        public String SubmitCommand(String command)
        {
            String[] formattedCommand = Regex.Matches(command, INPUT_PATTERN)
                .Cast<Match>()
                .Select(m => m.Value.Trim('"'))
                .ToArray();

            // Split the command into file-based sections.
            IEnumerable<String>[] fileSplit = formattedCommand.SplitArray(">>").ToArray();
            if (fileSplit.Count() == 1)
            {
                fileSplit = formattedCommand.SplitArray(">").ToArray();
            }

            // There will always be a initial part even if the result won't be piped into a file.
            IEnumerable<String>[] pipeSplit = fileSplit[0].SplitArray("|").ToArray();

            String result = String.Empty;
            try
            {
                foreach (IEnumerable<String> section in pipeSplit)
                {
                    String[] sectionArray = section.ToArray();
                    List<String> arguments = new List<String>(sectionArray[1..]);
                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        arguments.Insert(0, result);
                    }
                    result = DoCommand(section.First(), arguments.ToArray());
                }
            }
            catch (TerminalException exception)
            {
                return exception.Message;
            }


            if (fileSplit.Count() > 1)  // If there's a file component to pipe the result into.
            {

            }

            return result;
        }


        private String DoCommand(String command, String[] arguments)
        {
            String response = $"'{command[0]}' is not recognised as the name of an operable program, command, or script.";

            TerminalProgram? program = Programs.FirstOrDefault(x => x.Command == command) ?? null;
            if (program != null)
            {
                response = program.Execute(arguments);
            }

            return response;
        }
    }
}
