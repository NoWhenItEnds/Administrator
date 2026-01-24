using System;
using System.Collections.Generic;
using System.Linq;
using Administrator.Managers;
using Administrator.Subspace;
using Administrator.Utilities;
using Godot;

namespace Administrator.UI
{
    /// <summary> The terminal UI the user uses to interact with the game world. </summary>
    public partial class Terminal : TilingWindow
    {
        /// <summary> The container that holds both the results of the previous user input and the input node. </summary>
        [ExportGroup("Nodes")]
        [Export] private VBoxContainer _outputContainer;

        /// <summary> The scrollable container that holds the terminal controls. </summary>
        [Export] private ScrollContainer _scrollContainer;

        /// <summary> The input field the user uses to type commands to the terminal. </summary>
        [Export] private LineEdit _inputNode;


        /// <summary> How many labels our object pool can contain. </summary>
        [ExportGroup("Settings")]
        [Export] private Int32 _outputPoolSize = 100;


        /// <summary> The prefab used to spawn additional output labels. </summary>
        [ExportGroup("Resources")]
        [Export] private PackedScene _outputLabelPrefab;


        /// <summary> The current user the player is using on the terminal. </summary>
        private User _currentUser = GameManager.Instance.PlayerComputer.Files.GetUsers().FirstOrDefault(x => x.Username == "admin") ?? throw new ArgumentNullException("TODO - Not this way, fix it."); // TODO - Fix it.

        /// <summary> Get the terminal user's current working directory. </summary>
        private String _workingDirectory => GameManager.Instance.PlayerComputer.Files.GetWorkingDirectory(_currentUser); // TODO - Not sure I need this.


        /// <summary> The user's current input. </summary>
        private String _currentInput = String.Empty;

        /// <summary> An object pool holding the terminal's output labels. </summary>
        private ObjectPool<RichTextLabel> _outputPool;

        /// <summary> The index of the currently selected historical input. -1 means that there isn't one. </summary>
        private Int32 _historyIndex = -1;

        /// <summary> An ordered list of the previous command typed. </summary>
        private readonly List<String> HISTORY = new List<String>();

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
            _inputNode.Text = String.Format(INPUT_FORMAT, _workingDirectory, PWD_SYMBOL, String.Empty);
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
            if (input[0] != _workingDirectory)
            {
                _inputNode.Text = String.Format(INPUT_FORMAT, _workingDirectory, PWD_SYMBOL, _currentInput);
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
                if (HISTORY.Count == 0 || HISTORY.Last() != _currentInput)    // Only add a history if we're not repeating ourselves.
                {
                    HISTORY.Add(_currentInput);
                }

                String response = GameManager.Instance.PlayerComputer.SubmitCommand(_currentUser, _currentInput);
                if (!String.IsNullOrWhiteSpace(response))   // Don't print a empty response.
                {
                    RichTextLabel responseLabel = _outputPool.GetAvailableObject();
                    responseLabel.Text = response;
                }

                // Move the input label to the bottom.
                _outputContainer.RemoveChild(_inputNode);
                _outputContainer.AddChild(_inputNode);

                // Reset the label.
                _historyIndex = -1;
                _currentInput = String.Empty;
                _inputNode.Text = String.Format(INPUT_FORMAT, _workingDirectory, PWD_SYMBOL, _currentInput);
                _inputNode.GrabFocus();
                _inputNode.CaretColumn = _inputNode.Text.Length;
                CallDeferred("ScrollToBottom"); // TODO - Not entirely working.
            }
        }


        /// <summary> Scroll the terminal to the bottom to follow the input. </summary>
        private void ScrollToBottom()
        {
            VScrollBar? scrollBar = _scrollContainer.GetVScrollBar();
            if (scrollBar != null)
            {
                scrollBar.Value = scrollBar.MaxValue;
            }
        }


        /// <inheritdoc/>
        public override void SetActive()
        {
            _inputNode.GrabFocus();
            _inputNode.CaretColumn = _inputNode.Text.Length;
        }


        /// <inheritdoc/>
        public override void _Input(InputEvent @event)
        {
            if (_inputNode.HasFocus())  // Only allow input if the text input is selected.
            {
                if (Input.IsPhysicalKeyPressed(Key.Up) || Input.IsPhysicalKeyPressed(Key.Down))
                {
                    GetViewport().SetInputAsHandled();  // Prevent the event from moving the caret.
                    Int32 direction = (Input.IsPhysicalKeyPressed(Key.Up) ? 1 : 0) - (Input.IsPhysicalKeyPressed(Key.Down) ? 1 : 0);
                    _historyIndex = Math.Clamp(_historyIndex + direction, -1, HISTORY.Count - 1);

                    // Update the text to the selected history. We count in reverse, taking the length minus the index.
                    _currentInput = _historyIndex >= 0 ? HISTORY[HISTORY.Count - _historyIndex - 1] : String.Empty;
                    _inputNode.Text = String.Format(INPUT_FORMAT, _workingDirectory, PWD_SYMBOL, _currentInput);
                    _inputNode.GrabFocus();
                    _inputNode.CaretColumn = _inputNode.Text.Length;
                }
            }
        }
    }
}
