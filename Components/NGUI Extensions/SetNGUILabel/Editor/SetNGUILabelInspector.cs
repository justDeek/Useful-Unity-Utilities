using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using GameDataEditor;

[CustomEditor(typeof(SetNGUILabel))]
public class SetNGUILabelInspector : Editor
{
	public bool showLoadButton = true;

	public void OnEnable()
	{
		showLoadButton = true;
	}

	public override void OnInspectorGUI()
	{
		///Get script reference
		SetNGUILabel script = (SetNGUILabel)target;

		///Get Icons
		string currentScriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
		currentScriptPath = currentScriptPath.Substring(0, currentScriptPath.LastIndexOf("/")); //get the folder of the current script
		string icoCorrectPath = currentScriptPath + "/Correct.png";
		string icoIncorrectPath = currentScriptPath + "/Incorrect.png";
		string icoPlayMakerOnPath = currentScriptPath + "/PlayMakerOn.png";
		string icoPlayMakerOffPath = currentScriptPath + "/PlayMakerOff.png";
		Texture2D icoCorrect = AssetDatabase.LoadAssetAtPath<Texture2D>(icoCorrectPath);
		Texture2D icoIncorrect = AssetDatabase.LoadAssetAtPath<Texture2D>(icoIncorrectPath);
		Texture playMakerIconOn = AssetDatabase.LoadAssetAtPath<Texture>(icoPlayMakerOnPath);
		Texture playMakerIconOff = AssetDatabase.LoadAssetAtPath<Texture>(icoPlayMakerOffPath);

		///Custom GUIStyles
		var customTextField = new GUIStyle(GUI.skin.textField);
		customTextField.stretchWidth = true;
		var customLabel = new GUIStyle(GUI.skin.label);
		customLabel.stretchWidth = true;

		///Display grayed-out, read-only script-field
		EditorGUI.BeginDisabledGroup(true);
		serializedObject.Update();
		SerializedProperty prop = serializedObject.FindProperty("m_Script");
		EditorGUILayout.PropertyField(prop, true);
		EditorGUI.EndDisabledGroup();

		///Display UILabel Field ("Target Label")
		UILabel _targetLabel = script.uiLabelComponent;
		GUILayout.BeginHorizontal();
		if(_targetLabel == null)
		{
			GUILayout.Label("Target Label", customLabel);
			GUILayout.Label(icoIncorrect, GUILayout.Width(20));
		} else
		{
			GUILayout.Label("Target Label", customLabel);
		}
		script.uiLabelComponent = (UILabel)EditorGUILayout.ObjectField(_targetLabel, typeof(UILabel), true);
		GUILayout.EndHorizontal();

		///Fold-Out: Raw String
		script.showRawString = EditorGUILayout.Foldout(script.showRawString, "Raw String", true);
		if(script.showRawString && !ConfirmationDialog())
		{
			SwitchFolds(1);
			customTextField.focused.textColor = Color.green;
			customTextField.normal.textColor = Color.green;
			GUILayout.BeginHorizontal();
			script.rawString = (string)EditorGUILayout.TextField("String Value", script.rawString, customTextField);
			if(!string.IsNullOrEmpty(script.rawString))
			{
				if(GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(16)))
				{
					GUI.FocusControl(null);
					script.rawString = string.Empty;
				}
			}
			GUILayout.EndHorizontal();
		}

		///Fold-Out: PlayerPrefs
		script.showPlayerPrefs = EditorGUILayout.Foldout(script.showPlayerPrefs, "PlayerPrefs", true);
		if(script.showPlayerPrefs && !ConfirmationDialog())
		{
			SwitchFolds(2);
			string _targetString = script.playerPrefsKeyName;
			bool isKeyPrevalent = PlayerPrefs.HasKey(_targetString);
			string foundKeyString = PlayerPrefs.GetString(_targetString, "*null*");

			if(isKeyPrevalent)
			{
				customTextField.focused.textColor = Color.green;
				customTextField.normal.textColor = Color.green;
			} else
			{
				customTextField.focused.textColor = Color.yellow;
				customTextField.normal.textColor = Color.red;
			}

			///Display PlayerPrefsKey Text-Field
			GUILayout.BeginHorizontal();
			script.playerPrefsKeyName = (string)EditorGUILayout.TextField("PlayerPrefs Key", _targetString, customTextField);
			if(!string.IsNullOrEmpty(_targetString))
			{
				if(GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(16)))
				{
					GUI.FocusControl(null);
					script.playerPrefsKeyName = string.Empty;
				}
			}
			GUILayout.EndHorizontal();

