﻿// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

//#if !(UNITY_TVOS || UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID || UNITY_FLASH || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE || UNITY_BLACKBERRY || UNITY_WP8 || UNITY_PSM || UNITY_WEBGL)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Android Native Audio")]
	[Tooltip("Lets you load a local StreamingAssets AudioClip into an AudioSource at runtime.")]
	public class AndroidNativeAudioNonRedundantFile : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The path after StreamingAssets + extension (e.g. '/Native.wav').")]
		public FsmString filePath;

		[ActionSection("Results")]

		[ObjectType(typeof(AudioClip))]
		[Tooltip("Gets the AudioClip from the StreamingAssets Path.")]
		public FsmObject storeObject;

		[Tooltip("If the converted Clip should be compressed or native.")]
		public FsmBool isCompressed;

		[ActionSection("Optional")]

		[CheckForComponent(typeof(AudioSource))]
		[Tooltip("Sets the loaded Clip directly into the specified AudioSource.")]
		public FsmGameObject audioSource;

		[Tooltip("If it should play immediately after setting the AudioClip.")]
		public FsmBool play;

		//[Tooltip("Error message if the file couldn't be found.")]
		private FsmString errorString;

		[ActionSection("Events")] 

		[Tooltip("Event to send when the data has finished loading (progress = 1).")]
		public FsmEvent isDone;

		[Tooltip("Event to send if there was an error.")]
		public FsmEvent isError;

		private WWW wwwObject;

		public override void Reset()
		{
			filePath = null;
			storeObject = new FsmObject(){UseVariable=true};
			isCompressed = false;
			audioSource = new FsmGameObject(){UseVariable=true};
			play = false;
			errorString = null;
			isDone = null;
		}

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(filePath.Value))
			{
				errorString.Value = "No Filepath specified!";
				Finish();
				return;
			}

			if (wwwObject == null)
			{
				errorString.Value = "WWW Object is Null!";
				Finish();
				return;
			}

			wwwObject = new WWW("file:" + Application.streamingAssetsPath + filePath.Value);

			while (!wwwObject.isDone)
			{
				errorString.Value = wwwObject.error;

				if (!string.IsNullOrEmpty(wwwObject.error))
				{
					Finish();
					Fsm.Event(isError);
					return;
				}

				if (isCompressed.Value)
				{
					storeObject.Value = wwwObject.GetAudioClipCompressed (false, AudioType.WAV);
				} else
				{
					storeObject.Value = wwwObject.GetAudioClip (false, false, AudioType.WAV);
				}

				if (!audioSource.IsNone)
				{
					var audioSourceGO = audioSource.Value.GetComponent<AudioSource>();
					if (audioSourceGO != null)
					{
						audioSourceGO.clip = (AudioClip)storeObject.Value;
						if (play.Value)
							audioSourceGO.Play ();
					}
						

				}

				errorString.Value = wwwObject.error;

				Fsm.Event(string.IsNullOrEmpty(errorString.Value) ? isDone : isError);

				Finish();
			}

		}
			
	}

}

//#endif