using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Concatenates the specified String with the prevalent one. Optionally choose to add it to the end or beginning and save afterwards.")]
	public class GDEAddString : GDEActionBase
	{
		[Tooltip("The String value to add to the one found at the Item- & Field-Name.")]
		public FsmString stringToAdd;
		[Tooltip("Wheter to add the string to the end (true) or beginning (false).")]
		public FsmBool addToEnd;

		[Tooltip("Wheter to save the GDE data afterwards.")]
		public FsmBool save;

		private FsmString prevalentString;

		public override void Reset()
		{
			base.Reset();

			stringToAdd = "";
			addToEnd = true;
			save = true;
			prevalentString = null;
		}

		public override void OnEnter()
		{
			GDEHelpers.AddString(ItemName.Value, FieldName.Value, stringToAdd.Value, addToEnd.Value, save.Value);
			Finish();
		}
	}
}

#endif
