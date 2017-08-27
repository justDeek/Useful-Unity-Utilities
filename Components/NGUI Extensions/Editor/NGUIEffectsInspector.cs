using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(NGUIEffects), true)]
public class NGUIEffectsInspector : UISpriteInspector //UIWidgetInspector
{
	// UISprite mSprite;

	void OnDrawGizmos()
	{
		NGUIEffects script = (NGUIEffects)this.target;
		GameObject go = (GameObject)script.gameObject;
		Behaviour be = (Behaviour)script;
		SerializedProperty effectSource = serializedObject.FindProperty("mEffectSource");
		SerializedProperty effectDistance = serializedObject.FindProperty("mEffectDistance");
		SerializedProperty drawGizmos = serializedObject.FindProperty("drawGizmos");
		if (drawGizmos.boolValue && NGUITools.GetActive(be) && UnityEditor.Selection.activeGameObject == go)
		{
			//Draw Shadow3D Gizmos
			if (script.effectStyle == NGUIEffects.Effect.Shadow3D)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawIcon(new Vector3(effectSource.vector3Value.x, effectSource.vector3Value.y, go.transform.position.z - (effectDistance.vector3Value.z / 100)), "NGUIShadow3DSource", true);
			}
		}
	}

	protected override void DrawCustomProperties()
	{
		SerializedProperty sp = serializedObject.FindProperty("mSpriteName");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Effect", GUILayout.Width(76f));
		sp = NGUIEditorTools.DrawProperty("", serializedObject, "mEffectStyle", GUILayout.MinWidth(16f));

		EditorGUI.BeginDisabledGroup(!sp.hasMultipleDifferentValues && !sp.boolValue);
		{
			NGUIEditorTools.DrawProperty("", serializedObject, "mEffectColor", GUILayout.MinWidth(10f));
			GUILayout.EndHorizontal();
			//Offset
			GUILayout.BeginHorizontal();
			{
				if (sp.enumValueIndex != 0)
				{ //if Effect is not 'None'
					GUILayout.Label(" ", GUILayout.Width(5f));
					GUILayout.Label("Offset", GUILayout.Width(50f));
					NGUIEditorTools.SetLabelWidth(20f);
					NGUIEditorTools.DrawProperty("X", serializedObject, "mEffectDistance.x", GUILayout.MinWidth(20f));
					NGUIEditorTools.DrawProperty("Y", serializedObject, "mEffectDistance.y", GUILayout.MinWidth(20f));
					NGUIEditorTools.DrawProperty("Z", serializedObject, "mEffectDistance.z", GUILayout.MinWidth(20f));
					NGUIEditorTools.DrawPadding();
					NGUIEditorTools.SetLabelWidth(60f);
					if (GUILayout.Button("X", GUILayout.Width(20f), GUILayout.Height(14f)))
					{
						SerializedProperty onEffectDistance = serializedObject.FindProperty("mEffectDistance");
						onEffectDistance.vector3Value = Vector3.zero;
					}
				}
			}
			GUILayout.EndHorizontal();
			//Source
			GUILayout.BeginHorizontal();
			{
				if (sp.enumValueIndex == 2)
				{ //if Effect is 'Shadow3D'
					GUILayout.Label(" ", GUILayout.Width(5f));
					GUILayout.Label("Source", GUILayout.Width(50f));
					NGUIEditorTools.SetLabelWidth(20f);
					NGUIEditorTools.DrawProperty("X", serializedObject, "mEffectSource.x", GUILayout.MinWidth(40f));
					NGUIEditorTools.DrawProperty("Y", serializedObject, "mEffectSource.y", GUILayout.MinWidth(40f));
					NGUIEditorTools.DrawPadding();
					NGUIEditorTools.SetLabelWidth(60f);
					if (GUILayout.Button("X", GUILayout.Width(20f), GUILayout.Height(14f)))
					{
						SerializedProperty onEffectSource = serializedObject.FindProperty("mEffectSource");
						onEffectSource.vector2Value = Vector2.zero;
					}
				}
			}
			GUILayout.EndHorizontal();
			//Scale
			GUILayout.BeginHorizontal();
			{
				if (sp.enumValueIndex != 0)
				{
					GUILayout.Label(" ", GUILayout.Width(5f));
					GUILayout.Label("Scale", GUILayout.Width(50f));
					NGUIEditorTools.SetLabelWidth(20f);
					NGUIEditorTools.DrawProperty("X", serializedObject, "mEffectScale.x", GUILayout.MinWidth(40f));
					NGUIEditorTools.DrawProperty("Y", serializedObject, "mEffectScale.y", GUILayout.MinWidth(40f));
					NGUIEditorTools.DrawPadding();
					NGUIEditorTools.SetLabelWidth(60f);
					if (GUILayout.Button("X", GUILayout.Width(20f), GUILayout.Height(14f)))
					{
						SerializedProperty onEffectScale = serializedObject.FindProperty("mEffectScale");
						onEffectScale.vector2Value = Vector2.one;
					}
				}
			}
		}
		EditorGUI.EndDisabledGroup();
		GUILayout.EndHorizontal();
		EditorGUI.EndDisabledGroup();
		// serializedObject.Update();
		// serializedObject.ApplyModifiedProperties();
		// NGUITools.SetDirty(serializedObject.targetObject);
		// if (mSprite != null)
		// {
		// 	NGUIEditorTools.RegisterUndo("Atlas Selection", mSprite);
		// 	mSprite.MakePixelPerfect();
		// 	EditorUtility.SetDirty(mSprite.gameObject);
		// }
		// NGUIEditorTools.RepaintSprites();
		base.DrawCustomProperties();
	}
}
