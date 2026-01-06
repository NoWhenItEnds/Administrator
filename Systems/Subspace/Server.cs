using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Administrator.Subspace.Files;
using Administrator.Subspace.Programs;
using Administrator.Utilities.Exceptions;
using Administrator.Utilities.Extensions;
using Godot;

namespace Administrator.Subspace
{
    /// <summary> A server on a network. </summary>
    public class Server
    {
        /// <summary> The server's filesystem. </summary>
        public readonly FileSystem Files = new FileSystem(["admin"]);

        /// <summary> An array of programs available to the server. </summary>
        public readonly HashSet<TerminalProgram> Programs  = new HashSet<TerminalProgram>();

        /// <summary>
        /// "[^"]*" : Matches a double quote, followed by any number of non-double quote characters, followed by a double quote.
        /// | : OR
        /// [^ ]+ : Matches one or more characters that are NOT a space.
        /// </summary>
        private const String INPUT_PATTERN = @"(""[^""]*""|[^ ]+)";


        /// <summary> A server on a network. </summary>
        public Server()
        {
            Programs.Add(new CreateDirectoryProgram(this));
            Programs.Add(new DateProgram(this));
            Programs.Add(new EchoProgram(this));
            Programs.Add(new ListProgram(this));
            Programs.Add(new ManualProgram(this));
            Programs.Add(new RemoveFileProgram(this));
        }

        public String SubmitCommand(String directoryPath, String command)
        {
            String[] formattedCommand = Regex.Matches(command, INPUT_PATTERN)
                .Cast<Match>()
                .Select(m => m.Value.Trim('"'))
                .ToArray();

            // Split the command into file-based sections.
            // TODO - Implement file piping. And fix this shit.
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
                    result = DoCommand(directoryPath, section.First(), arguments.ToArray());
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


        private String DoCommand(String directoryPath, String command, String[] arguments)
        {
            String response = $"'{command}' is not recognised as the name of an operable program, command, or script.";

            TerminalProgram? program = Programs.FirstOrDefault(x => x.Command == command) ?? null;
            if (program != null)
            {
                response = program.Execute(directoryPath, arguments);
            }

            return response;
        }
    }
}
