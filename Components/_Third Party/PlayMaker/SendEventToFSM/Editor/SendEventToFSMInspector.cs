using HutongGames.PlayMaker;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

///TODO: Use a drop-down list of all feasible, callable methods and fade that text-field in
///		 Get all available events from that fsm and list them in a drop down
///		 Get all variables from the target fsm, check if they are global and list those
///		 (or list all and show a warning if the selected one isn't global)
///		 Show appropriate Field depending on selected FsmVariable's type
///		 ? Have the user be able to select several methods in one action
///		 Check if LMAYAQi isn't using SendEventToFSMAdvanced, but if so, see if it got broken


#pragma warning disable CS0414 //Disable Warning: Variable was assigned but never used ...cause that's BS
[CustomEditor(typeof(SendEventToFSMAdvanced))]
public class SendEventToFSMInspector : Editor
{
	SerializedProperty fsmName = null;


	void OnEnable()
	{
		fsmName = serializedObject.FindProperty("fsmName");
	}

	public override void OnInspectorGUI()
	{
		///Get reference to script
		SendEventToFSMAdvanced script = (SendEventToFSMAdvanced)this.target;

		// --- Display grayed-out, read-only script field ---
		EditorGUI.BeginDisabledGroup(true);
		serializedObject.Update();
		SerializedProperty prop = serializedObject.FindProperty("m_Script");
		EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
		EditorGUI.EndDisabledGroup();
		// --------------------------------------------------

		///Check for any Collider Component on target
		if(!(script.gameObject.GetComponent<Collider>()) && !(script.gameObject.GetComponent<Collider2D>()))
		{
			EditorGUILayout.HelpBox("This GameObject requires at least one Collider or Collider2D!", MessageType.Warning, true);
		}

		///Display targetFSM field
		PlayMakerFSM _targetFsm = script.targetFSM;
		script.targetFSM = (PlayMakerFSM)EditorGUILayout.ObjectField("Target", _targetFsm, typeof(PlayMakerFSM), true);

		if(!script.targetFSM)
		{
			return;
		}


		// --- Start of serialized Properties ---
		EditorGUILayout.PropertyField(fsmName);

		// --- End of serialized Properties ---
		serializedObject.ApplyModifiedProperties();

		///Display event types
		script.chosenEvent = (SendEventToFSMAdvanced.Methods)EditorGUILayout.EnumPopup("Event-Type", script.chosenEvent);
		script.eventName = EditorGUILayout.TextField("Global-Event Name", script.eventName);

		NamedVariable[] allVariables = script.targetFSM.Fsm.FsmComponent.FsmVariables.GetAllNamedVariables();
		List<string> allVariableNames = new List<string>();
		foreach(var item in allVariables)
		{
			allVariableNames.Add(item.Name);
		}

		if(allVariableNames.Count == 0)
		{
			EditorGUILayout.HelpBox("No variables in the specified FSM", MessageType.Info);
			return;
		}

		///Display existing FsmVariables
		GUILayout.BeginHorizontal();
		GUILayout.Label("FsmVariable");
		script.currVarID = EditorGUILayout.Popup(script.currVarID, allVariableNames.ToArray());
		GUILayout.EndHorizontal();

		script.variableName = allVariableNames[script.currVarID];

		GUILayout.BeginHorizontal();
		GUILayout.Label("Current Value");
		EditorGUI.BeginDisabledGroup(true);
		GUILayout.TextField(script.targetFSM.FsmVariables.FindVariable(script.variableName).ToString(),
							GUILayout.MinWidth(180));
		EditorGUI.EndDisabledGroup();
		GUILayout.EndHorizontal();

		VariableType currVarType = allVariables[script.currVarID].VariableType;
		script.setValue = currVarType;

		switch(currVarType)
		{
			case VariableType.Float:
				script.onFSMFloat = EditorGUILayout.FloatField("Float Value", script.onFSMFloat);
				break;
			case VariableType.Int:
				script.onFSMInt = EditorGUILayout.IntField("Int Value", script.onFSMInt);
				break;
			case VariableType.Bool:
				script.onFSMBool = EditorGUILayout.Toggle("Bool Value", script.onFSMBool);
				break;
			case VariableType.GameObject:
				script.onFSMGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject Value", script.onFSMGameObject, typeof(GameObject), true);
				break;
			case VariableType.String:
				script.onFSMString = EditorGUILayout.TextField("String Value", script.onFSMString);
				break;
			case VariableType.Vector2:
				script.onFSMVector2 = EditorGUILayout.Vector2Field("Vector2 Value", script.onFSMVector2);
				break;
			case VariableType.Vector3:
				script.onFSMVector3 = EditorGUILayout.Vector3Field("Vector3 Value", script.onFSMVector3);
				break;
			case VariableType.Color:
				script.onFSMColor = EditorGUILayout.ColorField("Color Value", script.onFSMColor);
				break;
			case VariableType.Rect:
				script.onFSMRect = EditorGUILayout.RectField("Rect Value", script.onFSMRect);
				break;
			case VariableType.Material:
				script.onFSMMaterial = (Material)EditorGUILayout.ObjectField("Material Value", script.onFSMMaterial, typeof(Material), true);
				break;
			case VariableType.Texture:
				script.onFSMTexture = (Texture)EditorGUILayout.ObjectField("Texture Value", script.onFSMTexture, typeof(Texture), true);
				break;
			case VariableType.Quaternion:
				script.onFSMQuaternion.eulerAngles = EditorGUILayout.Vector3Field("Quaternion Value", script.onFSMQuaternion.eulerAngles);
				EditorGUILayout.HelpBox("Uses its euler angles to display it as a Vector3", MessageType.Info);
				break;
			case VariableType.Object:
				script.onFSMObject = EditorGUILayout.ObjectField("Object Value", script.onFSMObject, typeof(UnityEngine.Object), true);
				break;
			case VariableType.Array:
			case VariableType.Enum:
				EditorGUILayout.HelpBox("Not defined", MessageType.Info);
				break;
		}
	}
}
#pragma warning restore CS0414
