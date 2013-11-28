using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrawlerPlayerComponent : MonoBehaviour
{
	private int mPlayerID = -1; //should get set from 0 - 4. If this is ever -1 it means this is not a valid player
	private int mAssociatedGamepad = -1; //again, if this is ever -1 it means no gamepad is assigned;
	private Color mPlayerColor = Color.white;
	private bool mIsActivePlayer = false;

	//TODO: Wrap this in a PlayerAttributes class
	#region PlayerAttributes
	public float PlayerStrength = 10000f;
	public float MoveSpeed = 1.0f;
	public float ClimbSpeed = 1.0f;
	public float JumpForce = 2.0f;
	public float MinimumJumpTime = 0.5f;
	public AnimationCurve JumpCurve = new AnimationCurve();
	public float AirControl = 0.9f;
	public float ConstantFriction =  0.9f;
	#endregion

	protected Vector3 mTarget = Vector3.zero;
	protected CharacterEntity mController;
	
	private Collider mClimbingVolume;  
	
	private Vector3 mInitialRotation;
	
	public enum PlayerStates
	{
		IDLE,
		MOVING,
		JUMPING,
		FALLING,
		LANDING,
		FROZEN,
		USING,
	}
	
	protected PlayerStates mPlayerState = PlayerStates.IDLE;
	protected float mTimeInState = 0.0f;

	public int PlayerID
	{
		get
		{
			return mPlayerID;
		}
	}

	public int AssociatedGamepad
	{
		get
		{
			return mAssociatedGamepad;
		}
	}

	public Color PlayerColor
	{
		get
		{
			return mPlayerColor;
		}
	}

	public bool IsActivePlayer
	{
		get
		{
			return mIsActivePlayer;
		}
		set
		{
			mIsActivePlayer = value;
		}
	}

	public PlayerStates GetState
	{
		get
		{
			return mPlayerState;
		}
	}
	
	public CharacterEntity GetEntity
	{
		get
		{
			return mController;
		}
	}    
	
	protected void Start()
	{
		EventManager.Instance.AddHandler<UserInputKeyEvent>(InputHandler);
		mController = GetComponent<CharacterEntity>();
		rigidbody.WakeUp();        
	}  
	
	protected void Update()
	{
		if (mController == null || mPlayerState == PlayerStates.FROZEN)
		{
			return;
		}
		
		switch(GetState)
		{
		case PlayerStates.IDLE:
			
			break;
			
		case PlayerStates.MOVING:
			
			break;
			
		case PlayerStates.JUMPING:
			
			if (mTimeInState > JumpCurve.keys[JumpCurve.length - 1].time && mTimeInState > MinimumJumpTime)
			{
				SetState(PlayerStates.FALLING);
				return;
			}
			
			mController.SetGrounded(false);
			mController.Move( mController.BaseTransform.up * (JumpForce * JumpCurve.Evaluate(mTimeInState)));	
			
			mTarget *= AirControl;
			
			break;
			
		case PlayerStates.FALLING:
			
			if (mController.IsGrounded)
			{
				SetState(PlayerStates.IDLE);
			}
			
			mTarget *= AirControl;
			
			break;
			
		case PlayerStates.LANDING:
			
			break;
			
		case PlayerStates.USING:
			
			break;
			
		case PlayerStates.FROZEN:
			
			break;
		}
		
		
		Vector3 norm = mTarget.normalized;
		mController.Move( ((new Vector3(norm.x, 0, norm.z) * (MoveSpeed)) + new Vector3(0, mTarget.y, 0)) * Time.deltaTime);
		mTarget = Vector3.zero; 
		
		if (ConstantFriction > 0)
		{
			mController.BaseRigidbody.velocity *= ConstantFriction;
		}
		
		mTimeInState += Time.deltaTime;
	}	
	
	public void MoveEntity(Vector3 direction)
	{
		mTarget += direction;
	}      
	
	public void SetState(PlayerStates state)
	{
		if (state == mPlayerState)
		{
			return;
		}
		
		switch(state)
		{
		case PlayerStates.IDLE:
			
			rigidbody.useGravity = true;
			
			break;
			
		case PlayerStates.MOVING:
			
			rigidbody.useGravity = true;
			
			break;
			
		case PlayerStates.JUMPING:
			
			if (mPlayerState == PlayerStates.FALLING)
			{
				return;
			}
			
			mController.SetGrounded(false);
			
			break;
			
		case PlayerStates.FALLING:
			
			rigidbody.useGravity = true;
			
			break;
			
		case PlayerStates.LANDING:
			
			break;
			
		case PlayerStates.FROZEN:
			
			break;
		}
		
		mTimeInState = 0.0f;
		
		mPlayerState = state;
	}
	
	public void InputHandler(object sender, UserInputKeyEvent evt)
	{
		if(evt.PlayerIndexInt != mPlayerID - 1)
		{
			return;
		}

		if (GetState == PlayerStates.IDLE || GetState == PlayerStates.MOVING || mPlayerState == PlayerStates.FALLING || mPlayerState == PlayerStates.JUMPING)
		{
			
			if(evt.KeyBind == BrawlerUserInput.Instance.MoveLeft && (evt.Type == UserInputKeyEvent.TYPE.KEYDOWN || evt.Type == UserInputKeyEvent.TYPE.KEYHELD))
			{
				mTarget += -Camera.main.transform.right;
			}
			
			if(evt.KeyBind == BrawlerUserInput.Instance.MoveRight && (evt.Type == UserInputKeyEvent.TYPE.KEYDOWN || evt.Type == UserInputKeyEvent.TYPE.KEYHELD))
			{
				mTarget += Camera.main.transform.right;
			}
			
			if(evt.KeyBind == BrawlerUserInput.Instance.MoveUp && (evt.Type == UserInputKeyEvent.TYPE.KEYDOWN || evt.Type == UserInputKeyEvent.TYPE.KEYHELD))
			{
				
			}
			
			if(evt.KeyBind == BrawlerUserInput.Instance.Jump)
			{
				switch(evt.Type)
				{
				case UserInputKeyEvent.TYPE.KEYDOWN: 
					
					SetState(PlayerStates.JUMPING);
					
					break;
					
				case UserInputKeyEvent.TYPE.KEYUP:
					
					SetState(PlayerStates.FALLING);
					
					break;
					
				case UserInputKeyEvent.TYPE.KEYHELD:
					
					break;
					
				case UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_DOWN: 
					
					SetState(PlayerStates.JUMPING);
					
					break;
					
				case UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_UP:
					
					SetState(PlayerStates.FALLING);
					
					break;
					
				case UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_HELD:
					
					break;
				}
			}
			
			if(evt.KeyBind == BrawlerUserInput.Instance.MoveDown && (evt.Type == UserInputKeyEvent.TYPE.KEYDOWN || evt.Type == UserInputKeyEvent.TYPE.KEYHELD))
			{
				
			}
			
			if(evt.KeyBind == BrawlerUserInput.Instance.PrimaryFire && (evt.Type == UserInputKeyEvent.TYPE.KEYDOWN))
			{
				
			}
			
			if(evt.KeyBind == BrawlerUserInput.Instance.UseKey01 && evt.Type == UserInputKeyEvent.TYPE.KEYDOWN)
			{
				
			}
			
			if(evt.KeyBind == BrawlerUserInput.Instance.UseKey02 && evt.Type == UserInputKeyEvent.TYPE.KEYDOWN)
			{
				
			}
			
			if (evt.KeyBind == BrawlerUserInput.Instance.MoveCharacter)
			{
				mTarget.x += (evt.JoystickInfo.AmountX);
			}
		}       
	}

	public void SetID(int id)
	{
		mPlayerID = id;
	}

	public void SetGamepadID(int gamepadID)
	{
		mAssociatedGamepad = gamepadID;
	}

	public void SetPlayerColor(Color color)
	{
		mPlayerColor = color;
		GetComponentInChildren<SpriteRenderer>().color = mPlayerColor;
	}
	
}

