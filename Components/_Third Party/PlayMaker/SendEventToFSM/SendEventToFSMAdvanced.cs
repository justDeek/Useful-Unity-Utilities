using HutongGames.PlayMaker;
using UnityEngine;
using System;

/// <summary>
/// Put this component on any GameObject with a Collider to send
/// an Event on click, on press down and / or on press up and
/// optionally change a prevalent variable in the target FSM.
/// </summary>
public class SendEventToFSMAdvanced : MonoBehaviour
{
	public enum Methods
	{
		OnAwake,
		OnStart,
		OnEnable,
		OnDisable,
		OnDestroy,
		OnMouseEnter,
		OnMouseOver,
		OnMouseExit,
		OnMouseDown,
		OnMouseDrag,
		OnMouseUp,
		OnCollisionEnter,
		OnCollisionEnter2D,
		OnCollisionStay,
		OnCollisionStay2D,
		OnCollisionExit,
		OnCollisionExit2D,
		OnTriggerEnter,
		OnTriggerEnter2D,
		OnTriggerStay,
		OnTriggerStay2D,
		OnTriggerExit,
		OnTriggerExit2D,
		OnUpdate,
		OnLateUpdate,
		OnFixedUpdate,
		OnApplicationFocus,
		OnApplicationPause,
		OnApplicationQuit,
		OnBecameVisible,
		OnBecameInvisible,
		OnConnectedToServer,
		OnDisconnectedFromServer,

	}

	public PlayMakerFSM targetFSM;
	[UnityEngine.Tooltip("Optionally specify the FSM name if the GameObject has more than one FSM attached. Leave empty to use the first one found.")]
	public string fsmName;

	public Methods chosenEvent = Methods.OnMouseDown;
	public string eventName = "";

	public string onClickEvent;
	public string onPressDownEvent;
	public string onPressUpEvent;
	//the name of the FSMVariable in the targetFSM that is supposed to be overriden
	public string variableName;

	//declare and set default enum values
	public VariableType setValue = VariableType.Unknown;
	public float onFSMFloat;
	public int onFSMInt;
	public bool onFSMBool;
	public GameObject onFSMGameObject;
	public string onFSMString;
	public Vector2 onFSMVector2;
	public Vector3 onFSMVector3;
	public Color onFSMColor = Color.white;
	public Rect onFSMRect;
	public Material onFSMMaterial;
	public Texture onFSMTexture;
	public Quaternion onFSMQuaternion;
	public UnityEngine.Object onFSMObject;
	public Array onFSMArray;
	public Enum onFSMEnum;
	public int currVarID = 0;

	#region MethodCalls
	public void Awake() { if(chosenEvent == Methods.OnAwake) SendFsmEvent(); }
	public void Start() { if(chosenEvent == Methods.OnStart) SendFsmEvent(); }
	public void OnEnable() { if(chosenEvent == Methods.OnEnable) SendFsmEvent(); }
	public void OnDisable() { if(chosenEvent == Methods.OnDisable) SendFsmEvent(); }
	public void OnDestroy() { if(chosenEvent == Methods.OnDestroy) SendFsmEvent(); }
	public void OnMouseEnter() { if(chosenEvent == Methods.OnMouseEnter) SendFsmEvent(); }
	public void OnMouseOver() { if(chosenEvent == Methods.OnMouseOver) SendFsmEvent(); }
	public void OnMouseExit() { if(chosenEvent == Methods.OnMouseExit) SendFsmEvent(); }
	public void OnMouseDrag() { if(chosenEvent == Methods.OnMouseDrag) SendFsmEvent(); }
	public void OnMouseDown() { if(chosenEvent == Methods.OnMouseDown) SendFsmEvent(); }
	public void OnMouseUp() { if(chosenEvent == Methods.OnMouseUp) SendFsmEvent(); }
	public void OnCollisionEnter(Collision coll) { if(chosenEvent == Methods.OnCollisionEnter) SendFsmEvent(); }
	public void OnCollisionEnter2D(Collision2D coll2D) { if(chosenEvent == Methods.OnCollisionEnter2D) SendFsmEvent(); }
	public void OnCollisionStay(Collision coll) { if(chosenEvent == Methods.OnCollisionStay) SendFsmEvent(); }
	public void OnCollisionStay2D(Collision2D coll2D) { if(chosenEvent == Methods.OnCollisionStay2D) SendFsmEvent(); }
	public void OnCollisionExit(Collision coll) { if(chosenEvent == Methods.OnCollisionExit) SendFsmEvent(); }
	public void OnCollisionExit2D(Collision2D coll2D) { if(chosenEvent == Methods.OnCollisionExit2D) SendFsmEvent(); }
	public void OnTriggerEnter(Collider coll) { if(chosenEvent == Methods.OnTriggerEnter) SendFsmEvent(); }
	public void OnTriggerEnter2D(Collider2D coll2D) { if(chosenEvent == Methods.OnTriggerEnter2D) SendFsmEvent(); }
	public void OnTriggerStay(Collider coll) { if(chosenEvent == Methods.OnTriggerStay) SendFsmEvent(); }
	public void OnTriggerStay2D(Collider2D coll2D) { if(chosenEvent == Methods.OnTriggerStay2D) SendFsmEvent(); }
	public void OnTriggerExit(Collider coll) { if(chosenEvent == Methods.OnTriggerExit) SendFsmEvent(); }
	public void OnTriggerExit2D(Collider2D coll2D) { if(chosenEvent == Methods.OnTriggerExit2D) SendFsmEvent(); }
	public void Update() { if(chosenEvent == Methods.OnUpdate) SendFsmEvent(); }
	public void LateUpdate() { if(chosenEvent == Methods.OnLateUpdate) SendFsmEvent(); }
	public void FixedUpdate() { if(chosenEvent == Methods.OnFixedUpdate) SendFsmEvent(); }
	public void OnApplicationFocus(bool hasFocus) { if(chosenEvent == Methods.OnApplicationFocus) SendFsmEvent(); }
	public void OnApplicationPause(bool pauseStatus) { if(chosenEvent == Methods.OnApplicationPause) SendFsmEvent(); }
	public void OnApplicationQuit() { if(chosenEvent == Methods.OnApplicationQuit) SendFsmEvent(); }
	public void OnBecameVisible() { if(chosenEvent == Methods.OnBecameVisible) SendFsmEvent(); }
	public void OnBecameInvisible() { if(chosenEvent == Methods.OnBecameInvisible) SendFsmEvent(); }
	public void OnConnectedToServer() { if(chosenEvent == Methods.OnConnectedToServer) SendFsmEvent(); }
	public void OnDisconnectedFromServer(NetworkDisconnection info) { if(chosenEvent == Methods.OnDisconnectedFromServer) SendFsmEvent(); }
	#endregion

