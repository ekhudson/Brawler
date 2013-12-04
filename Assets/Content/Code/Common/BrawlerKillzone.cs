using UnityEngine;
using System.Collections;

public class BrawlerKillzone : TriggerVolume 
{
	public Transform SpawnPoint;
	public Transform KillParticlePrefab;
	public LayerMask PlayerLayer;

	public override void OnTriggerEnter(Collider collider)
	{		
		if ((1 << collider.gameObject.layer) == PlayerLayer)
		{
			collider.transform.position = SpawnPoint.position;
		}
		
		base.OnTriggerEnter(collider);
	}
}
