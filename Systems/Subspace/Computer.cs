using System;
using System.Collections.Generic;
using System.Linq;
using Administrator.Subspace.Programs;
using Godot;

namespace Administrator.Subspace
{
    /// <summary> A personal computer or server on a network. </summary>
    public class Computer
    {
        /// <summary> An array of programs available to the computer. </summary>
        public HashSet<TerminalProgram> Programs { get; private set; }  = new HashSet<TerminalProgram>();


        public Computer()
        {
            Programs.Add(new ManualProgram(this));
        }

        public String SubmitCommand(String command)
        {
            // Handle operation order. File -> Pipe -> Command.
            String[] fileSplit = command.Split(">>", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (fileSplit.Length == 1)  // If there isn't append, try overwrite.
            {
                fileSplit = command.Split('>', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            }

            // Check that we don't have too many commands.
            if (fileSplit.Length > 2)
            {
                return "The output for this command is already being redirected.";
            }

            String[] pipeSplit = fileSplit[0].Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String previousResult = String.Empty;
            foreach (String current in pipeSplit)
            {
                String[] commandSplit = command.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if (!String.IsNullOrWhiteSpace(previousResult))
                {
                    List<String> modifiedCommand = new List<String>(commandSplit);
                    modifiedCommand.Insert(1, previousResult);  // Insert the piped value as the first argument.
                    commandSplit = modifiedCommand.ToArray();
                }

                previousResult = DoCommand(commandSplit);
            }

            return previousResult;
        }


        private String DoCommand(String[] command)
        {
            String response = $"'{command[0]}' is not recognised as the name of an operable program, command, or script.";

            TerminalProgram? program = Programs.FirstOrDefault(x => x.Command == command[0]) ?? null;
            if (program != null)
            {
                response = program.Execute(command[1..]);
            }

            return response;
        }
    }
}
