using System.Collections.Generic;
using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Returns the first Item with the given Value. Optionally search within given Schema and/or by Field-Name.")]
	public class GDEFindItemsByValue : FsmStateAction
	{
		[ActionSection("Input")]

		[RequiredField]
		[Tooltip("The value to search.")]
		public FsmString _value;

		[Tooltip("Only search in specified Field. If empty, goes through all fields.")]
		public FsmString searchInField;

		[Tooltip("The name of the Schema to get the Items from. If empty, searches in all Schemas.")]
		public FsmString searchInSchema;

		[Tooltip("Only returns the first Item containing this String. If empty, searches through all Items.")]
		public FsmString itemNameContains;

		[ActionSection("Output")]

		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray result;
		
		[Tooltip("Event to send if no Item could be found.")]
		public FsmEvent noneFoundEvent;

		private string currentSchema;

		public override void Reset()
		{
			_value = null;
			searchInField = null;
			searchInSchema = null;
			itemNameContains = null;
			result = null;
			noneFoundEvent = null;
		}

		public override void OnEnter()
		{
			result.Reset();

			List<string> tmpValues = GDEHelpers.ListAllBy(GDEDataType.Item, searchInSchema.Value);
			List<string> matchingValues = new List<string>();

			foreach (var tmp in tmpValues)
			{
				if(string.IsNullOrEmpty(itemNameContains.Value) && !itemNameContains.IsNone && !tmp.Contains(itemNameContains.Value)) continue;
				
				List<string> fieldNames = string.IsNullOrEmpty(searchInField.Value) || searchInField.IsNone ? null : new List<string> {searchInField.Value};
				List<object> fieldValues = GDEHelpers.GetFieldValues(tmp, fieldNames);

				foreach (var fieldValue in fieldValues)
				{
					if (fieldValue.ToString() == _value.Value)
					{
						matchingValues.Add(tmp);
						break;
					}
				}
			}

			if(matchingValues.Count == 0)
				Fsm.Event(noneFoundEvent);
			else
				result.SetArrayContents(matchingValues);

			Finish();
		}
	}
}

#endif
