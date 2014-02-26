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

	public void ApplySettings(BrawlerHitboxSettings setting, BrawlerHitbox hitbox)
	{
		BoxCollider boxCollider = hitbox.BaseCollider as BoxCollider;

		Vector3 spritePosition = mGameObject.transform.parent.position;

		hitbox.transform.position = spritePosition; new Vector3(spritePosition.x + setting.Position.center.x, spritePosition.y + setting.Position.center.y, 0f);

		boxCollider.size = new Vector3(setting.Position.width * 0.5f, setting.Position.height * 0.5f, 1f);
	}
}
