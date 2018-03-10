using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Inverts a GDE bool value and optionally saves the result.")]
	public class GDEBoolFlip : FsmStateAction
	{
		[RequiredField]
		public FsmString itemName;

		[RequiredField]
		public FsmString fieldName;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the bool variable.")]
		public FsmBool storeBoolResult;

		[Tooltip("Save the GDE data after flipping the bool value.")]
		public FsmBool save;

		public override void Reset()
		{
			itemName = null;
			fieldName = null;
			storeBoolResult = null;
			save = true;
		}

		public override void OnEnter()
		{
			storeBoolResult.Value = GDEHelpers.BoolFlip(itemName.Value, fieldName.Value);

			if(save.Value) GDEHelpers.Save();
			Finish();
		}
	}
}

#endif
