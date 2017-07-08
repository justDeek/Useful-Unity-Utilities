using UnityEngine;

public class SendEventToFSM : MonoBehaviour
{
    public PlayMakerFSM targetFSM;
    public string onClickEvent;
    public string onPressDownEvent;
    public string onPressUpEvent;
		public string variableName;

    private GameObject currentGO;

    void OnClick()
    {
      if (onClickEvent != null || onClickEvent != "")
      {
        var fsmGameObject = targetFSM.FsmVariables.GetFsmGameObject(variableName);
        if (fsmGameObject != null)
        {
          fsmGameObject.Value = this.gameObject;
        }
        targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onClickEvent.ToString ());
      }

    }

    void OnPress(bool pressed)
  	{
      var fsmGameObject = targetFSM.FsmVariables.GetFsmGameObject(variableName);
      if (fsmGameObject != null)
      {
        fsmGameObject.Value = this.gameObject;
      }

  		if (pressed)
  		{
        targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onPressDownEvent.ToString ());
  		}else{
        targetFSM.Fsm.Event(targetFSM.Fsm.EventTarget, onPressUpEvent.ToString ());
  		}
  	}

}
