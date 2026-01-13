using System;

namespace Administrator.Subspace
{
    /// <summary> A persistent user in a server. </summary>
    public class User : IEquatable<User>
    {
        /// <summary> The user's display name. </summary>
        public String Username { get; init; }


        /// <summary> A persistent user in a server. </summary>
        /// <param name="username"> The user's display name. </param>
        public User(String username)
        {
            Username = username;
        }


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Username);


        /// <inheritdoc/>
        public Boolean Equals(User? other) => other != null ? Username == other.Username : false;
    }
}