	//main code that gets executed on the specified method
	void SendFsmEvent()
	{
		if(targetFSM == null)
			targetFSM = GetComponent<PlayMakerFSM>();

		if(targetFSM == null)
		{
			Debug.LogError("TargetFSM missing in " + this.gameObject.name);
			return;
		}

		//if an FSM-Name was specified, use that to search for the corresponding FSM
		if(fsmName != "")
			targetFSM = PlayMakerFSM.FindFsmOnGameObject(targetFSM.gameObject, fsmName);

		SetFsmVariable();
		targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, eventName);
	}

	void SetFsmVariable()
	{
		//check wich type was set and send value accordingly
		switch(setValue)
		{
			case VariableType.Float:
				var fsmFloat = targetFSM.FsmVariables.GetFsmFloat(variableName);
				if(fsmFloat != null)
				{
					fsmFloat.Value = onFSMFloat;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Int:
				var fsmInt = targetFSM.FsmVariables.GetFsmInt(variableName);
				if(fsmInt != null)
				{
					fsmInt.Value = onFSMInt;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Bool:
				var fsmBool = targetFSM.FsmVariables.GetFsmBool(variableName);
				if(fsmBool != null)
				{
					fsmBool.Value = onFSMBool;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.GameObject:
				var fsmGameObject = targetFSM.FsmVariables.GetFsmGameObject(variableName);
				if(fsmGameObject != null)
				{
					fsmGameObject.Value = onFSMGameObject;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.String:
				var fsmString = targetFSM.FsmVariables.GetFsmString(variableName);
				if(fsmString != null)
				{
					fsmString.Value = onFSMString;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Vector2:
				var fsmVector2 = targetFSM.FsmVariables.GetFsmVector2(variableName);
				if(fsmVector2 != null)
				{
					fsmVector2.Value = onFSMVector2;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Vector3:
				var fsmVector3 = targetFSM.FsmVariables.GetFsmVector3(variableName);
				if(fsmVector3 != null)
				{
					fsmVector3.Value = onFSMVector3;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Color:
				var fsmColor = targetFSM.FsmVariables.GetFsmColor(variableName);
				if(fsmColor != null)
				{
					fsmColor.Value = onFSMColor;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Rect:
				var fsmRect = targetFSM.FsmVariables.GetFsmRect(variableName);
				if(fsmRect != null)
				{
					fsmRect.Value = onFSMRect;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Material:
				var fsmMaterial = targetFSM.FsmVariables.GetFsmMaterial(variableName);
				if(fsmMaterial != null)
				{
					fsmMaterial.Value = onFSMMaterial;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Texture:
				var fsmTexture = targetFSM.FsmVariables.GetFsmTexture(variableName);
				if(fsmTexture != null)
				{
					fsmTexture.Value = onFSMTexture;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Quaternion:
				var fsmQuaternion = targetFSM.FsmVariables.GetFsmQuaternion(variableName);
				if(fsmQuaternion != null)
				{
					fsmQuaternion.Value = onFSMQuaternion;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Object:
				var fsmObject = targetFSM.FsmVariables.GetFsmObject(variableName);
				if(fsmObject != null)
				{
					fsmObject.Value = onFSMObject;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Array:
				var fsmArray = targetFSM.FsmVariables.GetFsmArray(variableName);
				if(fsmArray != null)
				{
					//fsmArray.Values = onFSMArray;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
			case VariableType.Enum:
				var fsmEnum = targetFSM.FsmVariables.GetFsmEnum(variableName);
				if(fsmEnum != null)
				{
					//fsmEnum.Value = onFSMEnum;
				} else
				{
					Debug.LogWarning("Could not find variable: " + variableName);
				}
				break;
		}
	}
}
