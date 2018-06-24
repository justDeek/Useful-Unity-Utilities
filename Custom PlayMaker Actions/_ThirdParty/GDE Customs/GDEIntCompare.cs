using UnityEngine;
using iDecay.GDE;
using GameDataEditor;


#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Compares a GDE int to another int value.")]
	public class GDEIntCompare : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The name of the Item.")]
		public FsmString ItemName;

		[RequiredField]
		[Tooltip("The Field Name.")]
		public FsmString FieldName;

		[Tooltip("The int value to be compared against the GDE variable.")]
		public FsmInt compareTo;

		[RequiredField]
		[Tooltip("Tolerance for the Equal test (almost equal).\nNOTE: Ints that look the same are often not exactly the same, so you often need to use a small tolerance.")]
		public FsmInt tolerance;

		[Tooltip("Event sent if Int 1 equals Int 2 (within Tolerance)")]
		public FsmEvent equal;

		[Tooltip("Event sent if Int 1 is less than Int 2")]
		public FsmEvent lessThan;

		[Tooltip("Event sent if Int 1 is greater than Int 2")]
		public FsmEvent greaterThan;

		[Tooltip("Repeat every frame. Useful if the variables are changing and you're waiting for a particular result.")]
		public bool everyFrame;

		public override void Reset()
		{
			ItemName = null;
			FieldName = null;
			compareTo = 0;
			tolerance = 0;
			equal = null;
			lessThan = null;
			greaterThan = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoCompare();

			if(!everyFrame) Finish();
		}

		public override void OnUpdate()
		{
			DoCompare();
		}

		void DoCompare()
		{
			int result = (int)GDEHelpers.GetFieldValue(ItemName.Value, FieldName.Value);

			if(Mathf.Abs(result - compareTo.Value) <= tolerance.Value)
				Fsm.Event(equal);
			else if(result < compareTo.Value)
				Fsm.Event(lessThan);
			else if(result > compareTo.Value)
				Fsm.Event(greaterThan);
		}

		public override string ErrorCheck()
		{
			if(FsmEvent.IsNullOrEmpty(equal) &&
				FsmEvent.IsNullOrEmpty(lessThan) &&
				FsmEvent.IsNullOrEmpty(greaterThan))
				return "Action sends no events!";
			return "";
		}
	}
}

#endif
