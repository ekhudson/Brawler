using UnityEngine;
using System.Collections;

public class RequestAudioEvent : EventBase 
{
	public readonly GrendelAudioChannel Channel;
	public readonly AdjustableAudioClip Clip;
	public readonly Transform AttachToTarget = null;
	public readonly Vector3 Location = Vector3.zero;
	public readonly bool ForceLooping = false;

	public RequestAudioEvent(GrendelAudioChannel channel, AdjustableAudioClip audioClip, Vector3 location, Transform attachToTarget, bool forceLooping, object sender) : base (Vector3.zero, sender)
	{
		Channel = channel;
		Clip = audioClip;
		AttachToTarget = attachToTarget;
		Location = location;
		ForceLooping = forceLooping;
	}

	public RequestAudioEvent(GrendelAudioChannel channel, AdjustableAudioClip audioClip, Vector3 location, Transform attachToTarget) : base (Vector3.zero, sender)
	{
		Channel = channel;
		Clip = audioClip;
		Location = location;
		AttachToTarget = attachToTarget;
	}

	public RequestAudioEvent(GrendelAudioChannel channel, AdjustableAudioClip audioClip, Vector3 location) : base (Vector3.zero, sender)
	{
		Channel = channel;
		Clip = audioClip;
		Location = location;
	}
}
