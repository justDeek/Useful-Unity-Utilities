using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Retrieve the length of the AudioClip inside the specified AudioSource or from the given object.")]
	public class GetAudioClipLength : ComponentAction<AudioSource>
	{

		[CheckForComponent(typeof(AudioSource))]
		[Tooltip("The GameObject with the AudioSource component.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Supply a clip directly.")]
		public FsmObject audioClip;

		[UIHint(UIHint.Variable)]
		[Tooltip("The length of the clip.")]
		public FsmFloat length;

		private AudioClip _audioClip;

		public override void Reset()
		{
			gameObject = null;
			audioClip = new FsmObject() { UseVariable = true };
			length = 0;
		}

		public override void OnEnter()
		{

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(go))
			{
				_audioClip = audio.clip as AudioClip;
				length.Value = _audioClip.length;
			}
			else
			{
				_audioClip = audioClip.Value as AudioClip;
				length.Value = _audioClip.length;
			}

			Finish();
		}
	}
}
