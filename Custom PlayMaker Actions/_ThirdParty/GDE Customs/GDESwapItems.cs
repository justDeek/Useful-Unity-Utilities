using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Swaps two items by their name in the same Schema.")]
	public class GDESwapItems : FsmStateAction
	{
		[RequiredField]
		public FsmString schema;

		[RequiredField]
		[Tooltip(GDMConstants.ItemNameTooltip)]
		public FsmString itemName;

		[RequiredField]
		[Tooltip("Specify the Item Name that's supposed to swap values with the other one.")]
		public FsmString swapWith;

		[Tooltip("Wheter to save the GDE data afterwards.")]
		public FsmBool save;

		public override void Reset()
		{
			schema = null;
			itemName = null;
			swapWith = null;
			save = true;
		}

		public override void OnEnter()
		{
			GDEHelpers.SwapItems(schema.Value, itemName.Value, swapWith.Value, save.Value);
			Finish();
		}
	}
}

#endif

