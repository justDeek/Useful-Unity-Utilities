using UnityEngine;
using System;

/// <summary>
/// Put this component on any GameObject with a Collider to send
/// an Event on click, on press down and / or on press up and
/// optionally change a prevalent variable in the target FSM.
/// </summary>
public class SendEventToFSMAdvanced : MonoBehaviour
{
	public PlayMakerFSM targetFSM;
	[UnityEngine.Tooltip("Specify FSM Name if the GameObject has more than one FSM. Leave empty to use the first found.")]
	public string specificFSMName;

  public string onClickEvent;
	public string onPressDownEvent;
	public string onPressUpEvent;
	[UnityEngine.Tooltip("Optionally set a PlayerPrefs to retrieve in the Target FSM. Leave empty to skip this.")]
	public string playerPrefsKey;
	[UnityEngine.Tooltip("Leave empty to set the name of the GameObject this Script is attached to.")]
	public string playerPrefsValue;

	public enum FSMVariableTypes{
		None,
		Float,
		Int,
		Bool,
		GameObject,
		String,
		Vector2,
		Vector3,
		Color,
		Rect,
		Material,
		Texture,
		Quaternion,
		Object,
		Array,
		Enum
	}

	//declare and set default enum values
	public FSMVariableTypes sendValue = FSMVariableTypes.None;
	public float onFSMFloat;
	public int onFSMInt;
	public bool onFSMBool;
	public GameObject onFSMGameObject;
	public string onFSMString;
	public Vector2 onFSMVector2;
	public Vector3 onFSMVector3;
	public Color onFSMColor;
	public Rect onFSMRect;
	public Material onFSMMaterial;
	public Texture onFSMTexture;
	public Quaternion onFSMQuaternion;
	public UnityEngine.Object onFSMObject;
	public Array onFSMArray;
	public Enum onFSMEnum;

	//the name of the FSMVariable in the targetFSM that is supposed to be overriden
	public string variableName;

	void OnClick()
	{
		//if an FSM-Name was specified, use that to search for the corresponding FSM
		if (specificFSMName != "")
			targetFSM = PlayMakerFSM.FindFsmOnGameObject (targetFSM.gameObject, specificFSMName);

		//check if targetFSM is empty
		if (targetFSM == null)
			throw new Exception("NGUISendEventToFSM: targetFSM == null");

		//check if onClickEvent is empty
		if (string.IsNullOrEmpty(onClickEvent))
			throw new Exception("NGUISendEventToFSM: onClickEvent - IsNullOrEmpty");

		//create or set PlayerPrefs Key if not left empty
		if (!string.IsNullOrEmpty(playerPrefsKey))
		{
			if (string.IsNullOrEmpty(playerPrefsValue))
				playerPrefsValue = this.gameObject.name;

			//set the PlayerPrefs Key
			PlayerPrefs.SetString (playerPrefsKey, playerPrefsValue);
		}

		//check wich Type was set and send value accordingly
		switch (sendValue)
		{
		case FSMVariableTypes.None:

			break;
		case FSMVariableTypes.Float:
			var fsmFloat = targetFSM.FsmVariables.GetFsmFloat (variableName);
			if (fsmFloat != null)
			{
				fsmFloat.Value = onFSMFloat;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Int:
			var fsmInt = targetFSM.FsmVariables.GetFsmInt (variableName);
			if (fsmInt != null)
			{
				fsmInt.Value = onFSMInt;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Bool:
			var fsmBool = targetFSM.FsmVariables.GetFsmBool (variableName);
			if (fsmBool != null)
			{
				fsmBool.Value = onFSMBool;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.GameObject:
			var fsmGameObject = targetFSM.FsmVariables.GetFsmGameObject (variableName);
			if (fsmGameObject != null)
			{
				fsmGameObject.Value = onFSMGameObject;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.String:
			var fsmString = targetFSM.FsmVariables.GetFsmString (variableName);
			if (fsmString != null)
			{
				fsmString.Value = onFSMString;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Vector2:
			var fsmVector2 = targetFSM.FsmVariables.GetFsmVector2 (variableName);
			if (fsmVector2 != null)
			{
				fsmVector2.Value = onFSMVector2;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Vector3:
			var fsmVector3 = targetFSM.FsmVariables.GetFsmVector3 (variableName);
			if (fsmVector3 != null)
			{
				fsmVector3.Value = onFSMVector3;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Color:
			var fsmColor = targetFSM.FsmVariables.GetFsmColor (variableName);
			if (fsmColor != null)
			{
				fsmColor.Value = onFSMColor;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Rect:
			var fsmRect = targetFSM.FsmVariables.GetFsmRect (variableName);
			if (fsmRect != null)
			{
				fsmRect.Value = onFSMRect;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Material:
			var fsmMaterial = targetFSM.FsmVariables.GetFsmMaterial (variableName);
			if (fsmMaterial != null)
			{
				fsmMaterial.Value = onFSMMaterial;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Texture:
			var fsmTexture = targetFSM.FsmVariables.GetFsmTexture (variableName);
			if (fsmTexture != null)
			{
				fsmTexture.Value = onFSMTexture;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Quaternion:
			var fsmQuaternion = targetFSM.FsmVariables.GetFsmQuaternion (variableName);
			if (fsmQuaternion != null)
			{
				//fsmQuaternion.Value = onFSMQuaternion;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Object:
			var fsmObject = targetFSM.FsmVariables.GetFsmObject (variableName);
			if (fsmObject != null)
			{
				fsmObject.Value = onFSMObject;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Array:
			var fsmArray = targetFSM.FsmVariables.GetFsmArray (variableName);
			if (fsmArray != null)
			{
				//fsmArray.Values = onFSMArray;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;
		case FSMVariableTypes.Enum:
			var fsmEnum = targetFSM.FsmVariables.GetFsmEnum (variableName);
			if (fsmEnum != null)
			{
				//fsmEnum.Value = onFSMEnum;
			}
			else
			{
				Debug.LogWarning("Could not find variable: " + variableName);
			}
			break;

		}

		//send the event
		targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onClickEvent.ToString ());
  }

	void OnPress(bool pressed)
	{
		if (onPressUpEvent != null || onPressUpEvent != "")
		{
			var fsmGameObject = targetFSM.FsmVariables.GetFsmGameObject(variableName);
			if (fsmGameObject != null)
			{
				fsmGameObject.Value = this.gameObject;
			}
		}

		if (pressed)
		{
			targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onPressDownEvent.ToString ());
		}else{
			targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onPressUpEvent.ToString ());
		}
	}
}
