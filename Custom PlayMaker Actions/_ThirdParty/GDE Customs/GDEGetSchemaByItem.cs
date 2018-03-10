using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Store the Schema of the given Item Name.")]
	public class GDEGetSchemaByItem : FsmStateAction
	{
		[Tooltip("The Name of the Item in the Schema.")]
		public FsmString itemName;

		[UIHint(UIHint.Variable)]
		[Tooltip("The Name of the Schema to store.")]
		public FsmString storeSchema;

		public override void Reset()
		{
			itemName = null;
			storeSchema = null;
		}

		public override void OnEnter()
		{
			storeSchema.Value = GDEHelpers.GetSchemaByItem(itemName.Value);
		}
	}
}

#endif
