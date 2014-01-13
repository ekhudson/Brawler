﻿using UnityEngine;
using System.Collections;

public class BrawlerAIBase : BaseObject 
{
	protected BrawlerPlayerComponent mPlayerComponent;

	protected override void Start()
	{
		base.Start();
		mPlayerComponent = gameObject.GetComponent<BrawlerPlayerComponent>();
		mPlayerComponent.IsAI = true;
		mPlayerComponent.IsActivePlayer = true;
		gameObject.SetActive(true);
	}

	public void WakeAI()
	{
		Start();
	}
}