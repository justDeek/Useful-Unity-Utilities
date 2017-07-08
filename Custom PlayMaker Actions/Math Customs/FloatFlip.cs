
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Inverts the sign of a float variable. If the given float is positive, it gets flipped and becomes negative and vice versa.")]
	public class FloatFlip : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
    [Tooltip("The float variable to divide.")]
		public FsmFloat floatVariable;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The flipped result.")]
    public FsmFloat result;

    [Tooltip("Repeat every Frame. Useful if the variables are changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			result = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFlip();

			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFlip();
		}

		void DoFlip()
		{
			result.Value = floatVariable.Value * (-1);
		}
	}
}
