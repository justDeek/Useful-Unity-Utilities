using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Swaps the Field values of two Items by Schema.")]
	public class GDESwapFieldValues : FsmStateAction
	{
		[RequiredField]
		[Tooltip(GDMConstants.ItemNameTooltip)]
		public FsmString firstItemName;

		[RequiredField]
		public FsmString firstFieldName;

		[ActionSection("Swap With")]
		[RequiredField]
		[Tooltip(GDMConstants.ItemNameTooltip)]
		public FsmString secondItemName;

		[RequiredField]
		public FsmString secondFieldName;

		[Tooltip("Wheter to save the GDE data afterwards.")]
		public FsmBool save;

		public override void Reset()
		{
			firstItemName = null;
			firstFieldName = null;
			secondItemName = null;
			secondFieldName = null;
			save = true;
		}

		public override void OnEnter()
		{
			GDEHelpers.SwapFieldValues(firstItemName.Value, firstFieldName.Value,
									   secondItemName.Value, secondFieldName.Value, save.Value);
			Finish();
		}
	}
}

#endif

