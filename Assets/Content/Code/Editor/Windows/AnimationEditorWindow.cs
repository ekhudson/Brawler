using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class AnimationEditorWindow : EditorWindow 
{
	private static AnimationEditorWindow mWindowReference = null;
	private List<BrawlerPlayerComponent> mCharacterList = new List<BrawlerPlayerComponent>();
	private List<BrawlerAnimationClip> mAnimationList = new List<BrawlerAnimationClip>();
	private int mCurrentCharacterIndex = 0;
	private int mCurrentAnimationIndex = 0;
	private string[] mAnimationNameList;
	private string[] mCharacterNameList;
	private GUIStyle mEmptyStyle;

	private const float kGotoButtonWidth = 48f;
	private const float kAnimPreviewWidth = 128f;
	private const float kPreviewTickTimerWidth = 128f;
	private const string kCharacterDirectoryPath = "Prefabs/Characters";

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

		EditorUtility.DisplayProgressBar("Loading Characters", string.Empty, 0.5f);

		mWindowReference.mCharacterList = new List<BrawlerPlayerComponent>( Resources.LoadAll<BrawlerPlayerComponent> (kCharacterDirectoryPath) );

		mWindowReference.mCharacterNameList = ListToStringArray<BrawlerPlayerComponent>(mWindowReference.mCharacterList);

		EditorUtility.ClearProgressBar();

		mWindowReference.Show();
	}

	private void OnGUI()
	{
		EditorGUI.BeginChangeCheck ();

		if (mCharacterList.Count == 0 || mCharacterNameList.Length == 0)
		{
			GUILayout.Label("No Characters Found");
			return;
		}

		CharacterSelector();

		if (mAnimationList.Count == 0 || mAnimationNameList.Length == 0)
		{
			GUILayout.Label("No Animations Found");
			
			return;
		}

		AnimationPreview();
		PreviewControl();
	}

	private void CharacterSelector()
	{
		GUILayout.BeginVertical(GUI.skin.textField);

		mAnimationList = new List<BrawlerAnimationClip>( mCharacterList[mCurrentCharacterIndex].gameObject.GetComponentsInChildren<BrawlerAnimationClip>(true) );
		mAnimationNameList = ListToStringArray<BrawlerAnimationClip> (mAnimationList);
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();

		mCurrentCharacterIndex = EditorGUILayout.Popup("Current Character:", mCurrentCharacterIndex, mCharacterNameList);

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
			CurrentClip.RecalculateFrameTime();
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

		mAnimationTicksPerSecond = EditorGUILayout.FloatField("Animation Rate", mAnimationTicksPerSecond);
		CurrentClip.FramesPerSecond = EditorGUILayout.FloatField ("Animation FPS", CurrentClip.FramesPerSecond);

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

	}

	private static string[] ListToStringArray<T>(List<T> list) where T : Component
	{
		string[] names = new string[list.Count];

		int count = 0;

		foreach(T clip in list)
		{
			names[count] = clip.name;
			count++;
		}

		return names;
	}
}
