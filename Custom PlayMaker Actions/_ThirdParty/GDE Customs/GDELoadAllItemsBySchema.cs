using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Fill the specified ArrayList with all Items by Schema-Name. Optionally save the list via direct reference to an ArrayList Proxy-Component, as String or String-Array.")]
	public class GDELoadAllItemsBySchema : ArrayListActions
	{
		[Tooltip("The name of the Schema to get the Items from. If empty, gets all Items.")]
		public FsmString schema;

		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[ActionSection("Optionally")]

		[Tooltip("Drag the GameObject or Component of the desired ArrayList Proxy here.")]
		public PlayMakerArrayListProxy viaProxyReference;

		[UIHint(UIHint.Variable)]
		[Tooltip("Save the List as one, continuous String.")]
		public FsmString storeAsString;

		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		[Tooltip("Save the List in an String Array. Array has to be of type 'String' in the Variables-Tab.")]
		public FsmArray storeAsStringArray;

		public override void OnEnter()
		{
			List<string> allItems = new List<string>();
      string currentSchema = "";
			foreach(KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
			{
				if (pair.Key.StartsWith(GDMConstants.SchemaPrefix))
					continue;

				//skip if not in the specified schema
        if (schema.Value != null && schema.Value != "") {
					//get all values of current Item
	        Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;
          //get Schema of current Item
          currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
          //check if current Schema equals specified one
          if (!(currentSchema == schema.Value)) {
            continue;
          }
        }
				//add current Item to List
				allItems.Add(pair.Key);
			}
			if (Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<PlayMakerArrayListProxy>() != null) {
				PlayMakerArrayListProxy _proxy = GetArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value,false);
				_proxy.AddRange(allItems, string.Empty);
			}

			if (viaProxyReference != null)
				viaProxyReference.AddRange(allItems, string.Empty);

			if (storeAsString != null) {
				foreach (string schema in allItems) {
					if (schema == allItems.ToArray().GetValue(allItems.ToArray().Length - 1)) {
						storeAsString.Value = string.Concat(storeAsString.Value + schema);
					} else {
						storeAsString.Value = string.Concat(storeAsString.Value + schema + ", ");
					}
				}
			}

			if (storeAsStringArray != null)
				storeAsStringArray.Values = allItems.ToArray();

			Finish();
		}
	}
}

#endif
