using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using GameDataEditor;

[CustomEditor(typeof(SetNGUILabel))]
public class SetNGUILabelInspector : Editor {
	protected static bool showRawString, showPlayerPrefs, showPlayMakerGlobals, showGDE;
	public int currentFold;

	public void OnEnable() {
		GDEItemManager.Load(true);
	}

	public override void OnInspectorGUI() {
		SetNGUILabel script = (SetNGUILabel)this.target;
		string currentScriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
		currentScriptPath = currentScriptPath.Substring(0, currentScriptPath.LastIndexOf("/")); //get the folder of the current script
		string icoCorrectPath = currentScriptPath + "/Correct.png";
		string icoIncorrectPath = currentScriptPath + "/Incorrect.png";
		Texture2D icoCorrect = (Texture2D)AssetDatabase.LoadAssetAtPath(icoCorrectPath, typeof(Texture2D));
		Texture2D icoIncorrect = (Texture2D)AssetDatabase.LoadAssetAtPath(icoIncorrectPath, typeof(Texture2D));
//Custom GUIStyles
		var customTextField = new GUIStyle(GUI.skin.textField);
		customTextField.stretchWidth = true;
		var customLabel = new GUIStyle(GUI.skin.label);
		customLabel.stretchWidth = true;

//Display grayed-out, read-only script-field
    EditorGUI.BeginDisabledGroup(true);
    serializedObject.Update();
    SerializedProperty prop = serializedObject.FindProperty("m_Script");
    EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
    EditorGUI.EndDisabledGroup();

//Fold-Out: Raw String
		showRawString = EditorGUILayout.Foldout(showRawString, "Raw String", true);
		if (showRawString) {
			currentFold = 1;
			SwitchFolds();
			customTextField.focused.textColor = Color.green;
			customTextField.normal.textColor = Color.green;
			script.rawString = (string)EditorGUILayout.TextField("String Value", script.rawString, customTextField);
		}

//Fold-Out: PlayerPrefs
		showPlayerPrefs = EditorGUILayout.Foldout(showPlayerPrefs, "PlayerPrefs", true);
		if (showPlayerPrefs) {
			currentFold = 2;
			SwitchFolds();
			string _targetString = script.playerPrefsKeyName;
			bool isKeyPrevalent = PlayerPrefs.HasKey(_targetString);
			string foundKeyString = PlayerPrefs.GetString(_targetString, "*null*");

			if (isKeyPrevalent) {
				customTextField.focused.textColor = Color.green;
				customTextField.normal.textColor = Color.green;
			} else {
				customTextField.focused.textColor = Color.yellow;
				customTextField.normal.textColor = Color.red;
			}

			//Display PlayerPrefsKey Text-Field
			script.playerPrefsKeyName = (string)EditorGUILayout.TextField("PlayerPrefs Key", _targetString, customTextField);

			//Display "Is String?"
			GUILayout.BeginHorizontal();
			if (isKeyPrevalent) {
				GUILayout.Label("Is String?", GUILayout.Width(120));
				if (foundKeyString != "*null*") {
					GUILayout.Label(icoCorrect, GUILayout.Width(30));
					string endResult = foundKeyString;
					if (endResult == "") {
						endResult = "*empty*";
						customTextField.normal.textColor = Color.cyan;
					}
					EditorGUILayout.LabelField("Result:", GUILayout.Width(45));
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.TextField(endResult, customTextField);
					EditorGUI.EndDisabledGroup();
				} else {
					GUILayout.Label(icoIncorrect);
				}
			}
			GUILayout.EndHorizontal();
		}

//Fold-Out: PlayMaker GlobalVariables
		showPlayMakerGlobals = EditorGUILayout.Foldout(showPlayMakerGlobals, "PlayMaker Globals", true);
		if (showPlayMakerGlobals) {
			currentFold = 3;
			SwitchFolds();
			string _targetString = script.playmakerGlobalName;
			bool isGlobalPrevalent = FsmVariables.GlobalVariables.Contains(_targetString);
			FsmString foundFSMString = FsmVariables.GlobalVariables.FindFsmString(_targetString);

			if (isGlobalPrevalent) {
				customTextField.focused.textColor = Color.green;
				customTextField.normal.textColor = Color.green;
			} else {
				customTextField.focused.textColor = Color.yellow;
				customTextField.normal.textColor = Color.red;
			}

			//Display Global Name Text-Field
			script.playmakerGlobalName = (string)EditorGUILayout.TextField("Global Name", _targetString, customTextField);

			//Display "Is the Global a String?"
			GUILayout.BeginHorizontal();
			if (isGlobalPrevalent) {
				GUILayout.Label("Is String?", GUILayout.Width(120));
				if (foundFSMString != null) {
					GUILayout.Label(icoCorrect, GUILayout.Width(30));
					string endResult = foundFSMString.Value;
					if (endResult == "") {
						endResult = "*empty*";
						customTextField.normal.textColor = Color.cyan;
					}
					EditorGUILayout.LabelField("Result:", GUILayout.Width(45));
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.TextField(endResult, customTextField);
					EditorGUI.EndDisabledGroup();
				} else {
					GUILayout.Label(icoIncorrect);
				}
			}
			GUILayout.EndHorizontal();
		}

//Fold-Out: GDE
		showGDE = EditorGUILayout.Foldout(showGDE, "Game Data Editor", true);
		if (showGDE) {
			currentFold = 4;
			SwitchFolds();
			string _targetItem = script.gdeItemName;
			string _targetField = script.gdeFieldName;
			string endResult = "";
			bool isItemNamePrevalent, isFieldNamePrevalent, isGDEPrevalent;
			isItemNamePrevalent = isFieldNamePrevalent = isGDEPrevalent = false;

			var customSecondTextField = new GUIStyle(GUI.skin.textField);
			customSecondTextField.stretchWidth = true;

			if (_targetItem != "" && _targetField != "") {
				try
				{
					Dictionary<string, object> data;
					if (GDEDataManager.Get(_targetItem, out data))
					{
						string val;
						data.TryGetString(_targetField, out val);
						if (val != null) {
							isFieldNamePrevalent = true;
						}
						endResult = val;
					}
					endResult = GDEDataManager.GetString(_targetItem, _targetField, endResult);
					if (endResult != "")
						isGDEPrevalent = true;
				}
				catch(UnityException ex)
				{
					isGDEPrevalent = false;
				}
			}
 			string currentSchema = GDEItemManager.GetSchemaForItem(_targetItem);
			if (currentSchema != null && currentSchema != "") {
				customTextField.focused.textColor = Color.green;
				customTextField.normal.textColor = Color.green;
			} else {
				customTextField.focused.textColor = Color.yellow;
				customTextField.normal.textColor = Color.red;
			}

			if (endResult != "") {
				customSecondTextField.focused.textColor = Color.green;
				customSecondTextField.normal.textColor = Color.green;
			} else {
				customSecondTextField.focused.textColor = Color.yellow;
				customSecondTextField.normal.textColor = Color.red;
			}

			//Display Item- & Field-Name Text-Field
			script.gdeItemName = (string)EditorGUILayout.TextField("Item Name", _targetItem, customTextField);
			script.gdeFieldName = (string)EditorGUILayout.TextField("Field Name", _targetField, customSecondTextField);

			//Display "Has Result?"
			GUILayout.BeginHorizontal();
			if (_targetItem != "" && _targetField != "") {
				GUILayout.Label("Has Result?", GUILayout.Width(120));
				if (endResult != "") {
					GUILayout.Label(icoCorrect, GUILayout.Width(30));
					EditorGUILayout.LabelField("Result:", GUILayout.Width(45));
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.TextField(endResult, customTextField);
					EditorGUI.EndDisabledGroup();
				} else {
					GUILayout.Label(icoIncorrect);
				}
			}
			GUILayout.EndHorizontal();
		}

//Display UILabel Field
		UILabel _targetLabel = script.uiLabelComponent;
		GUILayout.BeginHorizontal();
		if (_targetLabel == null) {
			GUILayout.Label("Target Label", customLabel);
			GUILayout.Label(icoIncorrect, GUILayout.Width(20));
		} else {
			GUILayout.Label("Target Label", customLabel);
		}
		script.uiLabelComponent = (UILabel)EditorGUILayout.ObjectField(_targetLabel, typeof(UILabel), true);
		GUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

	public void SwitchFolds() {
		SetNGUILabel script = (SetNGUILabel)this.target;

		switch (currentFold) {
			case 1: //Raw String
				showPlayerPrefs = showPlayMakerGlobals = showGDE = false;
				script.playerPrefsKeyName = "";
				script.playmakerGlobalName = "";
				script.gdeItemName = "";
				script.gdeFieldName = "";
				break;
			case 2: //PlayerPrefs
				showRawString = showPlayMakerGlobals = showGDE = false;
				script.rawString = "";
				script.playmakerGlobalName = "";
				script.gdeItemName = "";
				script.gdeFieldName = "";
				break;
			case 3: //PlayMaker Globals
				showPlayerPrefs = showRawString = showGDE = false;
				script.rawString = "";
				script.playerPrefsKeyName = "";
				script.gdeItemName = "";
				script.gdeFieldName = "";
				break;
			case 4: //GDE
				showPlayerPrefs = showPlayMakerGlobals = showRawString = false;
				script.rawString = "";
				script.playerPrefsKeyName = "";
				script.playmakerGlobalName = "";
				break;
			default:
				showRawString = showPlayerPrefs = showPlayMakerGlobals = showGDE = false;
				break;
		}
	}

}
