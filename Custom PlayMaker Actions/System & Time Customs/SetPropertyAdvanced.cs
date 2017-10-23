#if !UNITY_FLASH

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.UnityObject)]
	[ActionTarget(typeof(Component), "targetProperty")]
	[ActionTarget(typeof(GameObject), "targetProperty")]
	[Tooltip("Sets the value of any public property or field on the targeted Unity Object. E.g., Drag and drop any component attached to a Game Object to access its properties. Advanced adds a delay till execution.")]
	public class SetPropertyAdvanced : FsmStateAction
	{
		[Tooltip("Any Object(GameObject, Component, Prefab, ...) to change the values/parameters/variables off of.")]
		public FsmProperty targetProperty;

		[Tooltip("Wheter to run once or every frame.")]
		public FsmBool everyFrame;

		[ActionSection("Advanced")]

		[Tooltip("Delay till execution. Only works if not Every Frame.")]
		public FsmFloat delay;

		[Tooltip("Run the delay independent of the current game TimeScale.")]
		public FsmBool ignoreTimeScale;

		[Tooltip("Set the Event to send after the delay.")]
		public FsmEvent finishEvent;

		private Coroutine routine;

		public override void Reset()
		{
			targetProperty = new FsmProperty { setProperty = true };
			everyFrame = false;
			delay = 0f;
			ignoreTimeScale = false;
			finishEvent = null;
		}

		public override void OnEnter()
		{
			routine = StartCoroutine(DoWait());

			if(!everyFrame.Value)
			{
				Finish();
				Fsm.DelayedEvent(finishEvent, delay.Value);
			}
		}

		public override void OnUpdate()
		{
			DoSetProperty();
		}

		public void DoSetProperty()
		{
			targetProperty.SetValue();
		}

		public override void OnExit()
		{
			StopCoroutine(routine);
		}

		private IEnumerator DoWait()
		{
			if(ignoreTimeScale.Value)
			{
				yield return new WaitForSecondsRealtime(delay.Value);
			} else
			{
				yield return new WaitForSeconds(delay.Value);
			}
			DoSetProperty();
		}

#if UNITY_EDITOR
		public override string AutoName()
		{
			var name = string.IsNullOrEmpty(targetProperty.PropertyName) ? "[none]" : targetProperty.PropertyName;
			var value = ActionHelpers.GetValueLabel(targetProperty.GetVariable());
			return string.Format("Set {0} to {1}", name, value);
		}
#endif
	}
}
#endif