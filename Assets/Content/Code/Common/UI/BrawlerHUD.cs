using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrawlerHUD : Singleton<BrawlerHUD>
{   
    public bool HideWindowsMouse = true;
	   
    private void Start()
    {
		if (HideWindowsMouse)
        {
            Screen.showCursor = false;
        }
    }

    private void OnGUI()
    {
            
    }  

}
