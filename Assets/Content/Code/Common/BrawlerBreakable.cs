using UnityEngine;
using System.Collections;

public class BrawlerBreakable : BaseObject 
{
	public float Health = 100;
	public Transform DamageParticle;
	public Transform BreakParticle;

	private float mCurrentHealth = 100;

	private const float kTestDamage = 50f;

	private void Start()
	{
		mCurrentHealth = Health;
	}

	private void OnCollisionEnter(Collision collision)
	{
		BrawlerPlayerComponent player = collision.gameObject>GetComponentInChildren<BrawlerPlayerComponent>();

		if (player != null)
		{
			if (player.GetState == BrawlerPlayerComponent.PlayerStates.HURT)
			{
				TakeDamage(kTestDamage, collid);
			}
		}


	}

	private void TakeDamage(float dmgAmt, Vector3 dmgLocation)
	{
		mCurrentHealth -= dmgAmt;

		if (mCurrentHealth <= 0)
		{
			DestroyBreakable();
		}
	}

	private void DestroyBreakable()
	{

	}
}
