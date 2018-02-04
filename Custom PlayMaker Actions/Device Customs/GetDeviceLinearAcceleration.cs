
using UnityEngine;
//using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Returns the last measured linear acceleration of a device in three-dimensional space.")]
	public class GetDeviceLinearAcceleration : FsmStateAction
	{
		[Tooltip("The multiplier of the direction.")]
		public FsmFloat speed;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the direction vector.")]
		public FsmVector3 storeVector;

		[UIHint(UIHint.Variable)]
		[Tooltip("The horizontal input.")]
		public FsmFloat storeHorizontalMovement;

		[UIHint(UIHint.Variable)]
		[Tooltip("The vertical input.")]
		public FsmFloat storeVerticalMovement;

		[UIHint(UIHint.Variable)]
		[Tooltip("Returns list of acceleration measurements which occurred during the last frame.")]
		public FsmVector3 storeAccelerationEvents;

		[Tooltip("Optionally updates the Transform of set GameObject to User-Movement.")]
		public FsmGameObject moveObjectAccordingly;

		[Tooltip("Resets all Input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame.")]
		public FsmBool resetInputAxesOnStart;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[Tooltip("Ignore TimeScale.")]
		public bool realTime;

		public override void Reset()
		{
			speed = 10.0F;
			storeVector = Vector3.zero;
			storeHorizontalMovement = null;
			storeVerticalMovement = null;
			storeAccelerationEvents = null;
			moveObjectAccordingly = null;
			resetInputAxesOnStart = false;
			everyFrame = true;
			realTime = false;
		}

		public override void OnEnter()
		{
			if(resetInputAxesOnStart.Value)
				Input.ResetInputAxes();


			if(!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoGetLinearAcceleration();

		}

		void DoGetLinearAcceleration()
		{
			Vector3 dir = Vector3.zero;
			dir.x = -Input.acceleration.y;
			dir.z = Input.acceleration.x;
			if(dir.sqrMagnitude > 1)
				dir.Normalize();

			if(realTime)
				dir *= Time.deltaTime;

			if(moveObjectAccordingly.Value != null)
				moveObjectAccordingly.Value.transform.Translate(dir * speed.Value);

			storeVector.Value = dir;
			storeHorizontalMovement.Value = dir.x;
			storeVerticalMovement.Value = dir.z;

			Vector3 acceleration = Vector3.zero;
			foreach(AccelerationEvent accEvent in Input.accelerationEvents)
			{
				acceleration += accEvent.acceleration * accEvent.deltaTime;
			}
			storeAccelerationEvents.Value = acceleration;
		}
	}
}
