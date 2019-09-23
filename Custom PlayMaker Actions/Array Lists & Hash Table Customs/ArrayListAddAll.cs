// License: Attribution 4.0 International (CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
//Author: Deek

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Add all items from an FsmArray to a PlayMaker Array List Proxy component")]
	public class ArrayListAddAll : ArrayListActions
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The FsmArray to add to the ArrayList proxy.")]
		public FsmArray array;

		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		public FsmEvent successEvent;
		public FsmEvent failureEvent;

		public override void Reset()
		{
			array = null;
			gameObject = null;
			reference = null;
			successEvent = null;
			failureEvent = null;
		}
		
		public override void OnEnter()
		{
			if(!SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				Debug.LogWarning("Couldn't find the Array List Proxy Component!");
				Fsm.Event(failureEvent);
				Finish();
			}

			proxy.AddRange(array.Values, array.ObjectTypeName);

			Fsm.Event(successEvent);
			Finish();	
		}
	}
}