
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(DebugFSM))]
public class DebugFSMInspector : Editor
{
	private static GUIStyle ToggleButtonStyleNormal = null;
	private static GUIStyle ToggleButtonStyleToggled = null;

	public override void OnInspectorGUI()
	{
		DebugFSM script = (DebugFSM)this.target;

		ToggleButtonStyleNormal = "Button";
		ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
		ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("States", script.debugStateNames ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
		{
			script.debugStateNames = true;
			script.debugVariables = false;
		}

		if(GUILayout.Button("Variables", script.debugVariables ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
		{
			script.debugStateNames = false;
			script.debugVariables = true;
		}
		GUILayout.EndHorizontal();

		if(script.debugStateNames)
		{
			script.traceBackAmount = EditorGUILayout.IntField("Trace-Back Amount", script.traceBackAmount);
		} else if(script.debugVariables)
		{
			script.startFrom = EditorGUILayout.IntField("Start From Variable #", script.startFrom);
		}

		EditorGUILayout.Separator();
		EditorGUILayout.Separator();

		base.DrawDefaultInspector();
	}
}
