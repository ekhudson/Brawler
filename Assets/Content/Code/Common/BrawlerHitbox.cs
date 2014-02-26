using UnityEngine;
using System.Collections;

public class BrawlerHitbox : TriggerVolume 
{
	public enum HitboxTypes
	{
		Head,
		Body,
		Leg,
		MoveCollider,
		Attack,
	}

	public HitboxTypes HitboxType;

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			return;
		}

		if (HitboxType == HitboxTypes.Head || HitboxType == HitboxTypes.Body || HitboxType == HitboxTypes.Leg)
	    {
			Gizmos.color = Color.cyan;
		}
		else if (HitboxType == HitboxTypes.MoveCollider)
		{
			Gizmos.color = Color.green;
		}
		else if (HitboxType == HitboxTypes.Attack)
		{
			Gizmos.color = Color.red;
		}

		Gizmos.DrawCube(mCollider.bounds.center, mCollider.bounds.extents);

		Gizmos.color = Color.white;
	}
}
