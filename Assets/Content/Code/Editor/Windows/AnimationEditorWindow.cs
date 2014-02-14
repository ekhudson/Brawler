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
	private const float kAnimControlLabelWidth = 64f;
	private const string kCharacterDirectoryPath = "Prefabs/Characters";
	private const float kFramePreviewWidth = 128f;
	private const float kControlWidthTiny = 32f;
	private const float kControlWidthSmall = 72f;
	private const float kControlWidthMedium = 128f;
	private const float kControlWidthLarge = 256f;

	private float mDeltaTime = 0f;
	private float mLastFrameTime = 0f;
	private float mPreviewTickTime = 0f;
	private float mPreviewTickTimeCurrent = 0f;
	private float mAnimationTicksPerSecond = 60f;

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

		mWindowReference.GetCharacters();

		mWindowReference.Show();
	}

	private void OnProjectChange()
	{
		GetCharacters();
	}

	private void GetCharacters()
	{
		EditorUtility.DisplayProgressBar("Loading Characters", string.Empty, 0.5f);

		mCharacterList = new List<BrawlerPlayerComponent>( Resources.LoadAll<BrawlerPlayerComponent> (kCharacterDirectoryPath) );
		
		mCharacterNameList = ListToStringArray<BrawlerPlayerComponent>(mWindowReference.mCharacterList);

		EditorUtility.ClearProgressBar();
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
		AnimationSettings();
		FramePreviews();
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

		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		
		GUILayout.FlexibleSpace();

		GUILayout.Label(string.Format("Frame: {0} of {1}", CurrentClip.CurrentFrame.ToString(), (CurrentClip.Frames.Length - 1).ToString()), GUILayout.Width(kFramePreviewWidth));

		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		if (focusedWindow == this && CurrentClip.IsPlaying)
		{
			if (mPreviewTickTimeCurrent > mPreviewTickTime)
			{
				CurrentClip.RecalculateFrameTime();
				CurrentClip.Tick((float)EditorApplication.timeSinceStartup);
				mPreviewTickTimeCurrent = 0;
			}

			Repaint();	
		}

		mLastFrameTime = (float)EditorApplication.timeSinceStartup;
	}

	private void PreviewControl()
	{
		GUILayout.BeginHorizontal();

		GUILayout.FlexibleSpace();	

		if (CurrentClip.IsPlaying)
		{
			GUI.color = Color.green;
		}

		if (GUILayout.Button( CurrentClip.IsPlaying ? "Stop" : "Play" , GUILayout.Width(kControlWidthSmall)))
		{
			if (CurrentClip.IsPlaying)
			{
				CurrentClip.Stop();
			}
			else if (!CurrentClip.IsPlaying)
			{
				CurrentClip.Play();
			}
		}

		GUI.color = Color.white;

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();
	}

	private void AnimationSettings()
	{
		GUILayout.BeginVertical(GUI.skin.textArea);

		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.Label("Clip FPS:", GUILayout.Width(kControlWidthSmall));

		CurrentClip.FramesPerSecond = EditorGUILayout.FloatField (CurrentClip.FramesPerSecond, GUILayout.Width(kControlWidthTiny));

		EditorGUILayout.Space();

		GUILayout.Label("Loop Mode:", GUILayout.Width(kControlWidthSmall));

		CurrentClip.LoopMode = (BrawlerAnimationClip.LoopModes)EditorGUILayout.EnumPopup(CurrentClip.LoopMode, GUILayout.Width(kControlWidthMedium));

		EditorGUILayout.Space();
		
		GUILayout.Label("Start Frame:", GUILayout.Width(kControlWidthSmall));
		
		CurrentClip.StartingFrame = EditorGUILayout.IntField(CurrentClip.StartingFrame, GUILayout.Width(kControlWidthTiny));

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		EditorGUILayout.Space();

		GUILayout.EndVertical();
	}

	private void FramePreviews()
	{
		GUILayout.BeginHorizontal();

		Sprite tempSprite;

		for(int frameCount = 0; frameCount < CurrentClip.Frames.Length; frameCount++)
		{
			tempSprite = CurrentClip.Sprites[ CurrentClip.Frames[frameCount].SpriteIndex ];
				
			tempSprite = (Sprite)EditorGUILayout.ObjectField(new GUIContent(tempSprite.texture), tempSprite, typeof(Sprite), false, GUILayout.Width(kControlWidthSmall), GUILayout.Height(kControlWidthSmall));
		}

		GUILayout.EndHorizontal();
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
