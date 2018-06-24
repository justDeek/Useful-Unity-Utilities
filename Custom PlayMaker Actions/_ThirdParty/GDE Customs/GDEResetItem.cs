using UnityEngine;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Resets the Fields of a GDE Item to their original value.")]
	public class GDEResetItem : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.FsmString)]
		[Tooltip(GDMConstants.ItemNameTooltip)]
		public FsmString ItemName;

		public override void Reset()
		{
			ItemName = null;
		}

		public override void OnEnter()
		{
			try
			{
				GDEDataManager.ResetToDefault(ItemName.Value);
			} catch(UnityException ex)
			{
				LogError(string.Format(GDMConstants.ErrorResettingValue, ItemName.Value));
				LogError(ex.ToString());
			} finally
			{
				Finish();
			}
		}
	}
}

#endif

