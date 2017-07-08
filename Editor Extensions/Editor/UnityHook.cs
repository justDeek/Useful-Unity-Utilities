//by Olivier Fouques (@PsyKola)

using UnityEditor;
using System.Reflection;
using UnityEngine;

[InitializeOnLoad]
public class UnityHook
{
	static UnityHook()
	{
		//Replace float format string to prevent '0.0000000e-0' in Inspector fields
		typeof(EditorGUI).GetField("kFloatFieldFormatString", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, "0.#######");
		typeof(EditorGUI).GetField("kDoubleFieldFormatString", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, "0.###############");
	}
}
