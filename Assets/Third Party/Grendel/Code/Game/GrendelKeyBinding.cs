using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GrendelKeyBinding
{
    public string BindingName = "New Binding";
    public KeyCode Key = KeyCode.A;
    public KeyCode AltKey = KeyCode.B;
    public bool Enabled = true;
    public MouseButtons MouseButton = MouseButtons.None;
    public MouseButtons AltMouseButton = MouseButtons.None;
	public Joysticks Joystick = Joysticks.None;
	public List<GrendelKeyBinding> Conflicts = new List<GrendelKeyBinding>(); //TODO: Figure out the most efficient way to update keybind conflicts

    private bool mIsDown = false;

	public GrendelKeyBinding(string bindingName, KeyCode key, KeyCode altKey, MouseButtons mouseButton, MouseButtons altMouseButton, GrendelKeyBinding.Joysticks joystick)
	{
		BindingName = bindingName;
		Key = key;
		AltKey = altKey;
		MouseButton = mouseButton;
		AltMouseButton = altMouseButton;
		Joystick = joystick;
	}

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

	public enum Joysticks
	{
		None = 0,
		Joystick1 = 1,
		Joystick2 = 2,
		Joystick3 = 3,
		Joystick4 = 4,
		Joystick5 = 5,
		Joystick6 = 6,
		Joystick7 = 7,
		Joystick8 = 8,
		Joystick9 = 9,
		Joystick10 = 10,
		Joystick11 = 11,
	}
}
