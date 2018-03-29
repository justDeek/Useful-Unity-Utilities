using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Sends Events based  on the value of a bool variable from a GDE Item.")]
	public class GDEBoolTest : GDEActionBase
	{
		[Tooltip("Event to send if the bool variable is True.")]
		public FsmEvent isTrueEvent;

		[Tooltip("Event to send if the bool variable is False.")]
		public FsmEvent isFalseEvent;

		[Tooltip("Event to send if the Item or Field couldn't have been found.")]
		public FsmEvent notFoundEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the bool variable to test.")]
		public FsmBool storeBoolResult;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			base.Reset();
			storeBoolResult = null;
			isTrueEvent = null;
			isFalseEvent = null;
			notFoundEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			CheckGDEBool();

			if(!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			CheckGDEBool();
		}

		void CheckGDEBool()
		{
			//GDEHelpers.CheckFieldType(ItemName.Value, FieldName.Value, GDEFieldType.Bool);
			storeBoolResult.Value = (bool)GDEHelpers.GetFieldValue(ItemName.Value, FieldName.Value);
			Fsm.Event(storeBoolResult.Value ? isTrueEvent : isFalseEvent);
		}
	}
}

#endif
