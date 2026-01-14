using System;
using System.Linq;
using Administrator.Resources;
using Administrator.Subspace;
using Administrator.Utilities.Extensions;
using Administrator.Utilities.Singletons;
using Godot;

namespace Administrator.Managers
{
    /// <summary> The central parent singleton for the game world. </summary>
    public partial class GameManager : SingletonNode<GameManager>
    {
        /// <summary> An mapping between specific keys and the sound they play. </summary>
        /// <remarks> Key.None indicates that the sound it common and should be used as a default case for keys not already defined. </remarks>
        private KeySound[] _keyboardSounds = Array.Empty<KeySound>();

        // TODO - Move to player's system?
        public Server PlayerComputer { get; private set; } = new Server();


        /// <inheritdoc/>
        public override void _Ready()
        {
            GD.Print("Hello, World!");

            _keyboardSounds = ResourceExtensions.GetResources<KeySound>();
        }


        /// <inheritdoc/>
        public override void _Input(InputEvent @event)
        {
            // Play keyboard sound.
            HandleSoundInput(@event);   // TODO - Should this live here? Or on a player manager of some sort? Probably alongside the player computer.
        }


        /// <summary> Check the provided input to determine if we should play a sound. </summary>
        /// <param name="event"> The input event to assess. </param>
        private void HandleSoundInput(InputEvent @event)
        {
            if (@event is InputEventKey key)
            {
                // Try to get the specific key, or the default 'None'.
                KeySound? keySound = _keyboardSounds.FirstOrDefault(x => x.Keycode == key.Keycode) ?? _keyboardSounds.FirstOrDefault(x => x.Keycode == Key.None);
                if (keySound != null)
                {
                    AudioStream? sound = null;
                    if (key.IsJustPressed())
                    {
                        sound = keySound.PressedSounds.PickRandom();
                    }
                    else if (key.IsJustReleased())
                    {
                        sound = keySound.ReleasedSounds.PickRandom();
                    }

                    if (sound != null)
                    {
                        AudioManager.Instance.QueueSoundEffect(sound);
                    }
                }
            }
        }
    }
}
