using Administrator.Utilities.Singletons;
using Godot;

namespace Administrator.Managers
{
    /// <summary> The central parent singleton for the game world. </summary>
    public partial class GameManager : SingletonNode<GameManager>
    {
        /// <inheritdoc/>
        public override void _Ready()
        {
            GD.Print("Hello, World!");
        }
    }
}
