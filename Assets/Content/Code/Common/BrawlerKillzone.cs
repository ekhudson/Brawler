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
			Transform go = (Transform)Instantiate(KillParticlePrefab, collider.transform.position, Quaternion.identity);
			ParticleSystem deathParticle = go.GetComponent<ParticleSystem>();

			if (deathParticle != null)
			{
				deathParticle.startColor = collider.GetComponent<BrawlerPlayerComponent>().PlayerColor;
				Destroy (go.gameObject, deathParticle.duration);
			}

			collider.transform.position = SpawnPoint.position;
		}
		
		base.OnTriggerEnter(collider);
	}
}
