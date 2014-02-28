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
			Gizmos.color = Color.Lerp(Color.cyan, Color.clear, 0.65f);
		}
		else if (HitboxType == HitboxTypes.MoveCollider)
		{
			Gizmos.color = Color.Lerp(Color.green, Color.clear, 0.65f);
		}
		else if (HitboxType == HitboxTypes.Attack)
		{
			Gizmos.color = Color.Lerp(Color.red, Color.clear, 0.65f);
		}

		Gizmos.DrawCube(mCollider.bounds.center, mCollider.bounds.size);

		Gizmos.color = Color.white;
	}
}
