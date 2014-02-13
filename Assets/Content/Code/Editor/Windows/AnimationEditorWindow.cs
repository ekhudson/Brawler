using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class AnimationEditorWindow : EditorWindow 
{
	private static AnimationEditorWindow mWindowReference = null;
	private List<BrawlerAnimationClip> mAnimationList = new List<BrawlerAnimationClip>();
	private int mCurrentAnimationIndex = 0;
	private string[] mAnimationNameList;
	private GUIStyle mEmptyStyle;

	private const float kGotoButtonWidth = 48f;
	private const float kAnimPreviewWidth = 128f;
	private const float kPreviewTickTimerWidth = 128f;

	private float mDeltaTime = 0f;
	private float mLastFrameTime = 0f;
	private float mPreviewTickTime = 0f;
	private float mPreviewTickTimeCurrent = 0f;
	private float mAnimationTicksPerSecond = 120f;

	private BrawlerAnimationClip CurrentClip
	{
		get
		{
			return mAnimationList[mCurrentAnimationIndex];
		}
	}

	[MenuItem("Brawler/Animation Window")]
	public static void Init()
	{
		mWindowReference = (AnimationEditorWindow)EditorWindow.GetWindow(typeof(AnimationEditorWindow));

		string[] paths = AssetDatabase.GetAllAssetPaths();

		EditorUtility.DisplayProgressBar("Loading Animations", string.Empty, 0.5f);

		foreach( string s in paths )			
		{			
			Object o = AssetDatabase.LoadAssetAtPath( s, typeof( BrawlerAnimationClip ) );
			
			if( o != null )				
			{
				mWindowReference.mAnimationList.Add(o as BrawlerAnimationClip);
			}			
		}

		EditorUtility.ClearProgressBar();

		mWindowReference.mAnimationNameList = ListToStringArray(mWindowReference.mAnimationList);

		mWindowReference.Show();
	}

	private void OnGUI()
	{
		AnimationSelector();
		AnimationPreview();
		PreviewControl();
	}

	private void AnimationSelector()
	{
		GUILayout.BeginVertical(GUI.skin.textField);
		
		if (mAnimationList.Count == 0 || mAnimationNameList.Length == 0)
		{
			GUILayout.Label("No Animations Found");
			return;
		}
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		
		mCurrentAnimationIndex = EditorGUILayout.Popup("Current Animation:", mCurrentAnimationIndex, mAnimationNameList);
		
		if(GUILayout.Button("Go To", EditorStyles.miniButton, GUILayout.Width(kGotoButtonWidth)))
		{
			Selection.activeGameObject = mAnimationList[mCurrentAnimationIndex].gameObject;
		}
		
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.EndVertical();
	}

	private void AnimationPreview()
	{
		mDeltaTime = (float)EditorApplication.timeSinceStartup - mLastFrameTime;

		mPreviewTickTimeCurrent += mDeltaTime;
		mPreviewTickTime = 1f / mAnimationTicksPerSecond;

		if (mEmptyStyle == null)
		{
			mEmptyStyle = new GUIStyle();
		}

		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.Box(new GUIContent(CurrentClip.CurrentSprite.texture), mEmptyStyle, GUILayout.Width(kAnimPreviewWidth), GUILayout.Height(kAnimPreviewWidth));

		GUILayout.Label(string.Format("{0} / {1}", System.Math.Round(mPreviewTickTimeCurrent, 2).ToString(), System.Math.Round(mPreviewTickTime, 2).ToString()), GUILayout.Width(kPreviewTickTimerWidth));

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		if (mPreviewTickTimeCurrent > mPreviewTickTime)
		{
			CurrentClip.Tick((float)EditorApplication.timeSinceStartup);
			mPreviewTickTimeCurrent = 0;
		}

		Repaint();	

		mLastFrameTime = (float)EditorApplication.timeSinceStartup;
	}

	private void PreviewControl()
	{
		GUILayout.BeginVertical(GUI.skin.textArea);

		GUILayout.BeginHorizontal();

		GUILayout.FlexibleSpace();

		mAnimationTicksPerSecond = EditorGUILayout.FloatField(mAnimationTicksPerSecond, GUILayout.Width(kGotoButtonWidth));

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

	}

	private static string[] ListToStringArray(List<BrawlerAnimationClip> list)
	{
		string[] names = new string[list.Count];

		int count = 0;

		foreach(BrawlerAnimationClip clip in list)
		{
			names[count] = clip.name;
			count++;
		}

		return names;
	}
}
