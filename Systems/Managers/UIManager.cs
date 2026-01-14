using System;
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

        private TilingWindow? _focusedWindow = null;    // TODO - Hacky. Remove with Godot's focus system.


        /// <inheritdoc/>
        public override void _Input(InputEvent @event)
        {
            // Check if the user is trying to input a command.
            if (@event is InputEventKey key && key.IsJustPressed() && key.IsCommandOrControlPressed())
            {
                switch(key.Keycode)
                {
                    case Key.Enter:     // Open the terminal.
                        CreateTerminal(_focusedWindow);
                        break;
                    case Key.Q:         // Close the current window.
                        break;
                    case Key.D:         // Open application list.
                        break;
                }
            }
        }


        private void CreateTerminal(TilingWindow? focusWindow)
        {
            Terminal terminal = _terminalPrefab.Instantiate<Terminal>();
            _windows.Add(terminal);
            AddChild(terminal);

            Rect2 newWindowPosition = GetRect();    // Use the whole window if there isn't a focused window.
            if (focusWindow != null)
            {
                Rect2[] windowPositions = CalculateWindowPositions(focusWindow.GetRect());
                focusWindow.UpdatePosition(windowPositions[0]); // The focused window should be set to use the first new position.
                newWindowPosition = windowPositions[1];
            }

            terminal.Initialise(newWindowPosition);
            _focusedWindow = terminal;
        }


        /// <summary> Calculate new window positions by splitting the original. </summary>
        /// <param name="original"> The original position to split. </param>
        /// <param name="splitPosition"> A percentage of the window where it should be split. </param>
        /// <returns> An ordered array of new positions for windows. The first entry will represent the original. </returns>
        private Rect2[] CalculateWindowPositions(Rect2 original, Single splitPosition = 0.5f)
        {
            Rect2[] results = new Rect2[2];
            Rect2 absoluteOriginal = original.Abs();    // Ensure we handle rects with 'negative' dimensions.

            if (absoluteOriginal.Size.X >= absoluteOriginal.Size.Y) // Split horizontally.
            {
                Single split = absoluteOriginal.Position.X + absoluteOriginal.Size.X * splitPosition;
                results[0] = new Rect2(absoluteOriginal.Position.X, absoluteOriginal.Position.Y, split - absoluteOriginal.Position.X, absoluteOriginal.Size.Y);
                results[1] = new Rect2(split, absoluteOriginal.Position.Y, absoluteOriginal.End.X - split, absoluteOriginal.Size.Y);
            }
            else                                                    // Split vertically.
            {
                Single split = absoluteOriginal.Position.Y + absoluteOriginal.Size.Y * splitPosition;
                results[0] = new Rect2(absoluteOriginal.Position.X, absoluteOriginal.Position.Y, absoluteOriginal.Size.X, split - absoluteOriginal.Position.Y);
                results[1] = new Rect2(absoluteOriginal.Position.X, split, absoluteOriginal.Size.X, absoluteOriginal.End.Y - split);
            }

            return results;
        }
    }
}
