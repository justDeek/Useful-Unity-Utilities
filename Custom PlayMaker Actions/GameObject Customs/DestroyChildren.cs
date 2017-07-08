// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Destroys all children from the Game Object.")]
	public class DestroyChildren : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject to destroy children from.")]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			DoDestroyChildren(Fsm.GetOwnerDefaultTarget(gameObject));

			Finish();
		}

		static void DoDestroyChildren(GameObject go)
		{
			if (go != null)
			{
				go.transform.DestroyChildren ();
			}
		}
	}
}