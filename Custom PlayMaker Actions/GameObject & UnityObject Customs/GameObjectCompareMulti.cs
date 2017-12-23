// License: Attribution 4.0 International (CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Author : Deek

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=15458.0")]
	[Tooltip("Compares one GameObject against multiple and sends events based on the result.")]
	public class GameObjectCompareMulti : FsmStateAction
	{
		[RequiredField]
		[Title("Game Object")]
		[Tooltip("A Game Object variable to compare.")]
		public FsmGameObject gameObjectVariable;

		[CompoundArray("Amount", "CompareTo", "CompareEvent")]

		[UIHint(UIHint.FsmGameObject)]
		[Tooltip("GameObject to compare to")]
		public FsmGameObject[] compareTos;

		[Tooltip("Event to raise on match")]
		public FsmEvent[] compareEvents;

		[Tooltip("Event to raise if no matches are found")]
		public FsmEvent noMatchEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result of the check in a Bool Variable. (True if equal, false if not equal).")]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame. Useful if you're waiting for a true or false result.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObjectVariable = null;
			compareTos = null;
			compareEvents = null;
			noMatchEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGameObjectCompare();

			if(!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGameObjectCompare();
		}

		void DoGameObjectCompare()
		{

			// exit if objects are null
			if((gameObjectVariable == null) || (compareTos == null) || (compareEvents == null))
				return;

			// loop until we find a match
			int j = compareTos.Length;
			for(int i = 0; i < j; i++)
			{
				if(gameObjectVariable.Value == compareTos[i].Value)
				{
					// fire the event
					Fsm.Event(compareEvents[i]);
					storeResult.Value = true;
					return;
				}
			}

			// nothing found, so fire a No-Match-Event
			if(noMatchEvent != null)
			{
				storeResult.Value = false;
				Fsm.Event(noMatchEvent);
			}

		}

	}
}
