// License: Attribution 4.0 International (CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Author : 'Your Playmaker username'
// supportUrl : 'url to the playmaker forum thread when available'
// Keywords: additions,words,this can,Get Searched for
// require minimum 5.3

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("<Custom Category Name>")]
	[ActionTarget(typeof(GameObject), "gameObject")]
	[Tooltip("Contains various useful examples for creating custom actions.")]
	[HelpUrl("<alternative link to forum when not using the Ecosystem tags>")]
	public class ExampleOverview : FsmStateAction
	{
		public enum ExEnum
		{
			Option1,
			Option2,
			Option3
		}

		[Tooltip("<variable description that shows up when hovering over its name>")]
		[Title("Game Object")] //gives a different, custom name for the variable displayed in the action
		[CheckForComponent(typeof(AudioSource))] // shows a warning when the GameObject doesn't contain that component
		public FsmOwnerDefault gameObject;

		//you can use a specific enum, either from the same or from a different script
		public ExEnum exampleEnum;
		//OR use FSMEnum to let the user select from all available enums
		public FsmEnum fsmEnum;

		public FsmVar variable;

		//allows the user to specify an indefinite amount of variable pairs, 
		//which are the first two variables after this attribute
		[CompoundArray("Amount", "<Name for first element>", "<Name for second element>")]

		[Tooltip("GameObject to compare to the main GameObject.")]
		public FsmGameObject[] compareTos;

		[Tooltip("Event to raise on match.")]
		public FsmEvent[] compareEvents;


		[ObjectType(typeof(AudioClip))] //restricts and shows what object type is allowed to go in that field
		public FsmObject _object;

		[HasFloatSlider(0, 1)]
		public FsmFloat slider;

		public FsmGameObject targetGameObject;

		//adds space and a label between the previous and following variable to divide your action in sections
		[ActionSection("Result")]
		//allows only to store the variable value in the action, not setting/changing it
		[UIHint(UIHint.Variable)]
		public FsmFloat result;

		public FsmEvent onFinishEvent;

		public override void Reset()
		{
			gameObject = null;
			exampleEnum = ExEnum.Option2;
			fsmEnum = null;
			variable = new FsmVar();
			compareTos = null;
			compareEvents = null;

			//"UseVariable" sets the field in the action to 'None' by default
			targetGameObject = new FsmGameObject() { UseVariable = true };
		}

		public override void OnPreprocess()
		{
			//required since PlayMaker 1.8.5 if you want to use OnLateUpdate()
			Fsm.HandleLateUpdate = true;
		}

		public override void Awake()
		{
		}

		public override void OnEnter()
		{
			//you need to call .UpdateValue() before using an FsmVar
			variable.UpdateValue();
			if(variable.gameObjectValue != null)
			{
				//"Owner" returns the GameObject this FSM is attached to
				variable.gameObjectValue = Owner;
			}

			//going through the elements of the compound array
			for(int i = 0; i < compareTos.Length; i++)
			{
				//skip if it's 'None'
				if(compareTos[i].IsNone)
				{
					continue;
				}

				//send the event with the same index as the first array
				if(compareTos[i] == targetGameObject)
				{
					Fsm.Event(compareEvents[i]);
				}
			}

			//examples of pre-defined variables that hold a specific, fsm-related value
			Fsm currentFSM = Fsm;
			FsmState currentState = State;
			string currentStateName = Name;

			//"Finished" returns wheter every action in the current state has been finished,
			//thus can be used to only send an event when every other action has finished
			if(Finished)
			{
				//if you want to send an event from your current action
				Event(onFinishEvent);
			}
			Fsm.SendEventToFsmOnGameObject(targetGameObject.Value, "FSM", onFinishEvent);
		}

		public override void OnUpdate()
		{
		}

		public override void OnLateUpdate()
		{
		}

		public override void OnFixedUpdate()
		{
		}

		//gets called when exiting the current state
		public override void OnExit()
		{
		}

		public override string ErrorCheck()
		{
			//return an error string or null if no error

			return null;
		}
	}
}
