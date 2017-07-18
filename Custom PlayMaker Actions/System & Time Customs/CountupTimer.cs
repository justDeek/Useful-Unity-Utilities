
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Time)]
	[Tooltip("Counts up as long as in current state and ends when leaving the current state or when the bool 'Stop' is set to true.")]
	public class CountupTimer : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Time since state start.")]
		public FsmFloat storeTime;
		public FsmBool realTime;
		[Tooltip("Stops the timer when true.")]
		public FsmBool stop;

		private float startTime;
		private float timer;

		public override void Reset()
		{
			storeTime = 0f;
			realTime = false;
			stop = false;
		}

		public override void OnEnter() {
			startTime = FsmTime.RealtimeSinceStartup;
			timer = 0f;
		}

		public override void OnUpdate()
		{
			if (!stop.Value) {
				if (realTime.Value) {
					timer = FsmTime.RealtimeSinceStartup - startTime;
				}
				else
				{
					timer += Time.deltaTime;
				}
				storeTime.Value = timer;
			}
		}
	}
}
