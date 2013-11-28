using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCamera : BaseObject
{

    public float DefaultFieldOfView = 60f;
    public Vector3 OffsetFromTarget = new Vector3(0,0,-30);
	public float TrackingSpeed = 1f;
	public List<BrawlerPlayerComponent> PlayerList = new List<BrawlerPlayerComponent>();


	void Start()
	{
		PlayerList = new List<BrawlerPlayerComponent>((BrawlerPlayerComponent[])Object.FindObjectsOfType(typeof(BrawlerPlayerComponent)));
	}

    //TODO: Move zoom keyhandling to the input system
	void FixedUpdate ()
    {
		mTransform.position = Vector3.Slerp(mTransform.position, PlayerList[0].transform.position + OffsetFromTarget, TrackingSpeed * Time.deltaTime); 	     	   
	}
}
