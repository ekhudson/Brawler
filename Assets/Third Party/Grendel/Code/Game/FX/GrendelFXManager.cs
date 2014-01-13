using UnityEngine;
using System.Collections;

public class GrendelFXManager : Singleton<GrendelFXManager> 
{
	public EffectPool FXPool;

	private void Start()
	{
		if (FXPool == null)
		{
			Debug.LogWarning("FX Pool not specified; FX pooling will not occur");
		}
	}

	public BaseObject SpawnEffect(GameObject effect, Vector3 position, Quaternion rotation, bool pooledOnly)
	{
		return FXPool.Instantiate(effect, position, rotation, pooledOnly) as BaseObject;
	}

}
