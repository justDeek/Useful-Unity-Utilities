using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Fill the specified ArrayList with all Keys (Field-Names) by Schema-Name. Optionally save the list via direct reference to an ArrayList Proxy-Component, as String or String-Array.")]
	public class GDELoadAllKeysBySchema : ArrayListActions
	{
		[RequiredField]
		[Tooltip("The name of the Schema to get all Keys from.")]
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
			List<object> allKeys = new List<object>();
			string currentSchema = "";
			string currentKey = "";
			foreach(KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
			{
				if (pair.Key.StartsWith(GDMConstants.SchemaPrefix))
					continue;

				Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;

				//skip if not in the specified schema
        if (schema.Value != null && schema.Value != "") {
          //get Schema of current Item
          currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
          //check if current Schema equals specified one
          if (!(currentSchema == schema.Value)) {
            continue;
          }
        }

				foreach (var subPair in currentDataSet)
				{
					currentKey = subPair.Key;
					if (!currentKey.Contains("_gdeType_") && !currentKey.Contains("_gdeSchema")) {
						if (!allKeys.Contains(subPair.Key)) {
							allKeys.Add(subPair.Key);
						}
					} else {
						continue;
					}
				}
			}

			if (Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<PlayMakerArrayListProxy>() != null) {
				PlayMakerArrayListProxy _proxy = GetArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value,false);
				_proxy.AddRange(allKeys, string.Empty);
			}

			if (viaProxyReference != null)
				viaProxyReference.AddRange(allKeys, string.Empty);

			if (storeAsString != null) {
				foreach (string key in allKeys) {
					if (key == allKeys.ToArray().GetValue(allKeys.ToArray().Length - 1)) {
						storeAsString.Value = string.Concat(storeAsString.Value + key);
					} else {
						storeAsString.Value = string.Concat(storeAsString.Value + key + ", ");
					}
				}
			}

			if (storeAsStringArray != null)
				storeAsStringArray.Values = allKeys.ToArray();

			Finish();
		}
	}
}

#endif
