using Administrator.Subspace;
using Administrator.Utilities.Singletons;
using Godot;

namespace Administrator.Managers
{
    /// <summary> The central parent singleton for the game world. </summary>
    public partial class GameManager : SingletonNode<GameManager>
    {
        // TODO - Move to player's system?
        private Computer _playerComputer = new Computer();


        /// <inheritdoc/>
        public override void _Ready()
        {
            GD.Print("Hello, World!");
        }
    }
}
