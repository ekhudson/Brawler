using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrawlerPlayerComponent : MonoBehaviour
{
	private int mPlayerID = -1; //should get set from 0 - 4. If this is ever -1 it means this is not a valid player
	private int mAssociatedGamepad = -1; //again, if this is ever -1 it means no gamepad is assigned;
	private Color mPlayerColor = Color.white;
	private bool mIsActivePlayer = true;

	//TODO: Wrap this in a PlayerAttributes class
	#region PlayerAttributes
	public float PlayerStrength = 100f;
	public float AttackTime = 0.4f;
	public float MinimumTimeBetweenAttacks = 0.5f;
	public float MoveSpeed = 1.0f;
	public float ClimbSpeed = 1.0f;
	public float JumpForce = 2.0f;
	public float MinimumJumpTime = 0.5f;
	public AnimationCurve JumpCurve = new AnimationCurve();
	public float AirControl = 0.9f;
	public float ConstantFriction =  0.9f;
	public TriggerVolume PunchBox;
	public Transform HitParticle;
	#endregion

	#region Sprites
	public Sprite DefaultSprite;
	public Sprite JumpSprite;
	public Sprite AttackSprite;
	public Sprite JumpAttackSprite;
	#endregion

	protected Vector3 mTarget = Vector3.zero;
	protected CharacterEntity mController;
	
	private Collider mClimbingVolume;  
	
	private Vector3 mInitialRotation;
	private SpriteRenderer mSpriteRenderer;
	private float mLastAttackEndTime;
	
	public enum PlayerStates
	{
		IDLE,
		MOVING,
		JUMPING,
		FALLING,
		LANDING,
		FROZEN,
		ATTACKING_GROUND,
		ATTACKING_AIR,
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
		mSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
			
		case PlayerStates.ATTACKING_GROUND:

			if (mTimeInState > AttackTime)
			{
				SetState(PlayerStates.IDLE);
				mLastAttackEndTime = Time.realtimeSinceStartup;
			}
			
			break;
		
		case PlayerStates.ATTACKING_AIR:
			
			if (mTimeInState > AttackTime)
			{
				SetState(PlayerStates.FALLING);
				mLastAttackEndTime = Time.realtimeSinceStartup;
			}
			
			break;
			
		case PlayerStates.FROZEN:
			
			break;
		}
		
		
		Vector3 norm = mTarget.normalized;
		mController.Move( ((new Vector3(norm.x, 0, norm.z) * (MoveSpeed)) + new Vector3(0, mTarget.y, 0)) * Time.deltaTime);
		mTarget = Vector3.zero; 
		
		if (ConstantFriction > 0)
		{
			Vector3 currentVelocity = mController.BaseRigidbody.velocity;
			mController.BaseRigidbody.velocity = new Vector3(currentVelocity.x * ConstantFriction, currentVelocity.y, currentVelocity.z * ConstantFriction);
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
			mSpriteRenderer.sprite = DefaultSprite;
			
			break;
			
		case PlayerStates.MOVING:
			
			rigidbody.useGravity = true;
			mSpriteRenderer.sprite = DefaultSprite;
			
			break;
			
		case PlayerStates.JUMPING:
			
			if (mPlayerState == PlayerStates.FALLING)
			{
				return;
			}
			
			mController.SetGrounded(false);
			mSpriteRenderer.sprite = JumpSprite;
			
			break;
			
		case PlayerStates.FALLING:
			
			rigidbody.useGravity = true;
			mSpriteRenderer.sprite = JumpSprite;
			
			break;
			
		case PlayerStates.LANDING:
			
			break;

		case PlayerStates.ATTACKING_GROUND:

			mSpriteRenderer.sprite = AttackSprite;
			
			break;

		case PlayerStates.ATTACKING_AIR:
			
			mSpriteRenderer.sprite = JumpAttackSprite;
			
			break;

		case PlayerStates.FROZEN:
			
			break;
		}
		
		mTimeInState = 0.0f;
		
		mPlayerState = state;
	}
	
	public void InputHandler(object sender, UserInputKeyEvent evt)
	{
		if (!gameObject.activeSelf)
		{
			return;
		}

		if (evt.KeyBind != BrawlerUserInput.Instance.MoveCharacter)
		{
			Debug.Log (string.Format("player {2} getting {0} for player {1}", evt.KeyBind.BindingName, evt.PlayerIndexInt.ToString(), mPlayerID.ToString()));
		}
		
		if(evt.PlayerIndexInt != mPlayerID - 1 && evt.PlayerIndexInt != -1)
		{
			if (evt.KeyBind != BrawlerUserInput.Instance.MoveCharacter)
			{
				Debug.Log ("Ignoring");
			}

			return;
		}

		if (GetState == PlayerStates.IDLE || GetState == PlayerStates.MOVING || 
		    mPlayerState == PlayerStates.FALLING || mPlayerState == PlayerStates.JUMPING ||
		    mPlayerState == PlayerStates.ATTACKING_AIR)
		{
			
			if(evt.KeyBind == BrawlerUserInput.Instance.MoveLeft && (evt.Type == UserInputKeyEvent.TYPE.KEYDOWN || evt.Type == UserInputKeyEvent.TYPE.KEYHELD))
			{
				mTarget += -Camera.main.transform.right;

				if (mSpriteRenderer.transform.rotation.eulerAngles.y == 0)
				{
					mSpriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
				}

			}
			
			if(evt.KeyBind == BrawlerUserInput.Instance.MoveRight && (evt.Type == UserInputKeyEvent.TYPE.KEYDOWN || evt.Type == UserInputKeyEvent.TYPE.KEYHELD))
			{
				mTarget += Camera.main.transform.right;

				if (mSpriteRenderer.transform.rotation.eulerAngles.y != 0)
				{
					mSpriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
				}
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

					if (mPlayerState == PlayerStates.IDLE || mPlayerState == PlayerStates.MOVING)
					{
						SetState(PlayerStates.JUMPING);
					}
					
					break;
					
				case UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_DOWN: 
					
					SetState(PlayerStates.JUMPING);
					
					break;
					
				case UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_UP:
					
					SetState(PlayerStates.FALLING);
					
					break;
					
				case UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_HELD:

					if (mPlayerState == PlayerStates.IDLE || mPlayerState == PlayerStates.MOVING)
					{
						SetState(PlayerStates.JUMPING);
					}
					
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

			if (evt.KeyBind == BrawlerUserInput.Instance.Attack && (evt.Type == UserInputKeyEvent.TYPE.KEYDOWN || evt.Type == UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_DOWN))
			{
				if ( (Time.realtimeSinceStartup - mLastAttackEndTime) < MinimumTimeBetweenAttacks )
				{
					return;
				}

				if (mPlayerState == PlayerStates.IDLE || mPlayerState == PlayerStates.MOVING)
				{
					SetState(PlayerStates.ATTACKING_GROUND);
				}
				else if (mPlayerState == PlayerStates.JUMPING || mPlayerState == PlayerStates.FALLING)
				{
					SetState(PlayerStates.ATTACKING_AIR);
				}
				else if (mPlayerState == PlayerStates.ATTACKING_AIR || mPlayerState == PlayerStates.ATTACKING_GROUND)
				{
					return;
				}

				foreach(Collider obj in PunchBox.ObjectList)
				{
					if (obj.gameObject.GetInstanceID() == gameObject.GetInstanceID())
					{
						continue;
					}

					if (obj.GetComponent<Rigidbody>() != null)
					{
						obj.GetComponent<Rigidbody>().AddForceAtPosition(mSpriteRenderer.transform.right * PlayerStrength, obj.transform.position);

						Transform go = (Transform)Instantiate(HitParticle, obj.transform.position + new Vector3(0f,0f,-2f), Quaternion.identity);

						ParticleSystem hitParticle = go.GetComponent<ParticleSystem>();
						
						if (hitParticle != null)
						{
							hitParticle.startColor = PlayerColor;
							Destroy (go.gameObject, hitParticle.duration);
						}

						go.transform.rotation = mSpriteRenderer.transform.rotation;

					}
				}
			}
			
			if (evt.KeyBind == BrawlerUserInput.Instance.MoveCharacter)
			{
				mTarget.x += (evt.JoystickInfo.AmountX);

				if (evt.JoystickInfo.AmountX < 0 && mSpriteRenderer.transform.rotation.eulerAngles.y == 0)
				{
					mSpriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
				}
				else if (evt.JoystickInfo.AmountX > 0 && mSpriteRenderer.transform.rotation.eulerAngles.y != 0)
				{
					mSpriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
				}

				if (evt.JoystickInfo.AmountY > 0)
				{
					switch(mPlayerState)
					{
					case PlayerStates.IDLE:
						
						SetState(PlayerStates.JUMPING);
						
						break;
						
					case PlayerStates.MOVING:
						
						SetState(PlayerStates.JUMPING);
						
						break;
						
					case PlayerStates.JUMPING:
						
						break;
						
					case PlayerStates.FALLING:						

						
						break;
						
					case PlayerStates.LANDING:
						
						break;
						
					case PlayerStates.FROZEN:
						
						break;
					}
				}
				else if (evt.JoystickInfo.AmountY <= 0 && mPlayerState == PlayerStates.JUMPING)
				{
					SetState(PlayerStates.FALLING);
				}



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

		SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();

		if (sprite != null)
		{ 
			GetComponentInChildren<SpriteRenderer>().color = mPlayerColor;
		}
	}
	
}


