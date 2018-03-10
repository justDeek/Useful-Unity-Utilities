using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Removes an item from the runtime data.")]
	public class GDERemoveItem : FsmStateAction
	{
		[Tooltip("The Name of the Item to remove.")]
		public FsmString itemName;

		[Tooltip("Should be saved afterwards? If not you can still save later, otherwise changes will be discarded when restarting the game/project.")]
		public FsmBool save;

		[ActionSection("Result")]
		[Tooltip("Wheter removing the item was successful.")]
		public FsmEvent success;

		[Tooltip("Wheter removing the item failed.")]
		public FsmEvent failure;

		public override void Reset()
		{
			itemName = null;
			save = true;
			success = null;
			failure = null;
		}

		public override void OnEnter()
		{
			if(GDEHelpers.RemoveItem(itemName.Value, save.Value, false))
				Fsm.Event(success);
			else
				Fsm.Event(failure);

			Finish();
		}
	}
}

#endif
