using UnityEngine;
using System.Collections;

public class BrawlerHittable : BaseObject
{
	private bool mHasCollider = false;

	protected override void Start()
	{
		EventManager.Instance.AddHandler<HitEvent>(HitEventHandler);
	}


	public void HitEventHandler(object sender, HitEvent hitEvent)
	{
		Vector3 hitDirection = Vector3.zero;
		Vector3 actualHitPoint = Vector3.zero;

		if (mCollider != null)
		{
			if (mCollider.bounds.Intersects(hitEvent.HitBounds))
			{
				hitDirection = (mCollider.bounds.center - hitEvent.HitBounds.center);
				actualHitPoint = hitEvent.HitBounds.center + hitDirection;

				actualHitPoint -= mCollider.bounds.extents;
				actualHitPoint -= hitEvent.HitBounds.extents;

				actualHitPoint += mCollider.bounds.extents;

				OnHit(actualHitPoint);
			}
		}
		else if (mRenderer != null)
		{
			if (mRenderer.bounds.Intersects(hitEvent.HitBounds))
		    {
				OnHit(actualHitPoint);
			}
		}
		else
		{
			Debug.Log(string.Format("Hittable Object {0} has no collider or renderer, and thus can't check hit events properly", mGameObject.name), this);
		}
	}

	protected virtual void OnHit(Vector3 hitLocation)
	{
		//override to do stuff on hit
	}

	private void OnDestroy()
	{
		EventManager.Instance.RemoveHandler<HitEvent>(HitEventHandler);
	}

}
