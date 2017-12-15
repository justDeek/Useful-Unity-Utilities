// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

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

		public override void Reset()
		{
			gameObject = null;
			//-- insert reset values for variables --//
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

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
