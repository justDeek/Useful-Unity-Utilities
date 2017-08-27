using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Enum)]
	[Tooltip("Get a Random item from an Array.")]
	public class EnumGetRandom : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The Enum Variable to get a random Item from.")]
		public FsmEnum enumVariable;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			enumVariable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetRandomValue();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetRandomValue();
		}

		private void DoGetRandomValue()
		{
			List<object> allEnumItems = new List<object>();

			//get Type
			var enumType = enumVariable.Value.GetType();

			foreach (var singleItem in Enum.GetValues(enumType))
			{
				allEnumItems.Add(singleItem);
			}

			enumVariable.Value = (Enum)allEnumItems[UnityEngine.Random.Range(0, allEnumItems.Count)];
		}
	}
}

