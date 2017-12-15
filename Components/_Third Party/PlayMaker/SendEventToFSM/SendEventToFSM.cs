using UnityEngine;

public class SendEventToFSM : MonoBehaviour
{
	public PlayMakerFSM targetFSM;
	public string onStartEvent;
	public string onClickEvent;
	public string onPressDownEvent;
	public string onPressUpEvent;
	public string variableName;

	public void Start()
	{
		if(targetFSM == null)
			Debug.LogError("TargetFSM missing in " + this.gameObject.name);

		if(onStartEvent != null || onStartEvent != "")
		{
			SetThisGOInTargetFSM();
			targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onStartEvent.ToString());
		}
	}

	void OnClick()
	{
		if(onClickEvent != null || onClickEvent != "")
		{
			SetThisGOInTargetFSM();
			targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onClickEvent.ToString());
		}
	}

	void OnPress(bool pressed)
	{
		SetThisGOInTargetFSM();

		if(pressed)
		{
			targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onPressDownEvent.ToString());
		} else
		{
			targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onPressUpEvent.ToString());
		}
	}

	private void SetThisGOInTargetFSM()
	{
		if(variableName != null || variableName != "")
		{
			var fsmGameObject = targetFSM.FsmVariables.GetFsmGameObject(variableName);

			if(fsmGameObject != null)
			{
				fsmGameObject.Value = this.gameObject;
			}
		}
	}
}
