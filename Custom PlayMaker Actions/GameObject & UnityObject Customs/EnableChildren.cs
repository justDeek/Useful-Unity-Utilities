
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Enables/Disables all children from the specified parent.")]
	public class EnableChildren : FsmStateAction
	{
		[RequiredField]
    [Tooltip("GameObject to change the children off of.")]
		public FsmOwnerDefault parent;

		public bool enable;

		public override void Reset()
		{
			parent = null;
			enable = true;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(parent);

			if (go != null)
			{
				foreach (Transform child in go.transform)
				{
					child.gameObject.SetActive(enable);
				}
			}
			Finish();
		}

	}
}
