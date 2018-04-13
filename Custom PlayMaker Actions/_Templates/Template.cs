//License: Attribution 4.0 International (CC BY 4.0)
//Author: 

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

		//-- insert further variables --

		[Tooltip("Wheter to repeat this action on every frame or only once.")]
		public FsmBool everyFrame;

		private GameObject go;

		public override void Reset()
		{
			gameObject = null;
			//-- insert default values for further variables --//
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoTemplate();

			if(!everyFrame.Value) Finish();
		}

		public override void OnUpdate()
		{
			DoTemplate();
		}

		private void DoTemplate()
		{
			go = Fsm.GetOwnerDefaultTarget(gameObject);

			if(!go)
			{
				UnityEngine.Debug.LogError("GameObject is null!");
				return;
			}

			//-- insert main code --//
		}
	}
}
