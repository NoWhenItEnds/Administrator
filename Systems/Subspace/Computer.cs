using System;
using Administrator.Subspace.Components;

namespace Administrator.Subspace
{
    /// <summary> A personal computer or server on a network. </summary>
    public class Computer
    {
        /// <summary> The computer's personal language server. </summary>
        private LanguageServer _languageServer = new LanguageServer();


        public String SubmitCommand(String command)
        {
            return "TEST";
        }
    }
}
