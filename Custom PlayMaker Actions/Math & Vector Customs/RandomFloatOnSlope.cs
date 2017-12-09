using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Insert two ranges and a value on the first range to get that percentage on the second one. Useful for 2.5D games, e.g. if you want GameObjects to spawn further down the farther away they are or vice versa, thus creating an imaginary slope.")]
	public class RandomFloatOnSlope : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The min and max value of the first range.")]
		public FsmVector2 range1;

		[RequiredField]
		[Tooltip("The min and max value of the second range.")]
		public FsmVector2 range2;

		[Tooltip("Set the value on the first range manually. Gets overwritten if \"Use Random Value\" is checked.")]
		public FsmFloat valueOnFirstRange;

		[Tooltip("Wether to use a random value instead of defining one.")]
		public FsmBool useRandomValue;

		[Tooltip("The value on the second range.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;

		[Tooltip("Wheter to run on every frame.")]
		public FsmBool everyFrame;


		public override void Reset()
		{
			range1 = new FsmVector2();
			range2 = new FsmVector2();
			valueOnFirstRange = new FsmFloat() { UseVariable = true };
			useRandomValue = true;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetRandomFloatOnSlope();

			if(!everyFrame.Value)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetRandomFloatOnSlope();
		}

		void DoGetRandomFloatOnSlope()
		{
			if(useRandomValue.Value)
			{
				valueOnFirstRange.Value = Random.Range(range1.Value.x, range1.Value.y);
			}

			//get difference between both ranges
			float distance1 = range1.Value.y - range1.Value.x;
			float distance2 = range2.Value.y - range2.Value.x;

			//calculate percentage of current value from the max of the first range
			float percOfFirstValue = (valueOnFirstRange.Value * 100) / distance1;
			storeResult.Value = (distance2 / 100) * percOfFirstValue;
		}
	}
}