using GameDataEditor;
using iDecay.GDE;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Sends an event if the value of a specific field changed. Especially helpful for debugging.")]
	public class GDEHasFieldValueChanged : FsmStateActionAdvanced
	{
		public FsmString itemName;
		public FsmString fieldName;

		[ActionSection("Result")]
		[Tooltip("Wheter the field value changed.")]
		public FsmEvent hasChanged;

		[Tooltip("Wheter the field value didn't change. Leave empty to stay in this state.")]
		public FsmEvent hasNotChanged;

		[Tooltip("Debug the previous and current value by throwing their names into the log.")]
		public FsmBool debugValues;

		object previousValue;

		public override void Reset()
		{
			itemName = null;
			hasChanged = null;
			hasNotChanged = null;
			everyFrame = true;
			updateType = FrameUpdateSelector.OnUpdate;
			debugValues = false;
		}

		public override void OnEnter()
		{
			if(!everyFrame) Finish();

			previousValue = GDEHelpers.GetFieldValue(itemName.Value, fieldName.Value);
		}

		public override void OnActionUpdate()
		{
			DoCompareToPrevValue();
		}

		void DoCompareToPrevValue()
		{
			object currentValue = GDEHelpers.GetFieldValue(itemName.Value, fieldName.Value);

			if(previousValue != currentValue)
			{
				if(debugValues.Value)
				{
					Log("Previous Value: " + previousValue.ToString());
					Log("Current Value: " + currentValue.ToString());
				}
				Fsm.Event(hasChanged);
				previousValue = currentValue;
			} else
				Fsm.Event(hasNotChanged);
		}
	}
}

#endif
