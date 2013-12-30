using UnityEngine;
using System.Collections;

public class BrawlerBreakable : BrawlerHittable 
{
	public float Health = 100;
	public Transform DamageParticle;
	public Transform BreakParticle;

	private float mCurrentHealth = 100;

	private const float kTestDamage = 50f;

	protected override void Start()
	{
		base.Start();
		mCurrentHealth = Health;
	}

	protected override void OnHit(Vector3 hitLocation)
	{
		TakeDamage(10, hitLocation);
	}


//	private void OnCollisionEnter(Collision collision)
//	{
//		BrawlerPlayerComponent player = collision.gameObject.GetComponentInChildren<BrawlerPlayerComponent>();
//
//		if (player != null)
//		{
//			if (player.GetState == BrawlerPlayerComponent.PlayerStates.HURT)
//			{
//				TakeDamage(kTestDamage, collision.contacts[0].point);
//			}
//		}
//
//
//	}
//
	private void TakeDamage(float dmgAmt, Vector3 dmgLocation)
	{
		mCurrentHealth -= dmgAmt;

		if (mCurrentHealth <= 0)
		{
			DestroyBreakable();
		}
		else
		{
			Transform go = (Transform)Instantiate(DamageParticle, dmgLocation, Quaternion.identity);
			Destroy(go, 10f); //HACK: Make a better particle spawning system
		}
	}

	private void DestroyBreakable()
	{
		Transform go = (Transform)Instantiate(BreakParticle, mTransform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
