using GameDataEditor;
using iDecay.GDE;
using iDecay.PlayMaker;
using System.Collections.Generic;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Returns multiple Field Values by given Item Name and certain Field Names.")]
	public class GDEListMultipleFieldValues : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Name of the GDEItem.")]
		public FsmString itemName;

		[RequiredField]
		[Tooltip("The names of the Fields to get their value of.")]
		public FsmString[] fieldNames;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray result;

		[Tooltip("Event to send if no Fields could be found.")]
		public FsmEvent noneFoundEvent;

		public override void Reset()
		{
			itemName = null;
			fieldNames = new FsmString[0];
			result = null;
		}

		public override void OnEnter()
		{
			result.Reset();

			foreach(var fieldName in fieldNames)
			{
				result.Add(GDEHelpers.GetFieldValue(itemName.Value, fieldName.Value));
			}

			if(result.Length == 0)
				Fsm.Event(noneFoundEvent);

			Finish();
		}
	}
}

#endif
