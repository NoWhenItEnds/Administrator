using System;
using System.Collections.Generic;
using System.Linq;
using Administrator.Utilities.Exceptions;

namespace Administrator.Subspace.Programs
{
    /// <summary> A program that can be run via the terminal. </summary>
    public abstract class TerminalProgram
    {
        /// <summary> The command that runs the program. </summary>
        public abstract String Command { get; }

        /// <summary> A map of parameter information. </summary>
        public abstract HashSet<ParameterInformation> Parameters { get; }

        /// <summary> Descriptive text describing the program / command. </summary>
        public abstract String Description { get; }


        /// <summary> The server / computer the program originates from. </summary>
        protected readonly Server SOURCE;


        /// <summary> A program that can be run via the terminal. </summary>
        /// <param name="source"> The server / computer the program originates from. </param>
        public TerminalProgram(Server source)
        {
            SOURCE = source;
        }


        /// <summary> The logic that is run when executing the program. </summary>
        /// <param name="executingUser"> A reference to the user executing the command. </param>
        /// <param name="parameters"> The parameter key / value pairs given to the program. </param>
        /// <returns> The result to print to the terminal. </returns>
        public abstract String ExecuteLogic(User executingUser, Dictionary<ParameterInformation, String> parameters);


        /// <summary> Execute the program. </summary>
        /// <param name="executingUser"> A reference to the user executing the command. </param>
        /// <param name="arguments"> The arguments given to the program. </param>
        /// <returns> The result to print to the terminal. </returns>
        public String Execute(User executingUser, String[] arguments)
        {
            Dictionary<ParameterInformation, String> parameters = ParseParameters(arguments);
            ValidateParameters(parameters.Keys.ToArray());
            return ExecuteLogic(executingUser, parameters);
        }


        /// <summary> Validate / filter the arguments input. </summary>
        /// <param name="rawArguments"> The array of raw, unfiltered arguments. </param>
        /// <returns> A map of the parameter information bound to the value. </returns>
        /// <exception cref="TerminalException"/>
        private Dictionary<ParameterInformation, String> ParseParameters(String[] rawArguments)
        {
            Dictionary<ParameterInformation, String> boundParameters = new Dictionary<ParameterInformation, String>();
            Int32 currentPositionalArgument = 0;

            for (Int32 i = 0; i < rawArguments.Length; i++)
            {
                String current = rawArguments[i];

                if (current[0] == '-')                  // If the argument is a name...
                {
                    ParameterInformation? boundParameter = null;
                    if (current[1] == '-')              // If it has "--" then it's the verbose name.
                    {
                        String cleanedName = current.Remove(0, 2);
                        boundParameter = Parameters.FirstOrDefault(x => x.FullName == cleanedName) ?? null;
                    }
                    else
                    {
                        String cleanedName = current.Remove(0, 1);
                        boundParameter = Parameters.FirstOrDefault(x => x.ShortName == cleanedName) ?? null;
                    }

                    // We've found the parameter. Now attempt to bind it.
                    if (boundParameter != null)
                    {
                        if (boundParameter.ExpectsValue)        // Try to provide a value to the parameter if it requires it.
                        {
                            if (i + 1 < rawArguments.Length)   // Check we can provide a value.
                            {
                                String argument = rawArguments[i + 1];
                                if (argument[0] != '-')         // Check we DO provide a value.
                                {
                                    boundParameters.Add(boundParameter, argument);
                                    i++;
                                }
                                else
                                {
                                    throw new TerminalException($"Cannot bind parameter '{boundParameter.FullName}' to the given value '{argument}'. Expected a value.");
                                }

                            }
                            else
                            {
                                throw new TerminalException($"Cannot bind parameter '{boundParameter.FullName}'. It expects a value and one wasn't provided.");
                            }

                        }
                        else
                        {
                            boundParameters.TryAdd(boundParameter, String.Empty);
                        }

                    }
                    else
                    {
                        throw new TerminalException($"Cannot bind unknown parameter '{current}'.");
                    }
                }
                else    // Otherwise it's a positional argument.
                {
                    ParameterInformation? boundParameter = Parameters.FirstOrDefault(x => x.ShortName == currentPositionalArgument.ToString()) ?? null;
                    if (boundParameter != null)
                    {
                        boundParameters.Add(boundParameter, current);
                        currentPositionalArgument++;
                    }
                    else
                    {
                        throw new TerminalException($"The given positional argument at index {currentPositionalArgument} isn't supported by the program.");
                    }
                }
            }

            return boundParameters;
        }


        /// <summary> A guard clause to ensure that the program was given all the necessary parameters. </summary>
        /// <param name="parameters"></param>
        private void ValidateParameters(ParameterInformation[] parameters)
        {
            foreach (ParameterInformation parameter in Parameters)
            {
                if (parameter.IsRequired && !parameters.Contains(parameter))
                {
                    throw new TerminalException($"Parameter '{parameter.FullName}' is required yet wasn't provided.");
                }
            }
        }
    }
}
