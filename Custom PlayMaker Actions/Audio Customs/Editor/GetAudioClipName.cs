
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Returns the Name of the provided AudioClip. Optionally includes the extension.")]
	public class GetAudioClipName : FsmStateAction
	{
		[ObjectType(typeof(AudioClip))]
		[Tooltip("The AudioClip to get the name of.")]
		public FsmObject audioClip;

		[Tooltip("The retrieved String-Variable.")]
		public FsmString audioClipName;

		[Tooltip("If it should include the file extension. (e.g. foo.wav instead of foo)")]
		public bool includeFileExtension;

		[Tooltip("Required for the extension: Set the path the file is at, starting after Assets (/Some/File/At/foo.extension). Otherwise uses 'AssetDatabase', wich doesn't work outside of the Editor (not in any build nor on any platform)")]
		public FsmString atPath;

		public override void Reset()
		{
			audioClip = null;
			includeFileExtension = true;
			atPath = null;
		}

		public override void OnEnter()
		{
			//Get the File-Name and trim the ObjectType at the end
			audioClipName = audioClip.ToString ().TrimEnd (" (UnityEngine.AudioClip)".ToCharArray ());
			if (includeFileExtension)
			{
				//Get Extension from Path starting after "Assets" and save it in a String
				String extension = "";
				if (atPath.Value != null && atPath.Value != "")
				{
					extension = Path.GetExtension(atPath.Value);
				}
				else
				{
					extension = Path.GetExtension (AssetDatabase.GetAssetPath (assetObject:audioClip.Value).ToString ().TrimStart ("Assets".ToCharArray ()));
				}

				//Combine AudioClip-Name with the File-Extension
				audioClipName = String.Concat (audioClipName, extension);
			}
			Finish();
		}
	}
}
