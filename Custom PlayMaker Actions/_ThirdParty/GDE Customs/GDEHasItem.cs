using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Checks if the specified Item is prevalent. Optionally search within given Schema.")]
	public class GDEHasItem : FsmStateAction
	{
		[ActionSection("Input")]

		[RequiredField]
		[Tooltip("The Name of the Item.")]
		public FsmString itemName;

		[Tooltip("The Field-Name.")]
		public FsmString fieldName;

		[Tooltip("The name of the Schema to get the Items from. If empty, searches in all Schemas.")]
		public FsmString searchInSchema;

		[ActionSection("Output")]

		[UIHint(UIHint.Variable)]
		[Tooltip("Wheter the set Item is in any Schema.")]
		public FsmBool hasItem;

		[UIHint(UIHint.Variable)]
		[Tooltip("Wheter both Item-Name and Field-Name contain a value.")]
		public FsmBool hasResult;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a String for debugging or further use.")]
		public FsmString storeResult;

		private string result;

		public override void Reset()
		{
			itemName = null;
			fieldName = null;
			searchInSchema = null;
			hasItem = false;
			hasResult = false;
			storeResult = null;
		}

		public override void OnEnter()
		{
			List<string> allItems = new List<string>();
			string currentSchema = "";
			foreach (KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
			{
				if (pair.Key.StartsWith(GDMConstants.SchemaPrefix))
					continue;

				//skip if schema not specified
				if (!string.IsNullOrEmpty(searchInSchema.Value))
				{
					//get all values of current Item
					Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;
					//get Schema of current Item
					currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
					//check if current Schema equals specified one
					if (currentSchema != searchInSchema.Value)
					{
						continue;
					}
				}
				//add current Item to List
				allItems.Add(pair.Key);
			}

			hasItem.Value = allItems.Contains(itemName.Value);

			if (!string.IsNullOrEmpty(fieldName.Value))
			{
				try
				{
					Dictionary<string, object> data;
					if (GDEDataManager.Get(itemName.Value, out data))
					{
						string val;
						data.TryGetString(fieldName.Value, out val);
						result = val;
					}

					result = GDEDataManager.GetString(itemName.Value, fieldName.Value, result);

					if (!string.IsNullOrEmpty(result))
					{
						hasResult.Value = true;
						storeResult.Value = result;
					}
					else
					{
						hasResult.Value = false;
					}
				}
				catch (UnityException ex)
				{
					UnityEngine.Debug.LogError(ex.ToString());
				}
			}

			Finish();
		}
	}
}

#endif
