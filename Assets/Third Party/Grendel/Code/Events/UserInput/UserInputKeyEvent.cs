using UnityEngine;
using System.Collections;

using XInputDotNetPure;

public class UserInputKeyEvent : EventBase
{
    public enum TYPE
    {
        KEYDOWN,
        KEYHELD,
        KEYUP,
		GAMEPAD_BUTTON,
		GAMEPAD_JOYSTICK,
    }
    
    public readonly TYPE Type;
    public readonly GrendelKeyBinding KeyBind;
	public readonly JoystickInfoClass JoystickInfo;
	public readonly GamePadInfoClass GamePadInfo;
    
	public class JoystickInfoClass
	{
		public readonly PlayerIndex PlayerID;
		public readonly float AmountX = 0.0f;
		public readonly float AmountY = 0.0f;

		public JoystickInfoClass(PlayerIndex playerID, float amountX, float amountY)
		{
			PlayerID = playerID;
			AmountX = amountX;
			AmountY = amountY;
		}

		public JoystickInfoClass()
		{

		}
	}

	public class GamePadInfoClass
	{
		public readonly GamePadState PadState;

		public GamePadInfoClass(GamePadState gamePadState)
        {
			PadState = gamePadState;
		}
	}

	public UserInputKeyEvent(UserInputKeyEvent.TYPE inputType, GrendelKeyBinding bind, Vector3 location, object sender) : base(location, sender)
	{
		Type = inputType;
		KeyBind = bind;
	}

    public UserInputKeyEvent(UserInputKeyEvent.TYPE inputType, GrendelKeyBinding bind, JoystickInfoClass joystickInfo, Vector3 location, object sender) : base(location, sender)
    {
        Type = inputType;
        KeyBind = bind;
		JoystickInfo = joystickInfo;
    }
    
    public UserInputKeyEvent() : base (Vector3.zero, null)
    {        
        
    }
}