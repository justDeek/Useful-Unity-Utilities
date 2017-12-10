using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Store a random Item in a String. Optional parameters: Only get Items from a specific Schema, that contain a specific value and/or if their name contains a certain string (useful to search within range).")]
	public class GDEGetRandomItem : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The Name of the Item in the Schema.")]
		public FsmString storeItem;

		[ActionSection("Optionally")]

		[Tooltip("The Name of Schema the random Item has to be in.")]
		public FsmString schema;

		[Tooltip("Only from any Item that contains this Key. Can be any Field-Name (Example: If you had an Item 'Slot1' with Field-Name 'Amount', Type 'int' and Value '2'; then 'Amount' would be valid to get 'Slot1' as Item)")]
		public FsmString containsKey;

		[Tooltip("Only from any Item that contains this Value. Can be any Field-Value (in the above example, '2' would be valid to get 'Slot1' as Item)")]
		public FsmString containsValue;

		[Tooltip("Only from Items which name contain this value.")]
		public FsmString itemNameContains;

		public override void Reset()
		{
			schema = null;
			storeItem = null;
			containsKey = null;
			containsValue = null;
			itemNameContains = null;
		}

		public override void OnEnter()
		{
			string currentSchema = "";
			List<string> allItems = new List<string>();

			foreach(KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
			{
				if(pair.Key.StartsWith(GDMConstants.SchemaPrefix))
					continue;

				//skip current Item if it doens't contain the specified string
				if(itemNameContains.Value != null && itemNameContains.Value != "")
				{
					if(!pair.Key.Contains(itemNameContains.Value))
					{
						continue;
					}
				}
				//get all values of current Item
				Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;

				//skip if not in the specified schema
				if(schema.Value != null && schema.Value != "")
				{
					//get Schema of current Item
					currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
					//check if current Schema equals specified one
					if(!(currentSchema == schema.Value))
					{
						continue;
					}
				}
				//skip current Item if it doesn't contain the specified Key
				if(containsKey.Value != null && containsKey.Value != "")
				{
					if(!currentDataSet.ContainsKey(containsKey.Value))
					{
						continue;
					}
				}
				//skip current Item if it doesn't contain the specified Value
				if(containsValue.Value != null && containsValue.Value != "")
				{
					if(!currentDataSet.ContainsValue(containsValue.Value))
					{
						continue;
					}
				}
				//add current Item to List
				allItems.Add(pair.Key);
			}
			if(allItems.Count == 0)
			{
				LogError("Couldn't get any Items! Probably because of wrong/too narrow search parameters.");
			} else
			{
				storeItem.Value = allItems.Random();
			}
			Finish();
		}
	}
}

#endif
