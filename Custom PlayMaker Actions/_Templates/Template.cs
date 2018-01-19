// License: Attribution 4.0 International (CC BY 4.0)
// Author: 

/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("<Insert action description>")]
	public class Template : FsmStateAction
	{
		[RequiredField]
		[Tooltip("<Insert variable description>")]
		public FsmOwnerDefault gameObject;

		//-- insert further required variables --

		[Tooltip("Wheter to run every frame or only once.")]
		public FsmBool everyFrame;

		private GameObject go;

		public override void Reset()
		{
			gameObject = null;
			//-- insert reset values for variables --//
			everyFrame = false;
		}

		public override void OnEnter()
		{
			go = Fsm.GetOwnerDefaultTarget(gameObject);

			if(!go)
			{
				UnityEngine.Debug.LogError("GameObject is null!");
				return;
			}

			DoTemplate();

			if(!everyFrame.Value)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoTemplate();
		}

		private void DoTemplate()
		{
			//-- Your main code goes in here --//
		}
	}
}
