using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCamera : BaseObject
{

    public float DefaultFieldOfView = 60f;
    public float DistanceFromTarget = 30f;
	public float TrackingSpeed = 1f;
	public List<PlayerComponent> PlayerList = new List<PlayerComponent>();


	void Start()
	{
		PlayerList = new List<PlayerComponent>((PlayerComponent[])Object.FindObjectsOfType(typeof(PlayerComponent)));
	}

    //TODO: Move zoom keyhandling to the input system
	void FixedUpdate ()
    {
		mTransform.position = Vector3.Slerp(mTransform.position, PlayerList[0].transform.position + new Vector3(0f, 0f, DistanceFromTarget), TrackingSpeed * Time.deltaTime); 	     	   
	}
}
