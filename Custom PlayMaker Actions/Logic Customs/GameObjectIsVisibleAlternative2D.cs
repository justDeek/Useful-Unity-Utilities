
using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
    [ActionTarget(typeof(GameObject), "gameObject")]
	[Tooltip("Tests if a Game Object is visible. This will return true even if just a portion of the GO is visible. Checks if the collider bounds of the GameObject are outside the camera frustum planes.")]
	public class GameObjectIsVisibleAlternative2D : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Collider2D))]
    [Tooltip("The GameObject to test.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The Camera to check the GameObject from.")]
		public FsmGameObject camera;

        [Tooltip("Event to send if the GameObject is visible.")]
		public FsmEvent trueEvent;

        [Tooltip("Event to send if the GameObject is not visible.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;

		private GameObject go = null;
		private bool result = false;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			camera = Camera.main.gameObject;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsVisible();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsVisible();
		}

		void DoIsVisible()
		{
			go = Fsm.GetOwnerDefaultTarget(gameObject);
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera.Value.GetComponent<Camera>());
			if (GeometryUtility.TestPlanesAABB(planes , go.GetComponent<Collider2D>().bounds))
			{
				result = true;
			}
			else
			{
				result = false;
			}
			storeResult.Value = result;
			Fsm.Event(result ? trueEvent : falseEvent);
		}
	}
}
