using Godot;

namespace Administrator.Resources
{
    /// <summary> A mapping between a key and a sound that should play. </summary>
    [GlobalClass]
    public partial class KeySound : Resource
    {
        /// <summary> The key / keycode for the mapping. </summary>
        [Export] public Key Keycode { get; set; } = Key.None;

        /// <summary> An array of sounds that should be chosen from when the key is first pressed. </summary>
        [Export] public Godot.Collections.Array<AudioStream> PressedSounds { get; set; } = new Godot.Collections.Array<AudioStream>();

        /// <summary> An array of sounds that should be chosen from when the key is first released. </summary>
        [Export] public Godot.Collections.Array<AudioStream> ReleasedSounds { get; set; } = new Godot.Collections.Array<AudioStream>();

        public KeySound() { }
    }
}
