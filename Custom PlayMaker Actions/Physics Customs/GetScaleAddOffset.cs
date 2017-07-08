
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Get the scale of a Game Object and add an offset to that Vector. Optionally applies that offset to the GameObject.")]
	public class GetScaleAddOffset : FsmStateAction
	{
		public enum Vector3Operation
		{
			Add,
			Subtract,
			Multiply,
			Divide
		}

		[RequiredField]
		[Tooltip("The GameObject to get the scale off of.")]
		public FsmOwnerDefault gameObject;

		public FsmFloat xOffset;

		public FsmFloat yOffset;

		public FsmFloat zOffset;

		public Vector3Operation operation;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The starting scale with Offset.")]
		public FsmVector3 storeVector3Result;

		[Tooltip("If the GameObject's scale should be changed with the offset. (the Value of 'Store Vector3 Result' will be the new scale)")]
		public bool applyOffsetToGO;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			xOffset = null;
			yOffset = null;
			zOffset = null;
			operation = Vector3Operation.Add;
			storeVector3Result = null;
			applyOffsetToGO = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetScale();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetScale();
		}

		void DoGetScale()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			var scale = go.transform.localScale;
			var input = new Vector3(xOffset.Value, yOffset.Value, zOffset.Value);

			switch (operation)
			{
				case Vector3Operation.Add:
					storeVector3Result.Value = scale + input;
					break;
				case Vector3Operation.Subtract:
					storeVector3Result.Value = scale - input;
					break;
				case Vector3Operation.Multiply:
					var multResult = Vector3.zero;
					multResult.x = scale.x * xOffset.Value;
					multResult.y = scale.y * yOffset.Value;
					multResult.z = scale.z * zOffset.Value;
					storeVector3Result.Value = multResult;
					break;
				case Vector3Operation.Divide:
					var divResult = Vector3.zero;
					divResult.x = scale.x / xOffset.Value;
					divResult.y = scale.y / yOffset.Value;
					divResult.z = scale.z / zOffset.Value;
					storeVector3Result.Value = divResult;
					break;

		  }

			if (applyOffsetToGO == true)
			{
					go.transform.localScale = scale;
			}
	  }
  }
}
