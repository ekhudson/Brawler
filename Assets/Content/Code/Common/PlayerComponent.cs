using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerComponent : Singleton<PlayerComponent>
{
    public float PlayerStrength = 10000f;
    public float MoveSpeed = 1.0f;
    public float ClimbSpeed = 1.0f;
	public float JumpForce = 2.0f;
	public AnimationCurve JumpCurve = new AnimationCurve();

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

        Vector3 norm = mTarget.normalized;
        mController.Move( ((new Vector3(norm.x, 0, norm.z) * (MoveSpeed)) + new Vector3(0, mTarget.y, 0)) * Time.deltaTime);
        mTarget = Vector3.zero;

        switch(GetState)
        {
            case PlayerComponent.PlayerStates.IDLE:

            break;
    
            case PlayerComponent.PlayerStates.MOVING:
    
            break;
    
            case PlayerComponent.PlayerStates.JUMPING:

				if (mTimeInState > JumpCurve.keys[JumpCurve.length - 1].time)
				{
					SetState(PlayerStates.FALLING);
					return;
				}

				mController.Move( mController.BaseTransform.up * (JumpForce * JumpCurve.Evaluate(mTimeInState)));				
    
            break;

			case PlayerComponent.PlayerStates.FALLING:

				if (mController.IsGrounded)
				{
					SetState(PlayerStates.IDLE);
				}
			
			break;

			case PlayerComponent.PlayerStates.LANDING:
			
			break;

            case PlayerComponent.PlayerStates.USING:

            break;

            case PlayerComponent.PlayerStates.FROZEN:

            break;
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
		if (GetState == PlayerComponent.PlayerStates.IDLE || GetState == PlayerComponent.PlayerStates.MOVING)
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

			if(evt.KeyBind == BrawlerUserInput.Instance.Jump && (evt.Type == UserInputKeyEvent.TYPE.KEYDOWN))
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
		}       
	}

}
