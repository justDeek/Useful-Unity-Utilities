using GameDataEditor;
using iDecay.GDE;
using iDecay.PlayMaker;
using System.Collections.Generic;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Only returns the Item where each of its Field Values matches the given ones. Useful for comparing (crafting-)recipes.")]
	public class GDEGetItemWithMatchingValues : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Name of the Schema that contains the Items to search for.")]
		public FsmString schema;

		[RequiredField]
		[Tooltip("The names of the Fields to get their value of.")]
		public FsmString[] fieldNames;

		[RequiredField]
		[Tooltip("The Field Values to compare.")]
		public FsmVar[] fieldValues;

		[RequiredField]
		[Tooltip("Optionally ingore any Field Value that matches any of these.")]
		public FsmVar[] ignoreValues;

		[UIHint(UIHint.Variable)]
		[Tooltip("The resulting Item, if any found.")]
		public FsmString result;

		[Tooltip("Event to send if no Fields could be found.")]
		public FsmEvent noneFoundEvent;

		public override void Reset()
		{
			schema = null;
			fieldNames = new FsmString[0];
			fieldValues = new FsmVar[0];
			ignoreValues = new FsmVar[0];
			result = null;
		}

		public override void OnEnter()
		{
			List<object> fieldValueList = new List<object>();

			result.Value = GDEHelpers.GetItemWithMatchingValues(schema.Value, fieldNames.ToList(),
												 fieldValues.ToList(), ignoreValues.ToList());

			if(string.IsNullOrEmpty(result.Value))
				Fsm.Event(noneFoundEvent);

			Finish();
		}
	}
}

#endif
