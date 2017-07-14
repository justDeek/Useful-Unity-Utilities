
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Enable or disable a Collider or Collider2D on a GamObject. Optionally set all colliders found on the gameobject Target.")]
	public class EnableCollider : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject with the Colliders attached")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The flag value")]
		public FsmBool enable;

		[Tooltip("Set all Colliders on the GameObject target.")]
		public FsmBool applyToAllColliders;

		public override void Reset()
		{
			gameObject = null;
			enable = true;
			applyToAllColliders = false;
		}

		public override void OnEnter()
		{
			DoEnableCollider();

			Finish();
		}

		void DoEnableCollider()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;


			if (applyToAllColliders.Value)
			{
				// Find all of the colliders on the gameobject and set them all to be enabled.
				Collider[] cols = go.GetComponents<Collider> ();
				foreach (Collider c in cols) {
					c.enabled = enable.Value;
				}

				// Find all of the 2D colliders on the gameobject and set them all to be enabled.
				Collider2D[] cols2D = go.GetComponents<Collider2D> ();
				foreach (Collider2D c in cols2D) {
					c.enabled = enable.Value;
				}
			}else{
				if (go.GetComponent<Collider>() != null)go.GetComponent<Collider>().enabled  = enable.Value;
				if (go.GetComponent<Collider2D>() != null)go.GetComponent<Collider2D>().enabled  = enable.Value;
			}
		}
	}
}
