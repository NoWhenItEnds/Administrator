using System;
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
        private const String INPUT_FORMAT = "{0} > {1}";


        /// <inheritdoc/>
        public override void _Ready()
        {
            // Check our exports.
            ArgumentNullException.ThrowIfNull(_outputContainer);
            ArgumentNullException.ThrowIfNull(_inputNode);

            _inputNode.TextChanged += OnInputChanged;
            _inputNode.TextSubmitted += OnInputSubmitted;

            _outputPool = new ObjectPool<RichTextLabel>(_outputContainer, _outputLabelPrefab, _outputPoolSize);
            _inputNode.Text = String.Format(INPUT_FORMAT, _pwdText, String.Empty);
        }


        private void OnInputChanged(String newText)
        {
            String[] input = newText.Split(" > ");

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
                _inputNode.Text = String.Format(INPUT_FORMAT, _pwdText, _currentInput);
                _inputNode.CaretColumn = _inputNode.Text.Length;
            }
        }


        private void OnInputSubmitted(String submittedText)
        {
            RichTextLabel newLabel = _outputPool.GetAvailableObject();
            newLabel.Text = _currentInput;

            // Move the input label to the bottom.
            _outputContainer.RemoveChild(_inputNode);
            _outputContainer.AddChild(_inputNode);

            // Reset the label.
            _currentInput = String.Empty;
            _inputNode.Text = String.Format(INPUT_FORMAT, _pwdText, _currentInput);
            _inputNode.GrabFocus();
            _inputNode.CaretColumn = _inputNode.Text.Length;
        }
    }
}
