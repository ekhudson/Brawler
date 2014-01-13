using UnityEngine;
using System.Collections;

public class HitEvent : EventBase 
{
	public readonly Vector3 HitPoint;
	public readonly Bounds HitBounds;
	public readonly float HitForce;
	public readonly Vector3 HitVector;

	public HitEvent(Object sender, Bounds hitBounds, Vector3 hitPoint, float hitForce, Vector3 hitVector) : base(hitPoint, sender)
	{		
		HitPoint = hitPoint;
		HitBounds = hitBounds;
		HitForce = hitForce;
		HitVector = hitVector;
	}
	
	public HitEvent()
	{		
		HitPoint = Vector3.zero;
		HitBounds = new Bounds();
		HitForce = 0f;
		HitVector = Vector3.zero;
	}
}
