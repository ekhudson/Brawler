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
        Hittable,
	}

	public HitboxTypes HitboxType;

    public Entity ParentEntity;

    protected override void Start()
    {
        base.Start();
        if (ParentEntity == null)
        {
            ParentEntity = gameObject.GetComponent<Entity>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HitboxType != HitboxTypes.Attack) //TODO: Somehow make it so attack boxes are the only ones who do collision handling
        {
            return;
        }

        Debug.Log("hit something!");

        EventManager.Instance.Post(new HitEvent(this, ParentEntity, transform.position, 100f, ParentEntity.gameObject.transform.right));
    }

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
        else if (HitboxType == HitboxTypes.Hittable)
        {
            Gizmos.color = Color.Lerp(Color.yellow, Color.clear, 0.65f);
        }

		Gizmos.DrawCube(mCollider.bounds.center, mCollider.bounds.size);

		Gizmos.color = Color.white;
	}
}
