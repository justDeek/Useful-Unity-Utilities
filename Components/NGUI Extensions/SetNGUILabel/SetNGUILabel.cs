using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using GameDataEditor;
/*******************************************************************************
  A Component which applies the String found in the specified PlayMaker Global
	  to the referenced NGUI UILabel; most useful for translation purposes
			Also the ability to Debug the string-value it received
********************************************************************************/
public class SetNGUILabel : MonoBehaviour
{
	public string rawString = "";
	public string playerPrefsKeyName = "";
	public string playmakerGlobalName = "";
	public string gdeItemName = "";
	public string gdeFieldName = "";
	public UILabel uiLabelComponent = null;
	public string result = "";

	//for custom inspector:
	public bool showRawString, showPlayerPrefs, showPlayMakerGlobals, showGDE, usePlayMakerGlobalAsField;

	private string prevResult = "";
	private string currentGlobal = "";

	void Reset()
	{
		UILabel uiLabel = this.gameObject.GetComponent<UILabel>();
		if(uiLabel != null)
		{
			uiLabelComponent = uiLabel;
		}

		//"LMAYAQi" Project-specific defaults (delete if used in your own project)
		gdeItemName = this.gameObject.name;
		gdeFieldName = "currentLanguage";
		usePlayMakerGlobalAsField = true;
		showGDE = true;
	}

	void Start()
	{
		UpdateLabelText();
	}

	public void Update()
	{
		///Check if global has changed
		if(usePlayMakerGlobalAsField)
		{
			if(FsmVariables.GlobalVariables.Contains(gdeFieldName))
			{
				currentGlobal = FsmVariables.GlobalVariables.FindFsmString(gdeFieldName).Value;
				if(currentGlobal != null && currentGlobal != prevResult)
				{
					UpdateLabelText();
					prevResult = currentGlobal;
				}
			}
		}
	}

	void UpdateLabelText()
	{
		///if Raw String
		if(rawString != "")
		{
			result = rawString;
		}

		///if PlayerPrefs
		if(playerPrefsKeyName != "")
		{
			result = PlayerPrefs.GetString(playerPrefsKeyName);
		}

		///if PlayMaker Globals
		if(playmakerGlobalName != "")
		{
			var foundFSMString = FsmVariables.GlobalVariables.FindFsmString(playmakerGlobalName);
			if(foundFSMString != null)
				result = foundFSMString.Value;
		}

		///if GDE
		if(gdeItemName != "" && gdeFieldName != "")
		{
			string currentFieldName = gdeFieldName;
			if(usePlayMakerGlobalAsField)
			{
				var foundFSMString = FsmVariables.GlobalVariables.FindFsmString(gdeFieldName);
				if(foundFSMString != null)
					currentFieldName = foundFSMString.Value;
			}

			try
			{
				Dictionary<string, object> data;
				if(GDEDataManager.Get(gdeItemName, out data))
				{
					string val;
					data.TryGetString(currentFieldName, out val);
					result = val;
				}
				result = GDEDataManager.GetString(gdeItemName, currentFieldName, result);
			} catch(UnityException ex)
			{
				Debug.LogError(ex.ToString());
				return;
			}
		}

		///end result
		if(result != null)
		{
			uiLabelComponent.text = result;
		} else
		{
			Debug.LogWarning("No String retrieved! (at: " + this.gameObject + ")");
		}
	}
}
