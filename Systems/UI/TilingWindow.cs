using Godot;

namespace Administrator.UI
{
    /// <summary> Represents a UI control element that tiles like i3. </summary>
    public abstract partial class TilingWindow : PanelContainer
    {
        /// <summary> Initialise the new window. </summary>
        /// <param name="position"> The position to initially spawn the window. </param>
        public void Initialise(Rect2 position)
        {
            UpdatePosition(position);
        }


        /// <summary> Update the window's position. </summary>
        /// <param name="position"> A rect object containing the new position values. </param>
        public void UpdatePosition(Rect2 position)
        {
            GlobalPosition = position.Position;
            Size = position.Size;
        }
    }
}
