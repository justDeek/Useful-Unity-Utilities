//#if !(UNITY_TVOS || UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID || UNITY_FLASH || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE || UNITY_BLACKBERRY || UNITY_WP8 || UNITY_PSM || UNITY_WEBGL)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Android Native Audio")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=15458.0")]
	[Tooltip("Lets you load a local StreamingAssets AudioClip into an AudioSource at runtime.")]
	public class AndroidNativeAudioNonRedundantFile : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The path after StreamingAssets + extension (e.g. '/Native.wav'). The data must be in Ogg(Web/Standalones), MP3(phones) or WAV.")]
		public FsmString filePath;

		[ActionSection("Results")]

		[ObjectType(typeof(AudioClip))]
		[Tooltip("Gets the AudioClip from the StreamingAssets Path.")]
		public FsmObject storeObject;

		[Tooltip("This will load the data compressed and decompresses it as it plays.")]
		public FsmBool isCompressed;

		[Tooltip("Whether the clip should be completely downloaded before it's ready to play (false), or whether the stream can be played even if only part of the clip is downloaded (true). Only useful if 'isCompressed' is unticked.")]
		public FsmBool allowStreaming;

		[Tooltip("Specify whether the clip should be a 2D or 3D clip.")]
		public FsmBool is3D;

		[ActionSection("Optional")]

		[CheckForComponent(typeof(AudioSource))]
		[Tooltip("Sets the loaded Clip directly into the specified AudioSource.")]
		public FsmGameObject audioSource;

		[Tooltip("If it should play immediately after setting the AudioClip.")]
		public FsmBool play;

		[ActionSection("Events")]

		[Tooltip("Event to send when the data has finished loading (progress = 1).")]
		public FsmEvent isDone;

		[Tooltip("Event to send if there was an error.")]
		public FsmEvent isError;

		private WWW wwwObject;

		public override void Reset()
		{
			filePath = null;
			storeObject = new FsmObject() { UseVariable = true };
			isCompressed = false;
			allowStreaming = false;
			is3D = false;
			audioSource = new FsmGameObject() { UseVariable = true };
			play = false;
			isDone = null;
			isError = null;
		}

		public override void OnEnter()
		{
			// if (allowStreaming.Value = true)
			// {
			// 	isCompressed.Value = false;
			// }
			// if (isCompressed.Value = true)
			// {
			// 	allowStreaming.Value = false;
			// }

			if(string.IsNullOrEmpty(filePath.Value))
			{
				Debug.LogWarning("No Filepath specified!");
				Finish();
			}

			wwwObject = new WWW("file:" + Application.streamingAssetsPath + filePath.Value);

			if(wwwObject == null)
			{
				Debug.LogWarning("WWW Object is Null!");
				Finish();
			}

			while(!wwwObject.isDone)
			{
			}

			if(isCompressed.Value)
			{
				storeObject.Value = wwwObject.GetAudioClipCompressed(is3D.Value, AudioType.UNKNOWN); //MP3 files would be AudioType.MPEG
			} else
			{
				storeObject.Value = wwwObject.GetAudioClip(is3D.Value, allowStreaming.Value, AudioType.UNKNOWN);
			}

			if(!audioSource.IsNone)
			{
				var audioSourceGO = audioSource.Value.GetComponent<AudioSource>();
				if(audioSourceGO != null)
				{
					audioSourceGO.clip = (AudioClip)storeObject.Value;
					if(play.Value)
						audioSourceGO.Play();
				}
			}

			// //check if any error occured and send events accordingly
			Fsm.Event(string.IsNullOrEmpty(wwwObject.error) ? isDone : isError);

			Finish();

		}

	}

}
