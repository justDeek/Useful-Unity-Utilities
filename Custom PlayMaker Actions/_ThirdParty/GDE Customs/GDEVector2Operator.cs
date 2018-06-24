﻿using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Performs an operation on the specified Field value. Optionally save afterwards.")]
	public class GDEVector2Operator : FsmStateAction
	{
		[RequiredField]
		[Tooltip(GDMConstants.ItemNameTooltip)]
		public FsmString ItemName;

		[RequiredField]
		[Tooltip(GDMConstants.FieldNameTooltip)]
		public FsmString FieldName;

		[Tooltip("What operation should be performed on the field value.")]
		public GDEOperation operation;

		[Tooltip("The Vector2 value to add to the one found at the Item- & Field-Name.")]
		public FsmVector2 value;

		[Tooltip("Wheter to save the GDE data afterwards.")]
		public FsmBool save;

		public override void Reset()
		{
			ItemName = null;
			FieldName = null;
			operation = GDEOperation.Add;
			value = UnityEngine.Vector2.one;
			save = true;
		}

		public override void OnEnter()
		{
			GDEHelpers.GDEOperator(ItemName.Value, FieldName.Value, GDEFieldType.Vector2, operation, value.Value);

			if(save.Value) GDEHelpers.Save();
			Finish();
		}
	}
}

#endif
