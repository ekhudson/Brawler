using UnityEngine;
using System.Collections;

[System.Serializable]
public class BrawlerHitboxSettings
{
	public bool Active = false;

	private Rect mPosition = new Rect(0f,0f,12f,12f);

	public Rect Position
	{
		get
		{
			if (mPosition.width < 12f)
			{
				mPosition.width = 12f;
			}

			if (mPosition.height < 12f)
			{
				mPosition.height = 12f;
			}

			return mPosition;
		}
		set
		{
			mPosition = value;

			if (mPosition.width < 12f)
			{
				mPosition.width = 12f;
			}
			
			if (mPosition.height < 12f)
			{
				mPosition.height = 12f;
			}
		}
	}
}
