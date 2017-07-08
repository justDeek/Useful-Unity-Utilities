using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Create an Item in the specified Schema and optionally set it's Value.")]
	public class GDECreateItem : GDEActionBase
	{
		[RequiredField]
		[Tooltip("Specify the existing Schema this Item should belong to.")]
		[UIHint(UIHint.FsmString)]
		public FsmString schema;

		[Tooltip("Optionally apply the desired value to the created Item under the specified Field Name.")]
		[UIHint(UIHint.FsmString)]
		public FsmString value;

		public override void Reset()
		{
			base.Reset();
			schema = null;
			value = null;
		}

		public override void OnEnter()
		{
			try
			{
				GDEDataManager.RegisterItem(schema.Value, ItemName.Value);
				if (value.Value != null) {
					GDEDataManager.SetString(ItemName.Value, FieldName.Value, value.Value);
				}
			}
			catch(UnityException ex)
			{
				LogError(string.Format(GDMConstants.ErrorSettingValue, GDMConstants.StringType, ItemName.Value, FieldName.Value));
				LogError(ex.ToString());
			}
			finally
			{
				Finish();
			}
		}
	}
}

#endif
