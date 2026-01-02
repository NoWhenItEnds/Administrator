using Godot;

namespace Administrator.Managers
{
    /// <summary> The central parent singleton for the game world. </summary>
    public partial class GameManager : Node
    {
        /// <inheritdoc/>
        public override void _Ready()
        {
            GD.Print("Hello, World!");
        }
    }
}
