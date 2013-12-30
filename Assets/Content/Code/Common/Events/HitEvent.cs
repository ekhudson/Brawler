using UnityEngine;
using System.Collections;

public class HitEvent : EventBase 
{
	public readonly Vector3 HitPoint;
	public readonly Bounds HitBounds;

	public HitEvent(Object sender, Bounds hitBounds, Vector3 hitPoint) : base(hitPoint, sender)
	{		
		HitPoint = hitPoint;
		HitBounds = hitBounds;
	}
	
	public HitEvent()
	{		
		HitPoint = Vector3.zero;
		HitBounds = new Bounds();
	}
}
