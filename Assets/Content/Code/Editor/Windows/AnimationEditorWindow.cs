using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using GrendelEditor.UI;

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
	private float mDeltaTime = 0f;
	private float mLastFrameTime = 0f;
	private Color mPreviewColor = Color.white;
	private float mPreviewTickTime = 0f;
	private float mPreviewTickTimeCurrent = 0f;
	private int mCurrentSelectedPreview = -1;
	private float mAnimationTicksPerSecond = 60f;
	private bool mNeedCharacterRefresh = true;
	private bool mMouseDragging = false;
	private GUIStyle mHitboxStyle = null;
	private CurrentEditingHitbox mCurrentEditingHitbox;
	private const float kHitBoxResizeBorderFactor = 0.15f;

	class CurrentEditingHitbox
	{
		public int FrameNumber;
		public BrawlerHitboxSettings HitboxSettings;
	}

	private const float kGotoButtonWidth = 48f;
	private const float kAnimPreviewWidth = 128f;
	private const float kAnimControlLabelWidth = 64f;
	private const string kCharacterDirectoryPath = "Prefabs/Characters";
	private const float kFramePreviewWidth = 128f;
	private const float kControlWidthTiny = 32f;
	private const float kControlWidthSmall = 64f;
	private const float kControlWidthMedium = 128f;
	private const float kControlWidthLarge = 256f;
	private const float kFrameEditorSpriteWidth = 256f;
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
		mWindowReference.wantsMouseMove = true;

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
		if (mWindowReference == null)
		{
			mWindowReference = this;
			mWindowReference.wantsMouseMove = true;
		}

		EditorUtility.DisplayProgressBar("Loading Characters", string.Empty, 0.5f);

		mCharacterList = new List<BrawlerPlayerComponent>( Resources.LoadAll<BrawlerPlayerComponent> (kCharacterDirectoryPath) );

		mCharacterNameList = new string[mCharacterList.Count];

		mCharacterNameList = ListToStringArray<BrawlerPlayerComponent>(mWindowReference.mCharacterList);

		EditorUtility.ClearProgressBar();

		mNeedCharacterRefresh = false;
	}

	private void OnGUI()
	{
		if (EditorApplication.isCompiling || EditorApplication.isUpdating && !mNeedCharacterRefresh)
		{
			mNeedCharacterRefresh = true;
		}

		if (mNeedCharacterRefresh && !EditorApplication.isCompiling && !EditorApplication.isUpdating)
		{
			GetCharacters();
		}

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

		EditorGUILayout.Space();

		PreviewControl();

		EditorGUILayout.Space();

		AnimationSettings();

		EditorGUILayout.Space();

		FramePreviews();

		EditorGUILayout.Space();

		FrameEditor();

		if (EditorGUI.EndChangeCheck() && CurrentClip != null)
		{
			EditorUtility.SetDirty(CurrentClip);
		}

		if (Event.current.type == EventType.dragUpdated && !mMouseDragging)
		{
			mMouseDragging = true;
		}
		else if (Event.current.type == EventType.DragExited && mMouseDragging)
		{
			mMouseDragging = false;
		}

		Repaint();	
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

		GUI.color = mPreviewColor;

		GUILayout.Box(new GUIContent(CurrentClip.CurrentSprite.texture), mEmptyStyle, GUILayout.Width(kAnimPreviewWidth), GUILayout.Height(kAnimPreviewWidth));

		GUI.color = Color.white;

		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		
		GUILayout.FlexibleSpace();

		GUILayout.Label(string.Format("Frame: {0} of {1}", (CurrentClip.CurrentFrame + 1).ToString(), CurrentClip.Frames.Length.ToString()), GUILayout.Width(kFramePreviewWidth));

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

		EditorGUILayout.Space();

		GUILayout.Label("Preview Color:", GUILayout.Width(kControlWidthSmall + kControlWidthTiny));

		mPreviewColor = EditorGUILayout.ColorField(mPreviewColor, GUILayout.Width(kControlWidthTiny));

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		EditorGUILayout.Space();
	}

	private void AnimationSettings()
	{
		GUILayout.Label("Animation Settings:", EditorStyles.toolbarDropDown);

		EditorGUILayout.Space();

		GUILayout.BeginVertical(GUI.skin.textArea);

		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.Label("Clip FPS:", GUILayout.Width(kControlWidthSmall));

		CurrentClip.FramesPerSecond = EditorGUILayout.FloatField (CurrentClip.FramesPerSecond, GUILayout.Width(kControlWidthTiny));

		EditorGUILayout.Space();

		GUILayout.Label("Loop Mode:", GUILayout.Width(kControlWidthSmall + 8f));

		CurrentClip.LoopMode = (BrawlerAnimationClip.LoopModes)EditorGUILayout.EnumPopup(CurrentClip.LoopMode, GUILayout.Width(kControlWidthMedium));

		EditorGUILayout.Space();
		
		GUILayout.Label("Start Frame:", GUILayout.Width(kControlWidthSmall + 12f));
		
		CurrentClip.StartingFrame = EditorGUILayout.IntField(CurrentClip.StartingFrame, GUILayout.Width(kControlWidthTiny));

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		EditorGUILayout.Space();

		GUILayout.EndVertical();
	}

	private void FramePreviews()
	{
		if (mWindowReference == null)
		{
			mWindowReference = this;
			mWindowReference.wantsMouseMove = true;
		}

		int framesPerRow = (int)(mWindowReference.position.width / kControlWidthMedium);
		int rowCount = 1;

		GUILayout.Label("Frame Previews:", EditorStyles.toolbarDropDown);

		EditorGUILayout.Space();

		GUILayout.BeginVertical(GUI.skin.textArea);

		GUILayout.BeginHorizontal();

		for(int frameCount = 0; frameCount < CurrentClip.Frames.Length; frameCount++)
		{
			PreviewFrame(CurrentClip.Frames[frameCount], frameCount);

			if (rowCount == framesPerRow)
			{
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				rowCount = 1;
			}
			else
			{
				rowCount++;
			}
		}

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
	}

	private void PreviewFrame(BrawlerFrameEntry frame, int frameCount)
	{
		Sprite tempSprite;
		
		Rect tempRect;

		tempSprite = CurrentClip.Sprites[ frame.SpriteIndex ];

		GUILayout.Box(string.Empty, GUILayout.Width(kControlWidthMedium), GUILayout.Height(kControlWidthMedium));

		tempRect = GUILayoutUtility.GetLastRect();

		if(mMouseDragging && tempRect.Contains(Event.current.mousePosition))
		{
			GUI.color = Color.cyan;
		}

		if(GUI.Button(tempRect, string.Empty))
		{
			mCurrentSelectedPreview = frameCount;
			Event.current.Use();
		}

		if (mCurrentSelectedPreview == frameCount)
		{
			GUI.color = Color.yellow;
			GUI.Box(tempRect, string.Empty);
			GUI.color = Color.white;
		}

		if (CurrentClip.IsPlaying && CurrentClip.CurrentFrame == frameCount)
		{
			GUI.color = Color.green;
			GUI.Box(tempRect, string.Empty);
			GUI.color = Color.white;
		}

		GUI.color = Color.white;

		GUI.DrawTexture(tempRect, tempSprite.texture);

		GUI.Label(tempRect, string.Format("{0} / {1}", (frameCount + 1).ToString(), CurrentClip.Frames.Length.ToString()), EditorStyles.whiteMiniLabel);
	}

	private void FrameEditor()
	{
		if (mCurrentSelectedPreview == -1)
		{
			return;
		}

		if (mCurrentSelectedPreview > CurrentClip.Frames.Length)
		{
			mCurrentSelectedPreview = -1;
			return;
		}

		EditorGUI.BeginChangeCheck();

		GUILayout.Label("Frame Editor:", EditorStyles.toolbarDropDown);

		EditorGUILayout.Space();

		GUILayout.BeginVertical(GUI.skin.textArea);

		GUILayout.BeginHorizontal();

		GUILayout.BeginVertical();

		HitboxControl(CurrentClip.Frames[mCurrentSelectedPreview].AttackBoxSettings, "Attack Box", Color.red);
		HitboxControl(CurrentClip.Frames[mCurrentSelectedPreview].HeadBoxSettings, "Head Box", Color.blue);
		HitboxControl(CurrentClip.Frames[mCurrentSelectedPreview].BodyBoxSettings, "Body Box", Color.blue);
		HitboxControl(CurrentClip.Frames[mCurrentSelectedPreview].LegBoxSettings, "Leg Box", Color.blue);
		HitboxControl(CurrentClip.Frames[mCurrentSelectedPreview].CollisionBoxSettings, "Collision Box", Color.green);

		GUILayout.EndVertical();

		GUILayout.FlexibleSpace();

		GUILayout.Box(string.Empty, mEmptyStyle, GUILayout.Width(kFrameEditorSpriteWidth), GUILayout.Height(kFrameEditorSpriteWidth));

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		Rect hitboxControlsRect = GUILayoutUtility.GetLastRect();

		GUI.DrawTexture(CenterRectOnOtherRect(new Rect(0,0,kFrameEditorSpriteWidth, kFrameEditorSpriteWidth), hitboxControlsRect) , CurrentClip.Sprites[ CurrentClip.Frames[mCurrentSelectedPreview].SpriteIndex ].texture);
	
		HitboxEditor(hitboxControlsRect, CurrentClip.Frames[mCurrentSelectedPreview].AttackBoxSettings, Color.red, mCurrentEditingHitbox == null ? false : mCurrentEditingHitbox.HitboxSettings == CurrentClip.Frames[mCurrentSelectedPreview].AttackBoxSettings, 100);	
		HitboxEditor(hitboxControlsRect, CurrentClip.Frames[mCurrentSelectedPreview].HeadBoxSettings, Color.cyan, mCurrentEditingHitbox == null ? false : mCurrentEditingHitbox.HitboxSettings == CurrentClip.Frames[mCurrentSelectedPreview].HeadBoxSettings, 101);
		HitboxEditor(hitboxControlsRect, CurrentClip.Frames[mCurrentSelectedPreview].BodyBoxSettings, Color.cyan, mCurrentEditingHitbox == null ? false : mCurrentEditingHitbox.HitboxSettings == CurrentClip.Frames[mCurrentSelectedPreview].BodyBoxSettings, 102);
		HitboxEditor(hitboxControlsRect, CurrentClip.Frames[mCurrentSelectedPreview].LegBoxSettings, Color.cyan, mCurrentEditingHitbox == null ? false : mCurrentEditingHitbox.HitboxSettings == CurrentClip.Frames[mCurrentSelectedPreview].LegBoxSettings, 103);
		HitboxEditor(hitboxControlsRect, CurrentClip.Frames[mCurrentSelectedPreview].CollisionBoxSettings, Color.green, mCurrentEditingHitbox == null ? false : mCurrentEditingHitbox.HitboxSettings == CurrentClip.Frames[mCurrentSelectedPreview].CollisionBoxSettings, 104);
	
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(CurrentClip);
		}
	}

	private void HitboxControl(BrawlerHitboxSettings settings, string name, Color color)
	{
		EditorGUILayout.Space();
		GUI.color = Color.Lerp(color, Color.white, settings.Active ? 0.45f : 0.75f);

		GUILayout.BeginVertical(GUI.skin.textArea, GUILayout.Width(kControlWidthLarge));

		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		GUILayout.Label(string.Format("{0}:", name), EditorStyles.toolbarButton);

		string buttonText = "Add";

		if (settings.Active)
		{
			buttonText = "Select";
		}

		if (mCurrentEditingHitbox != null && mCurrentEditingHitbox.HitboxSettings == settings)
		{
			buttonText = "Deselect";
		}

		if( GUILayout.Button(buttonText, EditorStyles.toolbarButton) )
		{
			if (!settings.Active)
			{
				settings.Active = true;
			}
			else if(settings.Active && mCurrentEditingHitbox != null && mCurrentEditingHitbox.HitboxSettings == settings)
			{
				mCurrentEditingHitbox = null;
			}
			else
			{
				if (mCurrentEditingHitbox == null)
				{
					mCurrentEditingHitbox = new CurrentEditingHitbox();
				}

				mCurrentEditingHitbox.FrameNumber = mCurrentSelectedPreview;
				mCurrentEditingHitbox.HitboxSettings = settings;
			}
		}

		if (settings.Active)
		{
			if(GUILayout.Button("X", EditorStyles.toolbarButton))
			{
				settings.Active = false;
				if (mCurrentEditingHitbox != null && mCurrentEditingHitbox.HitboxSettings == settings)
				{
					mCurrentEditingHitbox = null;
				}
			}
		}

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		if (settings.Active)
		{
			settings.Position = EditorGUILayout.RectField(settings.Position);
		}

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		EditorGUILayout.Space();

		GUILayout.EndVertical();

		GUI.color = Color.white;
		EditorGUILayout.Space();
	}

	private void HitboxEditor(Rect areaRect, BrawlerHitboxSettings settings, Color color, bool editing, int windowID)
	{
		if (!settings.Active)
		{
			return;
		}

		Rect invertedRect = new Rect(settings.Position);
		invertedRect.y *= -1;
		Rect previewRect = new Rect( CenterRectOnOtherRect(invertedRect, areaRect) );

		Color col = Color.Lerp(color, Color.clear, editing ? 0.15f : 0.55f);
		
		GUI.color = col;

//		Rect moveRect = new Rect( previewRect );
//
//		moveRect.width = moveRect.width - (moveRect.width * (kHitBoxResizeBorderFactor * 2));
//		moveRect.height = moveRect.height - (moveRect.height * (kHitBoxResizeBorderFactor * 2));
//
//		Rect moveRectLeft = new Rect( previewRect );
//
//		//moveRectLeft.x -= (previewRect.width * 0.5f) + ((previewRect.width * kHitBoxResizeBorderFactor) * 0.5f);
//		moveRectLeft.width = (previewRect.width * kHitBoxResizeBorderFactor);

		if (editing)
		{
			Rect newRect = new Rect( CustomEditorGUI.ResizableBox(previewRect, col, 6f, GUI.skin.textArea) );
			previewRect.y *= -1;
			newRect.y *= -1;
			settings.Position = new Rect(settings.Position.x + (newRect.center - previewRect.center).x, settings.Position.y + (newRect.center.y - previewRect.center.y),
			                               newRect.width, newRect.height);
		}
		else
		{
			GUI.Box(previewRect, string.Empty, GUI.skin.textArea);
		}

		GUI.color = Color.white;
	}

	private void EmptyWindow(int windowID)
	{
		GUILayout.BeginVertical();

		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.BeginVertical();
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

	private static Rect CenterRectOnOtherRect(Rect newRect, Rect otherRect)
	{
		newRect.center = otherRect.center + new Vector2(newRect.x, newRect.y);
		return newRect;	
	}
}
