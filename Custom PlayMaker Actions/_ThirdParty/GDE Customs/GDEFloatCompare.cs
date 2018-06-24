using UnityEngine;
using iDecay.GDE;
using GameDataEditor;


#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Compares a GDE float to another float value.")]
	public class GDEFloatCompare : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The name of the Item.")]
		public FsmString ItemName;

		[RequiredField]
		[Tooltip("The Field Name.")]
		public FsmString FieldName;

		[Tooltip("The float value to be compared against the GDE variable.")]
		public FsmFloat compareTo;

		[RequiredField]
		[Tooltip("Tolerance for the Equal test (almost equal).\nNOTE: Floats that look the same are often not exactly the same, so you often need to use a small tolerance.")]
		public FsmFloat tolerance;

		[Tooltip("Event sent if Float 1 equals Float 2 (within Tolerance)")]
		public FsmEvent equal;

		[Tooltip("Event sent if Float 1 is less than Float 2")]
		public FsmEvent lessThan;

		[Tooltip("Event sent if Float 1 is greater than Float 2")]
		public FsmEvent greaterThan;

		[Tooltip("Repeat every frame. Useful if the variables are changing and you're waiting for a particular result.")]
		public bool everyFrame;

		public override void Reset()
		{
			ItemName = null;
			FieldName = null;
			compareTo = 0f;
			tolerance = 0f;
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
			float result = (float)GDEHelpers.GetFieldValue(ItemName.Value, FieldName.Value);

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
