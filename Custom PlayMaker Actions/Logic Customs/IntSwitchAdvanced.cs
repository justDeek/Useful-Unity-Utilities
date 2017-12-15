
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends an Event based on the value of an Integer Variable.")]
	public class IntSwitchAdvanced : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;
		[CompoundArray("Int Switches", "Compare Int", "Send Event")]
		public FsmInt[] compareTo;
		public FsmEvent[] sendEvent;

		[Tooltip("Event to raise if no matches are found")]
		public FsmEvent noMatchEvent;

		public FsmBool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			compareTo = new FsmInt[1];
			sendEvent = new FsmEvent[1];
			noMatchEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIntSwitch();

			if (!everyFrame.Value)
				Finish();
		}

		public override void OnUpdate()
		{
			DoIntSwitch();
		}

		void DoIntSwitch()
		{
			if (intVariable.IsNone)
				return;

			for (int i = 0; i < compareTo.Length; i++)
			{
				if (intVariable.Value == compareTo[i].Value)
				{
					Fsm.Event(sendEvent[i]);
					return;
				}
			}
			if (noMatchEvent != null)
			{
				Fsm.Event(noMatchEvent);
			}
		}
	}
}