
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Get the position of a Game Object and add an offset to that Vector. Optionally applies that offset to the GameObject.")]
	public class GetPositionAddOffset : FsmStateAction
	{
		public enum Vector3Operation
		{
			Add,
			Subtract,
			Multiply,
			Divide
		}

		[RequiredField]
		[Tooltip("The GameObject to get the position off of.")]
		public FsmOwnerDefault gameObject;

		public FsmFloat xOffset;

		public FsmFloat yOffset;

		public FsmFloat zOffset;

		public Space space;

		public Vector3Operation operation;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The starting position with offset.")]
		public FsmVector3 storeVector3Result;

		[Tooltip("If the GameObject's position should be changed with the offset. (the Value of 'Store Vector3 Result' will be the new position)")]
		public bool applyOffsetToGO;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			xOffset = null;
			yOffset = null;
			zOffset = null;
			space = Space.Self;
			operation = Vector3Operation.Add;
			storeVector3Result = null;
			applyOffsetToGO = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetPosition();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetPosition();
		}

		void DoGetPosition()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			var position = space == Space.World ? go.transform.position : go.transform.localPosition;
			var input = new Vector3(xOffset.Value, yOffset.Value, zOffset.Value);

			switch (operation)
			{
				case Vector3Operation.Add:
					storeVector3Result.Value = position + input;
					break;
				case Vector3Operation.Subtract:
					storeVector3Result.Value = position - input;
					break;
				case Vector3Operation.Multiply:
					var multResult = Vector3.zero;
					multResult.x = position.x * xOffset.Value;
					multResult.y = position.y * yOffset.Value;
					multResult.z = position.z * zOffset.Value;
					storeVector3Result.Value = multResult;
					break;
				case Vector3Operation.Divide:
					var divResult = Vector3.zero;
					divResult.x = position.x / xOffset.Value;
					divResult.y = position.y / yOffset.Value;
					divResult.z = position.z / zOffset.Value;
					storeVector3Result.Value = divResult;
					break;
		  }

			if (applyOffsetToGO == true)
			{
				if (space == Space.World)
				{
					go.transform.position = position;
				}
				else
				{
					go.transform.localPosition = position;
				}
			}
	  }
  }
}