			///Display "Is String?"
			GUILayout.BeginHorizontal();
			if(isKeyPrevalent)
			{
				GUILayout.Label("Is String?", GUILayout.Width(120));
				if(foundKeyString != "*null*")
				{
					GUILayout.Label(icoCorrect, GUILayout.Width(30));
					string endResult = foundKeyString;
					if(endResult == "")
					{
						endResult = "*empty*";
						customTextField.normal.textColor = Color.cyan;
					}
					EditorGUILayout.LabelField("Result:", GUILayout.Width(45));
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.TextField(endResult, customTextField);
					EditorGUI.EndDisabledGroup();
				} else
				{
					GUILayout.Label(icoIncorrect);
				}
			}
			GUILayout.EndHorizontal();
		}

		///Fold-Out: PlayMaker GlobalVariables
		script.showPlayMakerGlobals = EditorGUILayout.Foldout(script.showPlayMakerGlobals, "PlayMaker Globals", true);
		if(script.showPlayMakerGlobals && !ConfirmationDialog())
		{
			SwitchFolds(3);
			string _targetString = script.playmakerGlobalName;
			bool isGlobalPrevalent = FsmVariables.GlobalVariables.Contains(_targetString);
			FsmString foundFSMString = FsmVariables.GlobalVariables.FindFsmString(_targetString);

			if(isGlobalPrevalent)
			{
				customTextField.focused.textColor = Color.green;
				customTextField.normal.textColor = Color.green;
			} else
			{
				customTextField.focused.textColor = Color.yellow;
				customTextField.normal.textColor = Color.red;
			}

			///Display Global Name Text-Field
			GUILayout.BeginHorizontal();
			script.playmakerGlobalName = (string)EditorGUILayout.TextField("Global Name", _targetString, customTextField);
			if(!string.IsNullOrEmpty(_targetString))
			{
				if(GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(16)))
				{
					GUI.FocusControl(null);
					script.playmakerGlobalName = string.Empty;
				}
			}
			GUILayout.EndHorizontal();


			if(!string.IsNullOrEmpty(_targetString))
			{
				///Display "Is Global String?"
				GUILayout.BeginHorizontal();
				if(isGlobalPrevalent)
				{
					GUILayout.Label("Is Global String?", GUILayout.Width(120));
					if(foundFSMString != null)
					{
						GUILayout.Label(icoCorrect, GUILayout.Width(30));
						string endResult = foundFSMString.Value;
						if(endResult == "")
						{
							endResult = "*empty*";
							customTextField.normal.textColor = Color.cyan;
						}
						EditorGUILayout.LabelField("Result:", GUILayout.Width(45));
						EditorGUI.BeginDisabledGroup(true);
						EditorGUILayout.TextField(endResult, customTextField);
						EditorGUI.EndDisabledGroup();
					} else
					{
						GUILayout.Label(icoIncorrect);
					}
				}
				GUILayout.EndHorizontal();
			}

		} else
		{
			script.showPlayMakerGlobals = false;
		}

		///Fold-Out: GDE
		script.showGDE = EditorGUILayout.Foldout(script.showGDE, "Game Data Editor", true);
		if(script.showGDE)
		{
			SwitchFolds(4);
			string _targetItem = script.gdeItemName;
			string _targetField = script.gdeFieldName;
			string endResult = "";
			bool isItemNamePrevalent, isFieldNamePrevalent, isGDEPrevalent;
			isItemNamePrevalent = isFieldNamePrevalent = isGDEPrevalent = false;

			bool isGlobalPrevalent = FsmVariables.GlobalVariables.Contains(_targetField);
			FsmString foundFSMString = FsmVariables.GlobalVariables.FindFsmString(_targetField);

			if(isGlobalPrevalent && !script.usePlayMakerGlobalAsField)
			{
				isGlobalPrevalent = false;
			} else if(isGlobalPrevalent && script.usePlayMakerGlobalAsField && foundFSMString != null)
			{
				script.gdeFieldName = foundFSMString.Value;
			}

			var customSecondTextField = new GUIStyle(GUI.skin.textField);
			customSecondTextField.stretchWidth = true;

			///Display Load-Button
			if(showLoadButton)
			{
				if(GUILayout.Button("Load GDE (to check result)"))
				{
					GDEItemManager.Load(true);
					showLoadButton = false;
				}
			} else
			{
				if(_targetItem != "" && script.gdeFieldName != "")
				{
					try
					{
						Dictionary<string, object> data;
						if(GDEDataManager.Get(_targetItem, out data))
						{
							string val;
							data.TryGetString(script.gdeFieldName, out val);
							if(val != null)
							{
								isFieldNamePrevalent = true;
							}
							endResult = val;
						}
						endResult = GDEDataManager.GetString(_targetItem, script.gdeFieldName, endResult);
						if(endResult != "")
							isGDEPrevalent = true;
					} catch(UnityException ex)
					{
						isGDEPrevalent = false;
					}
				}
				string currentSchema = GDEItemManager.GetSchemaForItem(_targetItem);
				if(currentSchema != null && currentSchema != "")
				{
					customTextField.focused.textColor = Color.green;
					customTextField.normal.textColor = Color.green;
				} else
				{
					customTextField.focused.textColor = Color.yellow;
					customTextField.normal.textColor = Color.red;
				}

				if(endResult != "" || isGlobalPrevalent)
				{
					customSecondTextField.focused.textColor = Color.green;
					customSecondTextField.normal.textColor = Color.green;
				} else
				{
					customSecondTextField.focused.textColor = Color.yellow;
					customSecondTextField.normal.textColor = Color.red;
				}
			}

			///Display Item- & Field-Name Text-Field
			GUILayout.BeginHorizontal();
			script.gdeItemName = EditorGUILayout.TextField("Item Name", _targetItem, customTextField);
			if(!string.IsNullOrEmpty(_targetItem))
			{
				if(GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(16)))
				{
					GUI.FocusControl(null);
					script.gdeItemName = string.Empty;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			script.gdeFieldName = EditorGUILayout.TextField("Field Name", _targetField, customSecondTextField);
			if(!string.IsNullOrEmpty(_targetField))
			{
				if(GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(16)))
				{
					GUI.FocusControl(null);
					script.gdeFieldName = string.Empty;
				}
			}

			if(script.usePlayMakerGlobalAsField)
			{
				script.usePlayMakerGlobalAsField = GUILayout.Toggle(script.usePlayMakerGlobalAsField, playMakerIconOn, GUILayout.Width(34), GUILayout.Height(20));
			} else
			{
				script.usePlayMakerGlobalAsField = GUILayout.Toggle(script.usePlayMakerGlobalAsField, playMakerIconOff, GUILayout.Width(34), GUILayout.Height(20));
			}
			GUILayout.EndHorizontal();

			if(!showLoadButton)
			{
				///Display "Has Result?"
				GUILayout.BeginHorizontal();
				if(_targetItem != "" && _targetField != "")
				{
					GUILayout.Label("Has Result?", GUILayout.Width(120));
					if(endResult != "")
					{
						GUILayout.Label(icoCorrect, GUILayout.Width(30));
						EditorGUILayout.LabelField("Result:", GUILayout.Width(45));
						EditorGUI.BeginDisabledGroup(true);
						EditorGUILayout.TextField(endResult, customTextField);
						EditorGUI.EndDisabledGroup();
					} else
					{
						GUILayout.Label(icoIncorrect);
					}
				}
				GUILayout.EndHorizontal();
			}
		}
		serializedObject.ApplyModifiedProperties();
	}

	///Closes other folds and empties their content 
	public void SwitchFolds(int currentFold)
	{
		SetNGUILabel script = (SetNGUILabel)target;

		switch(currentFold)
		{
			case 1: //Raw String
				script.showPlayerPrefs = script.showPlayMakerGlobals = script.showGDE = false;
				script.playerPrefsKeyName = "";
				script.playmakerGlobalName = "";
				script.gdeItemName = "";
				script.gdeFieldName = "";
				script.usePlayMakerGlobalAsField = false;
				break;
			case 2: //PlayerPrefs
				script.showRawString = script.showPlayMakerGlobals = script.showGDE = false;
				script.rawString = "";
				script.playmakerGlobalName = "";
				script.gdeItemName = "";
				script.gdeFieldName = "";
				script.usePlayMakerGlobalAsField = false;
				break;
			case 3: //PlayMaker Globals
				script.showPlayerPrefs = script.showRawString = script.showGDE = false;
				script.rawString = "";
				script.playerPrefsKeyName = "";
				script.gdeItemName = "";
				script.gdeFieldName = "";
				script.usePlayMakerGlobalAsField = false;
				break;
			case 4: //GDE
				script.showPlayerPrefs = script.showPlayMakerGlobals = script.showRawString = false;
				script.rawString = "";
				script.playerPrefsKeyName = "";
				script.playmakerGlobalName = "";
				break;
			default:
				script.showRawString = script.showPlayerPrefs = script.showPlayMakerGlobals = script.showGDE = script.usePlayMakerGlobalAsField = false;
				break;
		}
	}

	///Displays a dialog if both GDE fields aren't empty
	public bool ConfirmationDialog()
	{
		SetNGUILabel script = (SetNGUILabel)target;

		if(script.gdeItemName != "" && script.gdeFieldName != "")
		{
			if(!EditorUtility.DisplayDialog("Really change Input-Type?", "Both GDE fields are filled. Are you sure you want to change the input type (the GDE fields will be emptied)?", "Change", "Cancel"))
			{
				return true;
			} else
			{
				return false;
			}
		}

		return false;
	}

}