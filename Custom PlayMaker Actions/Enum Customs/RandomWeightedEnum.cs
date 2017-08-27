
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Enum)]
	[Tooltip("Pick a random weighted Enum picked from an array of specified Enums.")]
	public class RandomWeightedEnum : FsmStateAction
	{
		[CompoundArray("Amount", "Enum", "Weighting")]
		public FsmEnum[] amount;
		[HasFloatSlider(0, 1)]
		public FsmFloat[] weights;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmEnum result;

		[Tooltip("Can hit the same enum twice in a row.")]
		public FsmBool Repeat;

		private int randomIndex;
		private int lastIndex = -1;

		public override void Reset()
		{
			amount = new FsmEnum[3];
			weights = new FsmFloat[] { 1, 1, 1 };
			result = null;
			Repeat = false;
		}
		public override void OnEnter()
		{
			PickRandom();

			Finish();
		}

		void PickRandom()
		{
			if (amount.Length == 0)
			{
				return;
			}

			if (Repeat.Value)
			{
				randomIndex = ActionHelpers.GetRandomWeightedIndex(weights);
				result.Value = amount[randomIndex].Value;

			}
			else
			{
				do
				{
					randomIndex = ActionHelpers.GetRandomWeightedIndex(weights);
				} while (randomIndex == lastIndex);

				lastIndex = randomIndex;
				result.Value = amount[randomIndex].Value;
			}
		}
	}
}