using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure; // Required in C#

public class UserInput<T> : Singleton<T> where T  : MonoBehaviour
{
    public float MouseSensitivityVertical = 1f;
    public float MouseSensitivityHorizontal = 1f;

	[HideInInspector]public GrendelKeyBinding MoveUp = new GrendelKeyBinding("Move Up", KeyCode.W, KeyCode.UpArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.Joysticks.None);
	[HideInInspector]public GrendelKeyBinding MoveDown = new GrendelKeyBinding("Move Down", KeyCode.S, KeyCode.DownArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.Joysticks.None);
    [HideInInspector]public GrendelKeyBinding MoveLeft = new GrendelKeyBinding("Move Left", KeyCode.A, KeyCode.LeftArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.Joysticks.None);
	[HideInInspector]public GrendelKeyBinding MoveRight = new GrendelKeyBinding("Move Right", KeyCode.D, KeyCode.RightArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.Joysticks.None);
    [HideInInspector]public GrendelKeyBinding Jump = new GrendelKeyBinding("Jump", KeyCode.Space, KeyCode.Return, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.Joysticks.None);
	[HideInInspector]public GrendelKeyBinding Run = new GrendelKeyBinding("Run", KeyCode.LeftShift, KeyCode.RightShift, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.Joysticks.None);
	[HideInInspector]public GrendelKeyBinding PrimaryFire = new GrendelKeyBinding("Primary Fire", KeyCode.None, KeyCode.None, GrendelKeyBinding.MouseButtons.One, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.Joysticks.None);
	[HideInInspector]public GrendelKeyBinding SecondaryFire = new GrendelKeyBinding("Secondary Fire", KeyCode.None, KeyCode.None, GrendelKeyBinding.MouseButtons.Two, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.Joysticks.None);

    [HideInInspector]public List<GrendelKeyBinding> KeyBindings = new List<GrendelKeyBinding>();

    private Dictionary<KeyCode, List<GrendelKeyBinding>> mGrendelKeyBindingsDictionary = new Dictionary<KeyCode, List<GrendelKeyBinding>>();
    private Dictionary<GrendelKeyBinding.MouseButtons, List<GrendelKeyBinding>> mMouseBindingsDictionary = new Dictionary<GrendelKeyBinding.MouseButtons, List<GrendelKeyBinding>>();
	private Dictionary<GrendelKeyBinding.Joysticks, List<GrendelKeyBinding>> mJoystickBindingsDictionary = new Dictionary<GrendelKeyBinding.Joysticks, List<GrendelKeyBinding>>();

    private List<GrendelKeyBinding> mKeysDown = new List<GrendelKeyBinding>();

	private List<int> mConnectControllerIndexes = new List<int>();
    
    // Use this for initialization
    private void Start ()
    {
        GatherKeyBindings(this.GetType());
        StoreGrendelKeyBindings();
        mKeysDown.Clear();
		GetConnectedControllers();
    }

	private void GetConnectedControllers()
	{
		for (int i = 0; i < 4; ++i)
		{
			PlayerIndex testPlayerIndex = (PlayerIndex)i;
			GamePadState testState = GamePad.GetState(testPlayerIndex);
			if (testState.IsConnected)
			{
				Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
				mConnectControllerIndexes.Add(i);
			}
		}
	}

    //Find all the GrendelKeyBindings on UserInput
    public void GatherKeyBindings(System.Type t)
    {
        KeyBindings.Clear();

        System.Type myType = t;

        System.Reflection.FieldInfo[] myField = myType.GetFields();

        for(int i = 0; i < myField.Length; i++)
        {
            if(myField[i].FieldType == typeof(GrendelKeyBinding))
            {
                GrendelKeyBinding binding = (GrendelKeyBinding)myField[i].GetValue(this);
                if (!KeyBindings.Contains(binding))
                {
                    KeyBindings.Add(binding);
                }
            }
        }
    }

    //Store all the KeyBindings for easy referencing
    private void StoreGrendelKeyBindings()
    {
        foreach(GrendelKeyBinding binding in KeyBindings)
        {
            if (binding.Key != KeyCode.None)
            {
                if (!mGrendelKeyBindingsDictionary.ContainsKey(binding.Key))
                {
                    mGrendelKeyBindingsDictionary.Add(binding.Key, new List<GrendelKeyBinding>(){ binding } );
                }
                else
                {
                    mGrendelKeyBindingsDictionary[binding.Key].Add(binding);
                }
            }

            if (binding.AltKey != KeyCode.None)
            {
                if (!mGrendelKeyBindingsDictionary.ContainsKey(binding.AltKey))
                {
                    mGrendelKeyBindingsDictionary.Add(binding.AltKey, new List<GrendelKeyBinding>(){ binding });
                }
                else
                {
                    mGrendelKeyBindingsDictionary[binding.AltKey].Add(binding);
                }
            }

            if (binding.MouseButton != GrendelKeyBinding.MouseButtons.None)
            {
                if (!mMouseBindingsDictionary.ContainsKey(binding.MouseButton))
                {
                    mMouseBindingsDictionary.Add(binding.MouseButton, new List<GrendelKeyBinding>(){ binding });
                }
                else
                {
                    mMouseBindingsDictionary[binding.MouseButton].Add(binding);
                }
            }

            if (binding.AltMouseButton != GrendelKeyBinding.MouseButtons.None)
            {
                if (!mMouseBindingsDictionary.ContainsKey(binding.AltMouseButton))
                {
                    mMouseBindingsDictionary.Add(binding.AltMouseButton, new List<GrendelKeyBinding>(){ binding });
                }
                else
                {
                    mMouseBindingsDictionary[binding.AltMouseButton].Add(binding);
                }
            }

			if (binding.Joystick != GrendelKeyBinding.Joysticks.None)
			{
				if (!mJoystickBindingsDictionary.ContainsKey(binding.Joystick))
				{
					mJoystickBindingsDictionary.Add(binding.Joystick, new List<GrendelKeyBinding>(){ binding });
				}
				else
				{
					mJoystickBindingsDictionary[binding.Joystick].Add(binding);
				}
			}
        }
    }
     
