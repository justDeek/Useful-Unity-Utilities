using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using GameDataEditor;
/*******************************************************************************
  A Component which applies the String found in the specified PlayMaker Global
		to the referenced NGUI UILabel; most useful for translation purposes
 		 			Also the ability to Debug the string-value it received
********************************************************************************/
public class SetNGUILabel : MonoBehaviour {

	public string rawString = "";
	public string playerPrefsKeyName = "";
	public string playmakerGlobalName = "";
	public string gdeItemName = "";
	public string gdeFieldName = "";
	public UILabel uiLabelComponent = null;
	public string result = "";

	private bool hasGlobal = false;

	void Reset() {
		hasGlobal = FsmVariables.GlobalVariables.Contains(playmakerGlobalName);
		UILabel uiLabel = this.gameObject.GetComponent<UILabel>();
		if (uiLabel != null) {
			uiLabelComponent = uiLabel;
		}
	}

	void Start () {
//if Raw String
		if (rawString != "") {
			result = rawString;
		}
//if PlayerPrefs
		if (playerPrefsKeyName != "") {
			result = PlayerPrefs.GetString(playerPrefsKeyName);
		}
//if PlayMaker Globals
		if (playmakerGlobalName != "") {
			var foundFSMString = FsmVariables.GlobalVariables.FindFsmString(playmakerGlobalName);
			if (foundFSMString != null)
				result = foundFSMString.Value;
		}
//if GDE
		if (gdeItemName != "" && gdeFieldName != "") {
			try
			{
				Dictionary<string, object> data;
				if (GDEDataManager.Get(gdeItemName, out data))
				{
						string val;
						data.TryGetString(gdeFieldName, out val);
						result = val;
				}
				result = GDEDataManager.GetString(gdeItemName, gdeFieldName, result);
			}
			catch(UnityException ex)
			{
				Debug.LogError(ex.ToString());
				return;
			}
		}
//Endresult
		if (result != null) {
			uiLabelComponent.text = result;
		} else {
			Debug.LogWarning("No String retrieved! (at: " + this.gameObject + ")");
		}

	}

}
