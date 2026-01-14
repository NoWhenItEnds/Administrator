using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Administrator.Subspace.Files
{
    /// <summary> A discrete, intractable file within the server's filesystem. </summary>
    public class File : IEquatable<File>
    {
        /// <summary> The file's full name including extension. </summary>
        [JsonPropertyName("name"), Required]
        public String Name { get; private set; }

        /// <summary> Represents whether the file contents should be read as raw text, a link to the user's filesystem, or a remote resource. </summary>
        [JsonPropertyName("sys_link"), Required]
        public SystemLink SysLink { get; private set; }

        /// <summary> The contents of the file. </summary>
        [JsonPropertyName("contents")]
        public String Contents { get; private set; } = String.Empty;

        /// <summary> When the file was created, in UTC. </summary>
        [JsonPropertyName("created_at"), Required]
        public DateTime CreatedAt { get; private set; }

        /// <summary> When the file was last modified, in UTC. </summary>
        [JsonPropertyName("modified-at"), Required]
        public DateTime ModifiedAt { get; private set; }


        /// <summary> A discrete, intractable file within the server's filesystem. </summary>
        /// <param name="name"> The file's full name including extension. </param>
        /// <param name="createdAt"> When the file was created, in UTC. </param>
        /// <param name="sysLink"> Represents whether the file contents should be read as raw text, a link to the user's filesystem, or a remote resource. </param>
        public File(String name, DateTime createdAt, SystemLink sysLink = SystemLink.TEXT)
        {
            Name = name;
            SysLink = sysLink;
            Contents = String.Empty;
            CreatedAt = createdAt;
            ModifiedAt = createdAt;
        }


        /// <summary> A discrete, intractable file within the server's filesystem. </summary>
        /// <param name="name"> The file's full name including extension. </param>
        /// <param name="contents"> The contents of the file. </param>
        /// <param name="createdAt"> When the file was created, in UTC. </param>
        /// <param name="sysLink"> Represents whether the file contents should be read as raw text, a link to the user's filesystem, or a remote resource. </param>
        public File(String name, String contents, DateTime createdAt, SystemLink sysLink = SystemLink.TEXT)
        {
            Name = name;
            SysLink = sysLink;
            Contents = contents;
            CreatedAt = createdAt;
            ModifiedAt = createdAt;
        }


        /// <summary> Get the file's extension. </summary>
        /// <returns> The file extension, sans '.'. A file with no extension will return an empty string. </returns>
        public String GetExtension()
        {
            String[] components = Name.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return components.Length > 1 ? components[^1] : String.Empty;
        }


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Name);


        /// <inheritdoc/>
        public Boolean Equals(File other) => Name == other.Name;
    }


    /// <summary> Represents whether the file contents should be read as raw text, a link to the user's filesystem, or a remote resource. </summary>
    public enum SystemLink
    {
        TEXT,
        LOCAL,
        REMOTE
    }
}
