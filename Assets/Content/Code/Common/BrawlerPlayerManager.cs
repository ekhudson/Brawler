using UnityEngine;
using System.Collections;

public class BrawlerPlayerManager : Singleton<BrawlerPlayerManager> 
{
	public List<BrawlerPlayerComponent> PlayerList = new List<BrawlerPlayerComponent>();

	private void Start()
	{
		PlayerList = new List<BrawlerPlayerComponent>((BrawlerPlayerComponent[])Object.FindObjectsOfType(typeof(BrawlerPlayerComponent)));
	}

}
