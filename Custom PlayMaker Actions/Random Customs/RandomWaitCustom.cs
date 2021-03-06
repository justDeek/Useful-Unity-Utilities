
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Time)]
	[Tooltip("Delays a State from finishing by a random time. NOTE: Other actions continue, but FINISHED can't happen before Time. Optionally saves the result.")]
	public class RandomWaitCustom : FsmStateAction
	{

		[RequiredField]
		[Tooltip("Minimum amount of time to wait.")]
		public FsmFloat min;

		[RequiredField]
		[Tooltip("Maximum amount of time to wait.")]
		public FsmFloat max;

		[Tooltip("Event to send when timer is finished.")]
		public FsmEvent finishEvent;

		[UIHintAttribute(UIHint.Variable)]
		[Tooltip("Save the result.")]
		public FsmFloat result;

		[Tooltip("Similar to 'Every Frame'. If set, get the current time untill state change (OnUpdate); otherwise returns the maximum time chosen (OnEnter).")]
		public FsmBool activeResult;

		[Tooltip("Ignore time scale.")]
		public bool realTime;


		private float startTime;
		private float timer;
		private float time;

		public override void Reset()
		{
			min = 0f;
			max = 1f;
			finishEvent = null;
			result = null;
			activeResult = false;
			realTime = false;
		}

		public override void OnEnter()
		{
			time = Random.Range(min.Value, max.Value);
			if (!activeResult.Value)
			{
				result.Value = time;
			}


			if (time <= 0)
			{
				Fsm.Event(finishEvent);
				Finish();
				return;
			}

			startTime = FsmTime.RealtimeSinceStartup;
			timer = 0f;
		}

		public override void OnUpdate()
		{
			// update time

			if (realTime)
			{
				timer = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				timer += Time.deltaTime;
			}

			if (activeResult.Value)
			{
				result.Value = timer;
			}

			if (timer >= time)
			{
				Finish();
				if (finishEvent != null)
				{
					Fsm.Event(finishEvent);
				}
			}
		}

	}
}
