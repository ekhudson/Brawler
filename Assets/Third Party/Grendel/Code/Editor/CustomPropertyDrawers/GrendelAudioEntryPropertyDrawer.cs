using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(GrendelAudioEntry))]
public class GrendelAudioEntryPropertyDrawer : PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		//GUILayout.BeginHorizontal();

		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
		EditorGUI.EndProperty();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(property.FindPropertyRelative("AudioBankNumber"));
		EditorGUILayout.PropertyField(property.FindPropertyRelative("AudioClipNumber"));
		EditorGUILayout.EndHorizontal();
		//GUILayout.EndHorizontal();
	}
}
