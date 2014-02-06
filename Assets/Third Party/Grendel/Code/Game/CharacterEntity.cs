using UnityEngine;
using System.Collections;

//A rigidbody based character controller for Grendel Entities
public class CharacterEntity : Entity
{
    public float SkinWidth = 0.01f;
    public float StepOffset = 0.35f;
    public float MinMoveAmount = 0.1f;
    private Vector3 mCurrentMove = Vector3.zero;
    private bool mIsGrounded = false;
	private PlayerStates mState = PlayerStates.USING;
	private int mLayer = 0;
	
	public enum PlayerStates
	{
		USING,
	}
	
	public PlayerStates State
	{
		get
		{
			return mState;
		}
	}
	
    public bool IsGrounded
    {
        get
        {
            return mIsGrounded;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return mCurrentMove;
        }
        set
        {
            mCurrentMove = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mRigidbody.isKinematic = false;
    }

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
		mLayer = mGameObject.layer;
    }
    
    // Update is called once per frame
    private void FixedUpdate ()
    {
        //if (!mIsGrounded)
        //{
            CheckIfGrounded();
        //}

        //mTransform.Translate(mCurrentMove, Space.World);
        //mRigidbody.MovePosition(mTransform.position + mCurrentMove);
        //mRigidbody.AddRelativeForce(mCurrentMove);
        mRigidbody.AddForce(mCurrentMove,ForceMode.VelocityChange);
		//mRigidbody.velocity = mCurrentMove;


        //mCurrentMove *= mRigidbody.drag;
		mCurrentMove = Vector3.zero;
    }

    public void Move(Vector3 amount)
    {
        mCurrentMove += amount;
    }

    public void CheckIfGrounded()
    {
        Ray ray = new Ray(mTransform.position, -mTransform.up);
        RaycastHit hit;

		mIsGrounded = false;

		if (Physics.Raycast(ray, out hit, (mCollider.bounds.size.y * 0.5f) + SkinWidth, ~(1 << mLayer)))
        {
            mIsGrounded = true;
        }

		Vector3 rayOrigin = ray.origin;

		//left bounds check
		ray.origin = new Vector3(rayOrigin.x - (mCollider.bounds.size.x * 0.25f), rayOrigin.y, rayOrigin.z);
		if (Physics.Raycast(ray, out hit, (mCollider.bounds.size.y * 0.5f) + SkinWidth, ~(1 << mLayer)))
		{
			mIsGrounded = true;
		}

		//right bounds check
		ray.origin = new Vector3(rayOrigin.x + (mCollider.bounds.size.x * 0.25f), rayOrigin.y, rayOrigin.z);
		if (Physics.Raycast(ray, out hit, (mCollider.bounds.size.y * 0.5f) + SkinWidth, ~(1 << mLayer)))
		{
			mIsGrounded = true;
		}

    }

	public void SetGrounded(bool grounded)
	{
		mIsGrounded = grounded;
	}

    public void OnCollisionExit(Collision collisionInfo)
    {
        //mIsGrounded = false;
    }

    private void OnDrawGizmosSelected()
    {
        if(!Application.isPlaying)
        {
            return;
        }

        Debug.DrawLine(mTransform.position - new Vector3(0, mCollider.bounds.size.y * 0.5f, 0), mTransform.position - new Vector3(0, (mCollider.bounds.size.y * 0.5f) + SkinWidth, 0), Color.yellow);
    }
}
