using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

//TODO: Fade playerPrefsValue instead of toggle
#pragma warning disable CS0414 //Disable Warning: Variable was assigned but never used ...cause that's BS
[CustomEditor(typeof(SendEventToFSMAdvanced))]
public class SendEventToFSMInspector : Editor
{

  private AnimBool fadePlayerPrefsValueField;
  AnimFloat tmp;
  SerializedProperty specificFSMName = null;
  SerializedProperty onClickEvent = null;
  SerializedProperty onPressDownEvent = null;
  SerializedProperty onPressUpEvent = null;
  SerializedProperty playerPrefsKey = null;
  SerializedProperty playerPrefsValue = null;
  SerializedProperty sendValue = null;
  SerializedProperty onFSMFloat = null;
  SerializedProperty onFSMInt = null;
  SerializedProperty onFSMBool = null;
  SerializedProperty onFSMString = null;
  SerializedProperty onFSMRect = null;
  SerializedProperty onFSMColor = null;
  SerializedProperty onFSMVector2 = null;
  SerializedProperty onFSMVector3 = null;
  SerializedProperty onFSMGameObject = null;
  SerializedProperty onFSMEnum = null;
  SerializedProperty onFSMTexture = null;
  SerializedProperty onFSMMaterial = null;
  SerializedProperty onFSMArray = null;
  SerializedProperty onFSMObject = null;
  SerializedProperty onFSMQuaternion = null;

	void OnEnable()
  {
      //fadePlayerPrefsValueField = new AnimBool(true);
      //tmp = new AnimFloat(0f);
      specificFSMName   = serializedObject.FindProperty("specificFSMName");
      onClickEvent      = serializedObject.FindProperty("onClickEvent");
      onPressDownEvent  = serializedObject.FindProperty("onPressDownEvent");
      onPressUpEvent    = serializedObject.FindProperty("onPressUpEvent");
      playerPrefsKey    = serializedObject.FindProperty("playerPrefsKey");
      playerPrefsValue  = serializedObject.FindProperty("playerPrefsValue");
      sendValue         = serializedObject.FindProperty("sendValue");
      onFSMBool         = serializedObject.FindProperty("onFSMBool");
      onFSMInt          = serializedObject.FindProperty("onFSMInt");
      onFSMString       = serializedObject.FindProperty("onFSMString");
      onFSMVector2      = serializedObject.FindProperty("onFSMVector2");
      onFSMVector3      = serializedObject.FindProperty("onFSMVector3");
      onFSMColor        = serializedObject.FindProperty("onFSMColor");
      onFSMRect         = serializedObject.FindProperty("onFSMRect");
      onFSMMaterial     = serializedObject.FindProperty("onFSMMaterial");
      onFSMTexture      = serializedObject.FindProperty("onFSMTexture");
      onFSMObject       = serializedObject.FindProperty("onFSMObject");
      onFSMArray        = serializedObject.FindProperty("onFSMArray");
      onFSMEnum         = serializedObject.FindProperty("onFSMEnum");
      onFSMQuaternion   = serializedObject.FindProperty("onFSMQuaternion");
      onFSMGameObject   = serializedObject.FindProperty("onFSMGameObject");
  }