    // Update is called once per frame
    private void Update ()
    { 
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            if(GameOptions.Instance.DebugMode){ Console.Instance.ToggleConsole(); }
        }

        foreach(GrendelKeyBinding binding in mKeysDown)
        {
            EventManager.Instance.Post(new UserInputKeyEvent(UserInputKeyEvent.TYPE.KEYHELD, binding, Vector3.zero, this));
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey && e.keyCode != KeyCode.None)
        {
            if(e.type == EventType.KeyDown || e.button != null)
            {
                ProcessKeycode(e.keyCode, UserInputKeyEvent.TYPE.KEYDOWN);
            }

            if(e.type == EventType.KeyUp)
            {
                ProcessKeycode(e.keyCode, UserInputKeyEvent.TYPE.KEYUP);
            }
        }
        else if (e.isMouse && e.type == EventType.mouseDown || e.type == EventType.mouseUp)
        {
            ProcessMouseInput(e.button, e.type);
        }

		GatherJoystickInput();
    }

    private void ProcessKeycode(KeyCode code, UserInputKeyEvent.TYPE inputType)
    {
        if (!mGrendelKeyBindingsDictionary.ContainsKey(code))
        {
            return;
        }

        foreach(GrendelKeyBinding binding in mGrendelKeyBindingsDictionary[code])
        {
            if (binding.Enabled)
            {
                EventManager.Instance.Post(new UserInputKeyEvent(inputType, binding, Vector3.zero, this));

                if (inputType == UserInputKeyEvent.TYPE.KEYDOWN)
                {
                    binding.IsDown = true;

                    if (!mKeysDown.Contains(binding))
                    {
                        mKeysDown.Add(binding);
                    }
                }
                else if (inputType == UserInputKeyEvent.TYPE.KEYUP)
                {
                    binding.IsDown = false;

                    if (mKeysDown.Contains(binding))
                    {
                        mKeysDown.Remove(binding);
                    }
                }
            }
        }
    }

    private void ProcessMouseInput(int button, EventType evtType)
    {
        GrendelKeyBinding.MouseButtons mouseButton = (GrendelKeyBinding.MouseButtons)(button + 1);
        UserInputKeyEvent.TYPE inputType = evtType == EventType.MouseDown ? UserInputKeyEvent.TYPE.KEYDOWN : UserInputKeyEvent.TYPE.KEYUP;

        if (!mMouseBindingsDictionary.ContainsKey(mouseButton))
        {
            return;
        }

        foreach(GrendelKeyBinding binding in mMouseBindingsDictionary[mouseButton])
        {
            if (binding.Enabled)
            {
                EventManager.Instance.Post(new UserInputKeyEvent(inputType, binding, Vector3.zero, this));

                if (inputType == UserInputKeyEvent.TYPE.KEYDOWN)
                {
                    binding.IsDown = true;

                    if (!mKeysDown.Contains(binding))
                    {
                        mKeysDown.Add(binding);
                    }
                }
                else if (inputType == UserInputKeyEvent.TYPE.KEYUP)
                {
                    binding.IsDown = false;

                    if (mKeysDown.Contains(binding))
                    {
                        mKeysDown.Remove(binding);
                    }
                }
            }
        }
    }

	private void GatherJoystickInput()
	{
		for(int i = 0; i < mConnectControllerIndexes.Count; i++)
		{
			int controllerIndex = mConnectControllerIndexes[i];		
			PlayerIndex playerIndex = (PlayerIndex)controllerIndex;
			GamePadState state = GamePad.GetState(playerIndex);

			if (!state.IsConnected)
			{
				Console.Instance.OutputToConsole(string.Format("Controller {0} has been disconnected!", controllerIndex.ToString()), Console.Instance.Style_Error);
				mConnectControllerIndexes.Remove(controllerIndex);
				continue;
			}

			ProcessJoystickInput(state, playerIndex);
		}
	}

	private void ProcessJoystickInput(GamePadState state, PlayerIndex playerIndex)
	{
		if (state.Buttons.A == ButtonState.Pressed)
		{

		}
	}

    /// <summary>
    /// Enables or disables a binding.
    /// </summary>
    /// <param name='binding'>
    /// Binding.
    /// </param>
    /// <param name='enable'>
    /// Enable (true) / Disable (false).
    /// </param>
    public void EnableBinding(GrendelKeyBinding binding, bool enable)
    {
        if(KeyBindings.Contains(binding))
        {
                binding.Enabled = enable;
        }
    }

    /// <summary>
    /// Enables or disables several bindings.
    /// </summary>
    /// <param name='bindings'>
    /// Array of bindings.
    /// </param>
    /// <param name='enable'>
    /// Enable (true) / Disable (false).
    /// </param>
    public void EnableBindings(GrendelKeyBinding[] bindings, bool enable)
    {
        foreach(GrendelKeyBinding binding in bindings)
        {
            EnableBinding(binding, enable);
        }
    }
}
