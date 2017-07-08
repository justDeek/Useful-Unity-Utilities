// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using UnityEditor;
using System.Security.Cryptography.X509Certificates;
using HutongGames.PlayMakerEditor;
using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory ("System")]
	[Tooltip("Finds and returns any Asset by Name, Label or Type. Filters Assets by their filename (without extension). CANNOT be used in build, only in Editor. Optionally returns the Path to the asset and the type.")]
	public class FindAssetByName : FsmStateAction
	{
		[Tooltip("Asset-Name or search with keywords. Use 'l:' or 't:' before any keyword to search by label or type.")]
		public FsmString assetName;

		[UIHint(UIHint.Variable)]
		public FsmString storePath;

		[UIHint(UIHint.Variable)]
		public FsmString storeType;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Stores the result in a pre-specified variable.")]
		public FsmObject storeAsset;

		public override void Reset()
		{
			assetName = "";
			storePath = null;
			storeType = null;
			storeAsset = null;
		}

		public override void OnEnter()
		{
			Find();
			Finish();
		}

		void Find()
		{
			string[] results;
			string path;
			System.Type userDefinedType = (Type.GetType (storeAsset.TypeName)); //maybe with System.Activator.CreateInstance ...
			if (!string.IsNullOrEmpty (assetName.Value)) 
			{
				results = AssetDatabase.FindAssets (assetName.Value);
				foreach (string guid in results) {
					path = AssetDatabase.GUIDToAssetPath (guid);
					//bis hier hin funtzts
					storeAsset.Value = AssetDatabase.LoadAssetAtPath (path, userDefinedType);

					if (storePath != null)
					{
						storePath.Value = path; //returns the complete path (Assets/../../assetName.extension)
					}

					if (storeType != null)
					{
						storeType.Value = storeAsset.TypeName; // returns the subtype (e.g. UI2DSprite)
					}

					return;

				}
				return;
			}

		}
			
	}
}