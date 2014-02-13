using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCamera : BaseObject
{

    public float DefaultFieldOfView = 60f;
    public Vector3 OffsetFromTarget = new Vector3(0,0,-30);
	public float TrackingSpeed = 1f;

	void FixedUpdate ()
    {
		Vector3 newPosition = Vector3.zero;

		if (BrawlerPlayerManager.Instance.CurrentActivePlayers == 0)
		{
			return;
		}

		foreach(BrawlerPlayerComponent player in BrawlerPlayerManager.Instance.PlayerList)
		{
			if (player.IsActivePlayer)
			{
				newPosition += player.transform.position;
			}
		}

		newPosition /= BrawlerPlayerManager.Instance.CurrentActivePlayers;


		mTransform.position = Vector3.Slerp(mTransform.position, newPosition + OffsetFromTarget, TrackingSpeed * Time.deltaTime); 	     	   
	}
}
