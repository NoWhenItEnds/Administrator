using System;
using System.Collections.Generic;
using Administrator.Utilities.Singletons;
using Godot;

namespace Administrator.Managers
{
    /// <summary> The game's singleton audio manager. </summary>
    public partial class AudioManager : SingletonNode<AudioManager>
    {
        /// <summary> A dedicated audio player for music. </summary>
        [ExportGroup("Nodes")]
        [Export] private AudioStreamPlayer _musicPlayer;

        /// <summary> How many channels are available for playing incidental sound effects. </summary>
        /// <remarks> We want, by default, 8 channels. 1 dedicated music channel and 7 effect channels. </remarks>
        [ExportGroup("Settings")]
        [Export(PropertyHint.Range, "4,12")] private Int32 _effectChannels = 7;


        /// <summary> A reference to all the sound effect audio players. </summary>
        private List<AudioStreamPlayer> _effectPlayers = new List<AudioStreamPlayer>();

        /// <summary> An ordered array of currently queued effects. </summary>
        private Queue<AudioStream> _queuedSoundEffects = new Queue<AudioStream>();


        /// <inheritdoc/>
        public override void _Ready()
        {
            //  Populate the players.
            for (Int32 i = 0; i < _effectChannels; i++)
            {
                AudioStreamPlayer player = new AudioStreamPlayer();
                AddChild(player);
                _effectPlayers.Add(player);
            }
        }


        /// <summary> Queue a sound effect to be played by the audio manager. </summary>
        /// <param name="effect"> A reference to the sound file stream. </param>
        public void QueueSoundEffect(AudioStream effect) => _queuedSoundEffects.Enqueue(effect);


        /// <inheritdoc/>
        public override void _Process(Double delta)
        {
            // See if we need to play any currently queued sound effects.
            if (_queuedSoundEffects.Count > 1)
            {
                foreach (AudioStreamPlayer player in _effectPlayers)
                {
                    if (!player.Playing)
                    {
                        AudioStream effect = _queuedSoundEffects.Dequeue();
                        player.Stream = effect;
                        player.Play();

                        // Note: This will only load a single sound effect every frame. If there are multiple queued, they will have to wait until the next frame. This is intentional.
                        break;
                    }
                }
            }
        }
    }
}
