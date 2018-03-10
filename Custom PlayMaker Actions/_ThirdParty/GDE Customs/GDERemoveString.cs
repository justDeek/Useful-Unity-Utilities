using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Removes the specified String from the prevalent one. Optionally choose to add it to the end or beginning and save afterwards.")]
	public class GDERemoveString : GDEActionBase
	{
		[Tooltip("The String value to add to the one found at the Item- & Field-Name.")]
		public FsmString stringToRemove;
		[Tooltip("Wheter to add the string to the end (true) or beginning (false).")]
		public FsmBool removeFromEnd;

		[Tooltip("Wheter to save the GDE data afterwards.")]
		public FsmBool save;

		private FsmString prevalentString;

		public override void Reset()
		{
			base.Reset();

			stringToRemove = "";
			removeFromEnd = true;
			save = true;
			prevalentString = null;
		}

		public override void OnEnter()
		{
			GDEHelpers.RemoveString(ItemName.Value, FieldName.Value,
									stringToRemove.Value, removeFromEnd.Value, save.Value);
			Finish();
		}
	}
}

#endif
