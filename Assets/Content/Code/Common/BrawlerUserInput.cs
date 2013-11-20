using UnityEngine;
using System.Collections;

public class BrawlerUserInput : UserInput <BrawlerUserInput>
{
    [HideInInspector]public GrendelKeyBinding UseKey01 = new GrendelKeyBinding("Use01", KeyCode.W, KeyCode.W, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None);
    [HideInInspector]public GrendelKeyBinding UseKey02 = new GrendelKeyBinding("Use02", KeyCode.S, KeyCode.S, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None);    
    [HideInInspector]public GrendelKeyBinding RotateObjectRight = new GrendelKeyBinding("RotateObjectRight", KeyCode.E, KeyCode.DownArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None);
    [HideInInspector]public GrendelKeyBinding InspectObject = new GrendelKeyBinding("InspectObject", KeyCode.F, KeyCode.DownArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None);
	[HideInInspector]public GrendelKeyBinding MoveCharacter = new GrendelKeyBinding("Move Character", KeyCode.None, KeyCode.None, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.Joysticks.Joystick1);
}
