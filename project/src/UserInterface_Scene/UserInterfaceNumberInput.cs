using System;
using Godot;

namespace WorldConquest.UserInterface_Scene;

public partial class UserInterfaceNumberInput : Control
{
	private Label _headingLabel;
	private SpinBox _integerInputSpinBox;
	private Button _confirmButton;
	private Button _cancelButton;

	[Signal]
	public delegate void SpinBoxInputConfirmedEventHandler(int numInput);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_headingLabel = this.GetNode<Label>("InputVBox/PromptText");
		_integerInputSpinBox = this.GetNode<SpinBox>("InputVBox/InputSpinBox");
		_confirmButton = this.GetNode<Button>("InputVBox/ButtonsHBox/ConfirmButton");
		_cancelButton = this.GetNode<Button>("InputVBox/ButtonsHBox/CancelButton");
		this.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Show the number input menu and prompt the player to input a value to carry out a certain action.
	/// </summary>
	/// <param name="message"></param> A message to be shown in the menu.
	/// <param name="minimum"></param> Minimum allowed value
	/// <param name="maximum"></param> Maximum allowed value
	public void ShowNumberInput(string message, int minimum, int maximum)
	{
		/*
		 * ToDo:
		 * 1. Set this.Visible to true.
		 * 2. Set spinbox Min Value to minimum
		 * 3. Set spinbox Max Value to maximum
		 * 4. Set heading label text to match the message.
		 */
		this.Visible = true;
		this._integerInputSpinBox.MinValue = minimum;
		this._integerInputSpinBox.MaxValue = maximum;
		this._headingLabel.Text = message;
	}

	/// <summary>
	/// Called when the "confirm" button is clicked. Sends a signal to BoardRoot which handles the event appropriately.
	/// </summary>
	private void _on_Confirm_Button_Pressed()
	{
		/*
		 * ToDo:
		 * 1: Emit the signal 'SpinBoxInputConfirmed'.
		 * 2: Set this.Visible to false.
		 */
		EmitSignal(SignalName.SpinBoxInputConfirmed, _integerInputSpinBox.Value);
		this.Visible = false;
	}

	/// <summary>
	/// Hide the menu.
	/// </summary>
	private void _on_Cancel_Button_Pressed()
	{
		/*
		 * ToDo:
		 * Set this.Visible to false.
		 */
		this.Visible = false;
	}
}
