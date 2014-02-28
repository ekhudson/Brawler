using UnityEngine;
using System.Collections;

public class BrawlerHitboxController : BaseObject 
{
	public BrawlerHitbox HeadCollider;
	public BrawlerHitbox BodyCollider;
	public BrawlerHitbox LegCollider;
	public BrawlerHitbox CollisionCollider;
	public BrawlerHitbox AttackCollider;

	private void Start()
	{

	}

	public void ApplySettings(BrawlerHitboxSettings setting, BrawlerHitbox hitbox, Sprite sprite)
	{
		BoxCollider boxCollider = hitbox.BaseCollider as BoxCollider;

		if (!setting.Active) 
		{
			hitbox.gameObject.SetActive (false);
			return;
		} 
		else if (!hitbox.gameObject.activeSelf) 
		{
			hitbox.gameObject.SetActive(true);
		}

		Vector3 spritePosition = mGameObject.transform.parent.position;

		float scaleFactor = 10f / 512f;

		Debug.Log (scaleFactor);

		hitbox.transform.position = new Vector3(spritePosition.x + (setting.Position.center.x * (scaleFactor * 0.5f)), spritePosition.y + (setting.Position.center.y * (scaleFactor * 0.5f)), mGameObject.transform.position.z);

		boxCollider.size = new Vector3( (setting.Position.width * (scaleFactor * 0.5f)), (setting.Position.height * (scaleFactor * 0.5f)), 1f);
	}
}
