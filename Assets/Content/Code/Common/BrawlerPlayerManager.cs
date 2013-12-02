using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrawlerPlayerManager : Singleton<BrawlerPlayerManager> 
{
	public List<BrawlerPlayerComponent> PlayerList = new List<BrawlerPlayerComponent>();

	private int mCurrentActivePlayers = 1;

	public int CurrentActivePlayers
	{
		get
		{
			return mCurrentActivePlayers;
		}
	}	

	public List<Color> PlayerColours = new List<Color>
	{
		Color.white,
		Color.red,
		GrendelColor.GrendelYellow,
		Color.green,
	};

	private void Start()
	{
		PlayerList = new List<BrawlerPlayerComponent>((BrawlerPlayerComponent[])Object.FindObjectsOfType(typeof(BrawlerPlayerComponent)));
		SetupPlayerData();
	}

	private void SetupPlayerData()
	{
		int id = 1;

		foreach(BrawlerPlayerComponent player in PlayerList)
		{
			player.SetID(id);
			player.SetGamepadID(id - 1);

			if (BrawlerUserInput.Instance.IsGamePadActive(id - 1))
		    {
				mCurrentActivePlayers++;
				player.IsActivePlayer = true;
			}
			else if (id != 1) //don't disable the first player
			{
				player.IsActivePlayer = false;
				player.gameObject.SetActive(false);
			}

			player.SetPlayerColor(PlayerColours[id - 1]);
			id++;
		}
	}

}
