using UnityEngine;
using System.Collections;

[System.Serializable]
public class BrawlerHitboxSettings
{
	public bool Active = false;

	[SerializeField]private Rect mPosition;

	private const float kRectMaxWidth = 12f;

	public Rect Position
	{
		get
		{
			if (mPosition.width < kRectMaxWidth)
			{
				mPosition.width = kRectMaxWidth;
			}

			if (mPosition.height < kRectMaxWidth)
			{
				mPosition.height = kRectMaxWidth;
			}

			return mPosition;
		}
		set
		{
			mPosition = value;

			if (mPosition.width < kRectMaxWidth)
			{
				mPosition.width = kRectMaxWidth;
			}
			
			if (mPosition.height < kRectMaxWidth)
			{
				mPosition.height = kRectMaxWidth;
			}
		}
	}

	public void Translate(Vector2 delta)
	{
		mPosition.center += delta;
	}
}
