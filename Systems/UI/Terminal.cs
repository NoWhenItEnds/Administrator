using System;
using Administrator.Managers;
using Administrator.Utilities;
using Godot;

namespace Administrator.UI
{
    /// <summary> The terminal UI the user uses to interact with the game world. </summary>
    public partial class Terminal : PanelContainer
    {
        /// <summary> The container that holds both the results of the previous user input and the input node. </summary>
        [ExportGroup("Nodes")]
        [Export] private VBoxContainer _outputContainer;

        /// <summary> The input field the user uses to type commands to the terminal. </summary>
        [Export] private LineEdit _inputNode;


        /// <summary> How many labels our object pool can contain. </summary>
        [ExportGroup("Settings")]
        [Export] private Int32 _outputPoolSize = 100;


        /// <summary> The prefab used to spawn additional output labels. </summary>
        [ExportGroup("Resources")]
        [Export] private PackedScene _outputLabelPrefab;


        /// <summary> The user's current working directory. </summary>
        private String _pwdText = "/home/usr";

        /// <summary> The user's current input. </summary>
        private String _currentInput = String.Empty;

        private ObjectPool<RichTextLabel> _outputPool;

        /// <summary> The format to render the input text. </summary>
        private const String INPUT_FORMAT = "{0}{1} {2}";

        /// <summary> The symbol to use for separating the PWD from the input. </summary>
        private const Char PWD_SYMBOL = '>';


        /// <inheritdoc/>
        public override void _Ready()
        {
            // Check our exports.
            ArgumentNullException.ThrowIfNull(_outputContainer);
            ArgumentNullException.ThrowIfNull(_inputNode);

            _inputNode.TextChanged += OnInputChanged;
            _inputNode.TextSubmitted += OnInputSubmitted;

            _outputPool = new ObjectPool<RichTextLabel>(_outputContainer, _outputLabelPrefab, _outputPoolSize);
            _inputNode.Text = String.Format(INPUT_FORMAT, _pwdText, PWD_SYMBOL, String.Empty);
        }


        /// <summary> Handle the text being changed. </summary>
        /// <param name="newText"> The input's current text. </param>
        private void OnInputChanged(String newText)
        {
            String[] input = newText.Split($"{PWD_SYMBOL} ");

            if (input.Length > 2)       // Prevent any silly business by collapsing text that is split beyond the initial pwd.
            {
                for (Int32 i = 2; i < input.Length; i++)
                {
                    input[1] += input[i];
                }
            }
            else if (input.Length > 1)  // Otherwise, update the text.
            {
                _currentInput = input[1];
            }
            else                        // Otherwise, reset the text.
            {
                _currentInput = String.Empty;
            }

            // Prevent deleting the pwd.
            if (input[0] != _pwdText)
            {
                _inputNode.Text = String.Format(INPUT_FORMAT, _pwdText, PWD_SYMBOL, _currentInput);
                _inputNode.CaretColumn = _inputNode.Text.Length;
            }
        }


        /// <summary> Handle the text being submitted. </summary>
        /// <param name="submittedText"> The full submission text. </param>
        private void OnInputSubmitted(String submittedText)
        {
            if(!String.IsNullOrWhiteSpace(_currentInput))
            {
                // First record the command.
                RichTextLabel commandLabel = _outputPool.GetAvailableObject();
                commandLabel.Text = submittedText;

                // Now send the command to the computer and record the response.
                String response = GameManager.Instance.PlayerComputer.SubmitCommand(_currentInput);
                RichTextLabel responseLabel = _outputPool.GetAvailableObject();
                responseLabel.Text = response;

                // Move the input label to the bottom.
                _outputContainer.RemoveChild(_inputNode);
                _outputContainer.AddChild(_inputNode);

                // Reset the label.
                _currentInput = String.Empty;
                _inputNode.Text = String.Format(INPUT_FORMAT, _pwdText, PWD_SYMBOL, _currentInput);
                _inputNode.GrabFocus();
                _inputNode.CaretColumn = _inputNode.Text.Length;
            }
        }
    }
}
