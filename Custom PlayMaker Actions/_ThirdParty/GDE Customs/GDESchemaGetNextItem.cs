using UnityEngine;
using iDecay.GDE;
using GameDataEditor;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Each time this action is called it gets the next Item from GDE by Schema.")]
	public class GDESchemaGetNextItem : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The name of the Schema to get each next Item from.")]
		public FsmString schema;

		[Tooltip("Set to true to force iterating from the first item. This variable will be set to false as it carries on iterating, force it back to true if you want to renter this action back to the first item.")]
		public FsmBool reset;

		[Tooltip("From where to start iteration, leave to 0 to start from the beginning")]
		public FsmInt startIndex;

		[Tooltip("When to end iteration, leave to 0 to iterate until the end")]
		public FsmInt endIndex;

		[Tooltip("Event to send to get the next item.")]
		public FsmEvent loopEvent;

		[Tooltip("Event to send when there are no more items.")]
		public FsmEvent finishedEvent;

		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;

		[ActionSection("Result")]

		[UIHint(UIHint.Variable)]
		[Tooltip("The current index.")]
		public FsmInt currentIndex;

		[UIHint(UIHint.Variable)]
		[VariableType(VariableType.String)]
		[Tooltip("The value for the current index.")]
		public FsmVar itemName;

		private int nextItemIndex = 0;
		private List<object> gdeData = new List<object>();

		public override void Reset()
		{

			schema = null;
			startIndex = null;
			endIndex = null;
			reset = new FsmBool() { UseVariable = true };
			loopEvent = null;
			finishedEvent = null;
			failureEvent = null;
			itemName = null;
			currentIndex = null;
		}

		public override void OnEnter()
		{
			if(reset.Value)
			{
				reset.Value = false;
				nextItemIndex = 0;
			}

			gdeData = GDEHelpers.GDEGetAllDataBy(GDEDataType.Item, schema.Value);

			if(nextItemIndex == 0)
			{
				if(gdeData.Count == 0)
				{
					Fsm.Event(failureEvent);
					Finish();
				}

				if(startIndex.Value > 0) nextItemIndex = startIndex.Value;
			}

			DoGetNextItem();
			Finish();
		}

		void DoGetNextItem()
		{
			if(nextItemIndex >= gdeData.Count)
			{
				nextItemIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}

			GetItemAtIndex();

			if(nextItemIndex >= gdeData.Count)
			{
				nextItemIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}

			if(endIndex.Value > 0 && nextItemIndex >= endIndex.Value)
			{
				nextItemIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}

			nextItemIndex++;

			if(loopEvent != null) Fsm.Event(loopEvent);
		}


		public void GetItemAtIndex()
		{
			if(itemName.IsNone) return;

			object element = null;
			currentIndex.Value = nextItemIndex;

			try
			{
				element = (string)gdeData[nextItemIndex];
			} catch(System.Exception e)
			{
				Debug.LogError(e.Message);
				Fsm.Event(failureEvent);
				return;
			}

			PlayMakerUtils.ApplyValueToFsmVar(Fsm, itemName, element);
		}
	}
}