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

		float scaleFactor = (512f / 100f) / 512f;

		//boxCollider.center = new Vector3(spritePosition.x + (setting.Position.x * scaleFactor), spritePosition.y + (setting.Position.y * scaleFactor), mGameObject.transform.position.z);

		boxCollider.center = new Vector3((setting.Position.x * scaleFactor), (setting.Position.y * scaleFactor), 0f);

		boxCollider.size = new Vector3( (setting.Position.width * scaleFactor), (setting.Position.height * scaleFactor), 1f);
	}
}
