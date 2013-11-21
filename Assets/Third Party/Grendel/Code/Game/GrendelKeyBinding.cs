using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using XInputDotNetPure;

[System.Serializable]
public class GrendelKeyBinding
{
    public string BindingName = "New Binding";
    public KeyCode Key = KeyCode.A;
    public KeyCode AltKey = KeyCode.B;
    public bool Enabled = true;
    public MouseButtons MouseButton = MouseButtons.None;
    public MouseButtons AltMouseButton = MouseButtons.None;
	public List<GrendelKeyBinding> Conflicts = new List<GrendelKeyBinding>(); //TODO: Figure out the most efficient way to update keybind conflicts

	[System.Serializable]
	public class GamePadButtonBinding
	{
		[HideInInspector]public string Name = string.Empty;
		public bool BindToThisButton = false;
		private ButtonState mButton;

		public GamePadButtonBinding(ButtonState button)
		{
			mButton = button;
		}

		public ButtonState Button
		{
			get
			{
				return mButton;
			}
		}
	}

	public GamePadButtonBinding A = new GamePadButtonBinding(Buttons.A);
	public GamePadButtonBinding B = new GamePadButtonBinding(Buttons.B);
	public GamePadButtonBinding X = new GamePadButtonBinding(Buttons.X);
	public GamePadButtonBinding Y = new GamePadButtonBinding(Buttons.Y);
	public GamePadButtonBinding Up = new GamePadButtonBinding(DPad.Up);
	public GamePadButtonBinding Down = new GamePadButtonBinding(DPad.Down);
	public GamePadButtonBinding Left = new GamePadButtonBinding(DPad.Left);
	public GamePadButtonBinding Right = new GamePadButtonBinding(DPad.Right);
	public GamePadButtonBinding LeftShoulder = new GamePadButtonBinding(Buttons.LeftShoulder);
	public GamePadButtonBinding RightShoulder = new GamePadButtonBinding(Buttons.RightShoulder);
	public GamePadButtonBinding LeftStick = new GamePadButtonBinding(Buttons.LeftStick);
	public GamePadButtonBinding RightStick = new GamePadButtonBinding(Buttons.RightStick);
	public GamePadButtonBinding Back = new GamePadButtonBinding(Buttons.Back);
	public GamePadButtonBinding Start = new GamePadButtonBinding(Buttons.Start);

    private bool mIsDown = false;

    public GrendelKeyBinding(string bindingName, KeyCode key, KeyCode altKey, MouseButtons mouseButton, MouseButtons altMouseButton)
    {
        BindingName = bindingName;
        Key = key;
        AltKey = altKey;
        MouseButton = mouseButton;
        AltMouseButton = altMouseButton;
    }

    public GrendelKeyBinding(string bindingName, KeyCode key, KeyCode altKey)
    {
        BindingName = bindingName;
        Key = key;
        AltKey = altKey;
        MouseButton = MouseButtons.None;
        AltMouseButton = MouseButtons.None;
    }

    public bool IsDown
    {
        get
        {
            return mIsDown;
        }
        set
        {
            mIsDown = value;
        }
    }

    public enum MouseButtons
    {
        None = 0,
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
    }

	public static GamePadButtons Buttons
	{
		get
		{
			return new GamePadButtons();
		}
	}

	public static GamePadDPad DPad
	{
		get
		{
			return new GamePadDPad();
		}
	}
}
