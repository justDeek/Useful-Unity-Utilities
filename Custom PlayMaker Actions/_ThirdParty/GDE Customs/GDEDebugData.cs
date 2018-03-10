using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Debug all GDE Data either from the DataDictionary, the ModifiedData or both. The more data you have, " +
			 "the longer this action will take, since it throws logs for every element in the dictionaries.")]
	public class GDEDebugData : FsmStateAction
	{
		[Tooltip("Which type of data to debug: DataDictionary contains all pre-defined data and ModifiedData ")]
		public GDEDictionary dataType;

		public override void Reset()
		{
			dataType = GDEDictionary.DataDictionary;
		}

		public override void OnEnter()
		{
			GDEHelpers.DebugDictionary(dataType);
			Finish();
		}
	}
}

#endif
