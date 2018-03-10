using GameDataEditor;
using iDecay.GDE;
using System.Collections.Generic;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Returns an array of all data found by the given search-parameters.")]
	public class GDEFind : FsmStateAction
	{
		[Tooltip("What type of data to search for.")]
		public GDEDataType dataType;

		[CompoundArray("Search-Parameters", "Search Type", "String")]
		[Tooltip("")]
		public SearchType[] searchType;

		[Tooltip("What to search by the given search type.")]
		public FsmString[] searchBy;

		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray result;

		[Tooltip("Event to send if no Item could be found.")]
		public FsmEvent noneFoundEvent;

		public override void Reset()
		{
			dataType = GDEDataType.Item;
			searchType = new SearchType[0];
			searchBy = new FsmString[0];
			result = null;
		}

		public override void OnEnter()
		{
			//convert FsmString[] to string[]
			string[] strArr = new string[searchBy.Length];

			for(int i = 0; i < searchBy.Length; i++)
			{
				strArr[i] = searchBy[i].Value;
			}

			List<string> matchingValues = GDEHelpers.FindAllMatching(dataType, searchType, strArr);

			if(matchingValues.Count == 0)
				Fsm.Event(noneFoundEvent);

			result.SetArrayContents(matchingValues);

			Finish();
		}
	}
}

#endif
