using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrawlerHUD : Singleton<BrawlerHUD>
{   
    public bool HideWindowsMouse = true;

	private Rect mScreenRect = new Rect();
	   
    private void Start()
    {
		if (HideWindowsMouse)
        {
            Screen.showCursor = false;
        }

		mScreenRect = Camera.main.rect;
	}

    private void OnGUI()
    {
		GUILayout.BeginArea(mScreenRect);

		DrawPlayerStatuses();   

		GUILayout.EndArea();
    }  


	private void DrawPlayerStatus()
	{
		GUILayout.BeginHorizontal();

		foreach (BrawlerPlayerComponent player in BrawlerPlayerManager.Instance.PlayerList)
		{
			DrawPlayerStatus(player);
		}

		GUILayout.EndHorizontal();
	}

	private void DrawPlayerStatus(BrawlerPlayerComponent player)
	{
		//GUILayout.Box(player
	}

}
