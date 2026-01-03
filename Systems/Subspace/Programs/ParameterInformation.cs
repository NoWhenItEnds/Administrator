using System;

namespace Administrator.Subspace.Programs
{
    /// <summary> Information that helps define a program's parameter. </summary>
    public class ParameterInformation : IEquatable<ParameterInformation>
    {
        /// <summary> The parameter's verbose name. </summary>
        public String FullName { get; init; }

        /// <summary> The parameter's shortened name. </summary>
        public String ShortName { get; init; }

        /// <summary> A description about the parameter's use. </summary>
        public String Description { get; init; }

        /// <summary> Whether the parameter expects a value to be bound to it, or whether it acts as a flag. </summary>
        public Boolean ExpectsValue { get; init; }


        /// <summary> Information that helps define a program's parameter. </summary>
        /// <param name="fullName"> The parameter's verbose name. </param>
        /// <param name="shortName"> The parameter's shortened name. </param>
        /// <param name="description"> A description about the parameter's use. </param>
        /// <param name="expectsValue"> Whether the parameter expects a value to be bound to it, or whether it acts as a flag. </param>
        public ParameterInformation(String fullName, String shortName, String description, Boolean expectsValue)
        {
            FullName = fullName;
            ShortName = shortName;
            Description = description;
            ExpectsValue = expectsValue;
        }


        /// <summary> Information that helps define a program's parameter. </summary>
        /// <param name="position"> The position for the positional parameter. </param>
        /// <param name="description"> A description about the parameter's use. </param>
        public ParameterInformation(Int32 position, String description)
        {
            FullName = position.ToString();
            ShortName = position.ToString();
            Description = description;
            ExpectsValue = true;
        }


        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            return HashCode.Combine(FullName, ShortName);
        }


        /// <inheritdoc/>
        public Boolean Equals(ParameterInformation other) => FullName == other.FullName || ShortName == other.ShortName;
    }
}
