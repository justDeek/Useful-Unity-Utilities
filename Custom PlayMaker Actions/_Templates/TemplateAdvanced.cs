﻿//License: Attribution 4.0 International (CC BY 4.0)
//Author: 

/*--- __ECO__ __PLAYMAKER__ __ACTION__
EcoMetaStart
{
"script dependancies":[
						"Assets/PlayMaker Custom Actions/__Internal/FsmStateActionAdvanced.cs"
					  ]
}
EcoMetaEnd
---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("<Insert action description>")]
	public class TemplateAdvanced : FsmStateActionAdvanced
	{
		[RequiredField]
		[Tooltip("<Insert variable description>")]
		public FsmOwnerDefault gameObject;

		//-- insert further variables --//

		private GameObject go;

		public override void Reset()
		{
			//resets 'everyFrame' and 'updateType'
			base.Reset();

			gameObject = null;
			//-- insert default values for further variables --//
			go = null;
		}

		public override void OnEnter()
		{
			DoTemplate();

			if(!everyFrame) Finish();
		}

		public override void OnActionUpdate()
		{
			DoTemplate();
		}

		private void DoTemplate()
		{
			go = Fsm.GetOwnerDefaultTarget(gameObject);

			if(!go)
			{
				LogError("GameObject in " + Owner.name + " (" + Fsm.Name + ") is null!");
				return;
			}

			//-- insert main logic --//

		}
	}
}
