using System.Collections.Generic;
using Administrator.UI;
using Administrator.Utilities.Extensions;
using Administrator.Utilities.Singletons;
using Godot;

namespace Administrator.Managers
{
    /// <summary> The manager singleton for the game world's UI. </summary>
    public partial class UIManager : SingletonControl<UIManager>
    {
        /// <summary> The prefab to use for spawning terminal instances. </summary>
        [ExportGroup("Windows")]
        [Export] private PackedScene _terminalPrefab;


        /// <summary> An array of the currently active windows. </summary>
        private List<TilingWindow> _windows = new List<TilingWindow>();


        /// <inheritdoc/>
        public override void _Input(InputEvent @event)
        {
            // Check if the user is trying to input a command.
            if (@event is InputEventKey key && key.IsJustPressed() && key.IsCommandOrControlPressed())
            {
                switch(key.Keycode)
                {
                    case Key.Enter:     // Open the terminal.
                        break;
                    case Key.Q:         // Close the current window.
                        break;
                    case Key.D:         // Open application list.
                        break;
                }
            }
        }
    }
}
