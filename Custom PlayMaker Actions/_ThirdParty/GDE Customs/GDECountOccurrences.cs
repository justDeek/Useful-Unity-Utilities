using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Returns the amount of all data found by the given search-parameters.")]
	public class GDECountOccurrences : FsmStateAction
	{
		[Tooltip("What type of data to search for.")]
		public GDEDataType dataType;

		[RequiredField]
		[Tooltip("Search for elements containing this String.")]
		public FsmString contains;

		[Tooltip("Only search within this Schema.")]
		public FsmString schema;

		[UIHint(UIHint.Variable)]
		public FsmInt result;

		public override void Reset()
		{
			dataType = GDEDataType.Item;
			contains = null;
			result = null;
		}

		public override void OnEnter()
		{
			result.Value = GDEHelpers.CountOccurrences(dataType, contains.Value, schema.Value);
			Finish();
		}
	}
}

#endif
