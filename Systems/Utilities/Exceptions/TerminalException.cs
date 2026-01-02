using System;

namespace Administrator.Utilities.Exceptions
{
    /// <summary> An exception thrown by a terminal. </summary>
    public class TerminalException : Exception
    {
        /// <summary> An exception thrown by a terminal. </summary>
        public TerminalException() { }


        /// <summary> An exception thrown by a terminal. </summary>
        /// <param name="message"> The exception's error message. </param>
        public TerminalException(String message) : base(message) { }


        /// <summary> An exception thrown by a terminal. </summary>
        /// <param name="message"> The exception's error message. </param>
        /// <param name="innerException"> The exception wrapped by this one. </param>
        public TerminalException(String message, Exception innerException) : base(message, innerException) { }
    }
}
