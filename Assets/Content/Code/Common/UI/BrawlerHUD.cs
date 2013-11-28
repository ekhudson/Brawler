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

		mScreenRect = new Rect(0,0, Screen.width, Screen.height);
	}

    private void OnGUI()
    {
		GUILayout.BeginArea(mScreenRect);

		DrawPlayerStatuses();   

		GUILayout.EndArea();
    }  


	private void DrawPlayerStatuses()
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
		GUILayout.FlexibleSpace();
		GUI.color = player.PlayerColor;
		GUILayout.BeginHorizontal(GUI.skin.button, GUILayout.Width(256f));
		GUILayout.Label(player.PlayerID.ToString());
		GUILayout.Label(string.Format("Gamepad Active: {0}",BrawlerUserInput.Instance.IsGamePadActive(player.AssociatedGamepad).ToString()));
		GUILayout.EndHorizontal();
		GUI.color = Color.white;
		GUILayout.FlexibleSpace();
	}

}
