using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Returns the first Item with the given Value. Optionally search within given Schema and/or by Field-Name.")]
	public class GDEFindItemByValue : FsmStateAction
	{
		[ActionSection("Input")]

		[RequiredField]
		[Tooltip("The value to search.")]
		public FsmString _value;

		[Tooltip("Optionally only search in specified Field.")]
		public FsmString searchInField;

		[Tooltip("The name of the Schema to get the Items from. If empty, searches in all Schemas.")]
		public FsmString searchInSchema;

		[ActionSection("Output")]

		[UIHint(UIHint.Variable)]
		[Tooltip("If it found the/an Item with that value.")]
		public FsmBool hasFound;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a String for debugging or further use.")]
		public FsmString storeItem;

		private string currentSchema;

		public override void Reset()
		{
			_value = null;
			searchInField = null;
			searchInSchema = null;
			hasFound = false;
			storeItem = null;
		}

		public override void OnEnter()
		{
			//reset values if the same action gets revisited
			hasFound.Value = false;
			storeItem.Value = null;

			GoThroughSchemas();

			//inform user if value couldn't be found
			if (!hasFound.Value)
			{
				UnityEngine.Debug.Log("GDE: \"" + _value.Value + "\" couldn't be found.");
			}

			Finish();
		}

		private void GoThroughSchemas()
		{
			foreach (KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
			{
				if (pair.Key.StartsWith(GDMConstants.SchemaPrefix))
					continue;

				//get all values of current Item
				Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;

				//skip if schema not specified
				if (!string.IsNullOrEmpty(searchInSchema.Value))
				{
					//get Schema of current Item
					currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
					//check if current Schema equals specified one
					if (currentSchema != searchInSchema.Value)
					{
						continue;
					}
				}

				foreach (var currentData in currentDataSet)
				{
					//skip if Field Name start with _gde (_gdeType_... or _gdeSchema)
					if (currentData.Key.StartsWith("_gde"))
					{
						continue;
					}

					//skip if field name not specified
					if (!string.IsNullOrEmpty(searchInField.Value))
					{
						if (currentData.Key != searchInField.Value)
						{
							continue;
						}
					}

					if (currentData.Value.ToString() == _value.Value)
					{
						hasFound.Value = true;
						storeItem.Value = pair.Key;
						return;
					}
				}
			}
		}
	}
}

#endif
