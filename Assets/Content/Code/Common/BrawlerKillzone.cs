using UnityEngine;
using System.Collections;

public class BrawlerKillzone : TriggerVolume 
{
	public Transform SpawnPoint;
	public Transform KillParticlePrefab;
	public LayerMask PlayerLayer;

	public override void OnTriggerEnter(Collider collider)
	{		
		if (collider.gameObject.layer == PlayerLayer.value)
		{
			collider.transform.position = SpawnPoint.position;
		}
		
		base.OnTriggerEnter(collider);
	}
}
