
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
			if(go != null)
			{
				go.transform.DestroyChildren();
				Transform t = go.transform;

				bool isPlaying = Application.isPlaying;

				while(t.childCount != 0)
				{
					Transform child = t.GetChild(0);

					if(isPlaying)
					{
						child.parent = null;
						UnityEngine.Object.Destroy(child.gameObject);
					} else
						UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
			}
		}
	}
}
