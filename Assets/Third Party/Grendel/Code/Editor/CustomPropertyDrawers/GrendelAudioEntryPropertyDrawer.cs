using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(GrendelAudioEntry))]
public class GrendelAudioEntryPropertyDrawer : PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		GUI.Label(position, label);
	}
}
