
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Inverts the sign of an int variable. If the given int is positive, it gets flipped and becomes negative and vice versa.")]
	public class IntFlip : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
    [Tooltip("The int variable to divide.")]
		public FsmInt intVariable;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The flipped result.")]
    public FsmInt result;

    [Tooltip("Repeat every Frame. Useful if the variables are changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
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
			result.Value = intVariable.Value * (-1);
		}
	}
}
