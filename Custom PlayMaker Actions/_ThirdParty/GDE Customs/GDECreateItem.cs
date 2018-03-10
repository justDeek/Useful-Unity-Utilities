using GameDataEditor;
using iDecay.GDE;
using System.Collections.Generic;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Create an Item in the specified Schema and optionally set it's Value.")]
	public class GDECreateItem : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Specify the existing Schema this Item should belong to.")]
		public FsmString schema = "";

		[RequiredField]
		[Tooltip("The name of the variable in the target FSM.")]
		public FsmString itemName = "";

		[CompoundArray("Key Amount", "Field Name", "Set Value")]

		[RequiredField]
		[Tooltip("The name of the variable in the target FSM.")]
		public FsmString[] fieldNames;

		[Tooltip("Optionally apply the desired value to the created Item under the specified Field-Name.")]
		public FsmVar[] values;

		[Tooltip("Should be saved afterwards? If not you can still save later, otherwise changes will be discarded when restarting the game/project.")]
		public FsmBool save;

		public override void Reset()
		{
			schema = null;
			itemName = null;
			fieldNames = new FsmString[0];
			values = new FsmVar[0];
			save = true;
		}

		public override void OnEnter()
		{
			List<string> tmpFieldNames = new List<string>();
			List<object> tmpFieldValues = new List<object>();
			//List<GDEFieldType> tmpFieldTypes = new List<GDEFieldType>();

			for(int i = 0; i < fieldNames.Length; i++)
			{
				tmpFieldNames.Add(fieldNames[i].Value);
				tmpFieldValues.Add(values[i].GetValue());
				//tmpFieldTypes.Add(GDEHelpers.GetFieldType(itemName.Value, fieldNames[i].Value));
			}

			GDEHelpers.CreateItem(schema.Value, itemName.Value, tmpFieldNames.ToArray(),
								  tmpFieldValues.ToArray(), null, save.Value);
			Finish();
		}
	}
}

#endif
