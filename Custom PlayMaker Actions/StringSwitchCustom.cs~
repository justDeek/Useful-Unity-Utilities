// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends an Event based on the value of a String Variable. Added functionality to send a No-Match-Event.")]
	public class StringSwitchCustom : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;
		[CompoundArray("String Switches", "Compare String", "Send Event")]
		public FsmString[] compareTo;
		public FsmEvent[] sendEvent;
		[HutongGames.PlayMaker.Tooltip("Event to raise if no matches are found")]
		public FsmEvent NoMatchEvent;
		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			compareTo = new FsmString[1];
			sendEvent = new FsmEvent[1];
			NoMatchEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoStringSwitch();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoStringSwitch();
		}
		
		void DoStringSwitch()
		{
//			if (stringVariable.IsNone) {
//
//			}

			
			for (int i = 0; i < compareTo.Length; i++) 
			{
				if (stringVariable.Value == compareTo[i].Value)
				{
					Fsm.Event(sendEvent[i]);
					return;
				}
					
			}
			if(NoMatchEvent != null)
			{
				Fsm.Event(NoMatchEvent);
				return;	
			}
		}
	}
}