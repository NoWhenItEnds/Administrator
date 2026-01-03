using System;
using System.Collections.Generic;
using Administrator.Utilities.Exceptions;

namespace Administrator.Subspace.Programs
{
    /// <summary> A program that can be run via the terminal. </summary>
    public abstract class TerminalProgram
    {
        /// <summary> The command that runs the program. </summary>
        public abstract String Command { get; }

        /// <summary> A map of parameter names and whether they take a value. </summary>
        public abstract Dictionary<String, Boolean> Parameters { get; } // TODO - Should include description for man.

        /// <summary> How many positional arguments the program expects. </summary>
        public abstract Int32 NumberOfPositionalArguments { get; }

        /// <summary> Descriptive text describing the program / command. </summary>
        public abstract String Manual { get; }


        /// <summary> The server / computer the program originates from. </summary>
        protected readonly Computer SOURCE;


        /// <summary> A program that can be run via the terminal. </summary>
        /// <param name="source"> The server / computer the program originates from. </param>
        public TerminalProgram(Computer source)
        {
            SOURCE = source;
        }


        /// <summary> The logic that is run when executing the program. </summary>
        /// <param name="parameters"> The named parameter key / value pairs given to the program. </param>
        /// <param name="positionalArguments"> The positional arguments given to the program. </param>
        /// <returns> The result to print to the terminal. </returns>
        public abstract String ExecuteLogic(Dictionary<String, String?> parameters, String[] positionalArguments);


        /// <summary> Execute the program. </summary>
        /// <param name="arguments"> The arguments given to the program. </param>
        /// <returns> The result to print to the terminal. </returns>
        public String Execute(String[] arguments)
        {
            (Dictionary<String, String?> parameters, String[] positionalArguments) result = ParseParameters(arguments);
            return ExecuteLogic(result.parameters, result.positionalArguments);
        }


        /// <summary> Validate / filter the arguments input. </summary>
        /// <param name="rawArguments"> The array of raw, unfiltered arguments. </param>
        /// <returns> A tuple containing a map of the parameter names and their value (if they have one), and an array of the standalone arguments. </returns>
        /// <exception cref="TerminalException"/>
        private (Dictionary<String, String?> parameters, String[] positionalArguments) ParseParameters(String[] rawArguments)
        {
            Dictionary<String, String?> parameters = new Dictionary<String, String?>();
            List<String> standaloneArguments = new List<String>();

            for (Int32 i = 0; i < rawArguments.Length; i++)
            {
                String current = rawArguments[i];

                if (current[0] == '-')                  // If the argument is a name, its a parameter.
                {
                    String cleanedParameter = current.Remove(0, 1);    // Remove the '-'.

                    // Check if the parameter exists.
                    if (!Parameters.ContainsKey(cleanedParameter))
                    {
                        throw new TerminalException($"Cannot bind unknown parameter '{cleanedParameter}'.");
                    }
                    else
                    {
                        if (Parameters[cleanedParameter])   // Add the parameter and its value.
                        {
                            parameters.Add(cleanedParameter, rawArguments[i + 1]);
                            i++;
                        }
                        else                                // Just add the parameter's name.
                        {
                            parameters.Add(cleanedParameter, null);
                        }
                    }
                }
                else                                        // Otherwise it's a positional argument.
                {
                    standaloneArguments.Add(current);
                }
            }

            // Check the positional arguments.
            if (standaloneArguments.Count != NumberOfPositionalArguments)
            {
                throw new TerminalException($"Incorrect number of positional arguments. Expected '{NumberOfPositionalArguments}', received '{standaloneArguments.Count}' ({String.Join(',', standaloneArguments)}).");
            }

            return new (parameters, standaloneArguments.ToArray());
        }
    }
}
