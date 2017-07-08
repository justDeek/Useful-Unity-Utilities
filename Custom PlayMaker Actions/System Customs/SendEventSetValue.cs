// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
  [ActionTarget(typeof(PlayMakerFSM), "eventTarget")]
  [ActionTarget(typeof(GameObject), "eventTarget")]
	[Tooltip("Sets a value of the specified type in another FSM, then sends an Event to it after an optional delay (combines 'Send Event' and 'Set Fsm Variable' for convenience & efficiency).")]
	public class SendEventSetValue : FsmStateAction
	{
		[Tooltip("Where to send the event.")]
		public FsmEventTarget eventTarget;

		[Tooltip("The name of the variable in the target FSM.")]
		public FsmString variableName;

		[Tooltip("The new value for the specified variable.")]
		public FsmVar setValue;

		[RequiredField]
		[Tooltip("The event to send. NOTE: Events must be marked Global to send between FSMs.")]
		public FsmEvent sendEvent;

		[HasFloatSlider(0, 10)]
		[Tooltip("Optional delay in seconds.")]
		public FsmFloat delay;

		[Tooltip("Repeat every frame. Rarely needed, but can be useful when sending events to other FSMs.")]
		public bool everyFrame;

		private PlayMakerFSM targetFsm;
		private NamedVariable targetVariable;
		private INamedVariable sourceVariable;

		private GameObject cachedGameObject;
		private string cachedFsmName;
		private string cachedVariableName;

		private DelayedEvent delayedEvent;

		public override void Reset()
		{
			eventTarget = null;
			setValue = new FsmVar();
			sendEvent = null;
			delay = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFsmVariable();

			if (delay.Value < 0.001f)
			{
				Fsm.Event(eventTarget, sendEvent);
			    if (!everyFrame)
			    {
			        Finish();
			    }
			}
			else
			{
				delayedEvent = Fsm.DelayedEvent(eventTarget, sendEvent, delay.Value);
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmVariable();

			if (!everyFrame)
			{
				if (DelayedEvent.WasSent(delayedEvent))
				{
					Finish();
				}
			}
			else
			{
        Fsm.Event(eventTarget, sendEvent);
			}
		}

		private void DoSetFsmVariable()
		{
				if (setValue.IsNone || string.IsNullOrEmpty(variableName.Value))
				{
						return;
				}

				var go = Fsm.GetOwnerDefaultTarget(eventTarget.gameObject);

				if (go == null)
				{
						return;
				}

				string fsmName = go.name;

				if (go != cachedGameObject || fsmName != cachedFsmName)
				{
						targetFsm = ActionHelpers.GetGameObjectFsm(go, fsmName);
						if (targetFsm == null)
						{
								return;
						}
						cachedGameObject = go;
						cachedFsmName = fsmName;
				}

				if (variableName.Value != cachedVariableName)
				{
						targetVariable = targetFsm.FsmVariables.FindVariable(setValue.Type, variableName.Value);
						cachedVariableName = variableName.Value;
				}

				if (targetVariable == null)
				{
						LogWarning("Missing Variable: " + variableName.Value);
						return;
				}

				setValue.ApplyValueTo(targetVariable);
		}

#if UNITY_EDITOR
		public override string AutoName()
		{
				return ("Set FSM Variable: " + ActionHelpers.GetValueLabel(variableName));
		}
#endif
	}
}