	public override void OnInspectorGUI()
  {
		SendEventToFSMAdvanced script = (SendEventToFSMAdvanced)this.target;

    //Display grayed-out, read-only script
    EditorGUI.BeginDisabledGroup(true);
    serializedObject.Update();
    SerializedProperty prop = serializedObject.FindProperty("m_Script");
    EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
    EditorGUI.EndDisabledGroup();
    serializedObject.ApplyModifiedProperties();

    //Check for any Collider Component on target
    if (!(script.gameObject.GetComponent<Collider>()) && !(script.gameObject.GetComponent<Collider2D>()))
    {
      EditorGUILayout.HelpBox("This GameObject requires at least one Collider or Collider2D!", MessageType.Warning, true);
    }

    //Display targetFSM Field
		PlayMakerFSM _targetFsm = script.targetFSM;
		script.targetFSM = (PlayMakerFSM)EditorGUILayout.ObjectField("Target", _targetFsm, typeof(PlayMakerFSM), true);

    // --- Start of serialized Properties ---
    serializedObject.Update();
    EditorGUILayout.PropertyField(specificFSMName);
    EditorGUILayout.PropertyField(onClickEvent);
    EditorGUILayout.PropertyField(onPressDownEvent);
    EditorGUILayout.PropertyField(onPressUpEvent);
    EditorGUILayout.PropertyField(playerPrefsKey);

    //fade PlayerPrefsValue Field if -PrefsKey is getting filled
    if (script.playerPrefsKey != "" && script.playerPrefsKey != null) {
      EditorGUILayout.PropertyField(playerPrefsValue);
    }
    else
    {
      script.playerPrefsValue = "";
    }

    // --- End of serialized Properties ---
    serializedObject.ApplyModifiedProperties();


		script.sendValue = (SendEventToFSMAdvanced.FSMVariableTypes)EditorGUILayout.EnumPopup("Send Variable", script.sendValue);
    //If not 'None', show variableName
    if (script.sendValue != SendEventToFSMAdvanced.FSMVariableTypes.None)
    {
      script.variableName = EditorGUILayout.TextField("Variable Name", script.variableName);
    }

    switch (script.sendValue)
    {
      case SendEventToFSMAdvanced.FSMVariableTypes.Float :
          script.onFSMFloat = EditorGUILayout.FloatField ("Float Value", script.onFSMFloat);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Int :
          script.onFSMInt = EditorGUILayout.IntField("Int Value", script.onFSMInt);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Bool :
          script.onFSMBool = EditorGUILayout.Toggle("Bool Value", script.onFSMBool);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.GameObject :
      script.onFSMGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject Value", script.onFSMGameObject, typeof(GameObject), true);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.String :
          script.onFSMString = EditorGUILayout.TextField("String Value", script.onFSMString);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Vector2 :
          script.onFSMVector2 = EditorGUILayout.Vector2Field("Vector2 Value", script.onFSMVector2);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Vector3 :
          script.onFSMVector3 = EditorGUILayout.Vector3Field("Vector3 Value", script.onFSMVector3);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Color :
          script.onFSMColor = EditorGUILayout.ColorField("Color Value", script.onFSMColor);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Rect :
          script.onFSMRect = EditorGUILayout.RectField("Rect Value", script.onFSMRect);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Material :
          script.onFSMMaterial = (Material)EditorGUILayout.ObjectField("Material Value", script.onFSMMaterial, typeof(Material), true);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Texture :
          script.onFSMTexture = (Texture)EditorGUILayout.ObjectField("Texture Value", script.onFSMTexture, typeof(Texture), true);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Quaternion :
        // script.onFSMQuaternion = EditorGUILayout.Vector4Field("Quaternion Value", script.onFSMQuaternion);
        EditorGUILayout.HelpBox("Not defined", MessageType.Info);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Object :
          script.onFSMObject = EditorGUILayout.ObjectField("Object Value", script.onFSMObject, typeof(UnityEngine.Object), true);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Array :
          // script.onFSMArray = EditorGUILayout.ObjectField("Array Value", script.onFSMArray, typeof(Array), true);
          EditorGUILayout.HelpBox("Not defined", MessageType.Info);
      break;
      case SendEventToFSMAdvanced.FSMVariableTypes.Enum :
          // script.onFSMEnum = EditorGUILayout.EnumPopup("Enum Value", script.onFSMEnum);
          EditorGUILayout.HelpBox("Not defined", MessageType.Info);
      break;
    }

	}

  public static void FixedEndFadeGroup(float aValue)
  {
    if (aValue == 0f || aValue == 1f)
    return;
    EditorGUILayout.EndFadeGroup();
  }

}
#pragma warning restore CS0414
