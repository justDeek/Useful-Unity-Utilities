using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Performs an operation on the specified Field value. Optionally save afterwards.")]
	public class GDEIntOperator : GDEActionBase
	{
		[Tooltip("What operation should be performed on the field value.")]
		public GDEOperation operation;

		[Tooltip("The int value to add to the one found at the Item- & Field-Name.")]
		public FsmInt value;

		[Tooltip("Wheter to save the GDE data afterwards.")]
		public FsmBool save;

		public override void Reset()
		{
			base.Reset();

			operation = GDEOperation.Add;
			value = 1;
			save = true;
		}

		public override void OnEnter()
		{
			GDEHelpers.GDEOperator(ItemName.Value, FieldName.Value, GDEFieldType.Int, operation, value.Value);

			if(save.Value) GDEHelpers.Save();
			Finish();
		}
	}
}

#endif
