using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Performs an operation on the specified Field value. Optionally save afterwards.")]
	public class GDEFloatOperator : FsmStateAction
	{
		[RequiredField]
		[Tooltip(GDMConstants.ItemNameTooltip)]
		public FsmString ItemName;

		[RequiredField]
		[Tooltip(GDMConstants.FieldNameTooltip)]
		public FsmString FieldName;

		[Tooltip("What operation should be performed on the field value.")]
		public GDEOperation operation;

		[Tooltip("The int value to add to the one found at the Item- & Field-Name.")]
		public FsmFloat value;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the new result.")]
		public FsmFloat storeResult;

		[Tooltip("Wheter to save the GDE data afterwards.")]
		public FsmBool save;

		public override void Reset()
		{
			ItemName = null;
			FieldName = null;
			operation = GDEOperation.Add;
			value = 1f;
			storeResult = null;
			save = true;
		}

		public override void OnEnter()
		{
			GDEHelpers.GDEOperator(ItemName.Value, FieldName.Value, GDEFieldType.Float, operation, value.Value);

			if(!storeResult.IsNone)
				storeResult.Value = (float)GDEHelpers.GetFieldValue(ItemName.Value, FieldName.Value);

			if(save.Value) GDEHelpers.Save();
			Finish();
		}
	}
}

#endif
