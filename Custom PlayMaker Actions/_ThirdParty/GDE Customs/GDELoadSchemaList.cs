using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Fill the specified ArrayList with all available Schemas. Optionally save the list via direct reference to the ArrayList Proxy-Component, as String or String-Array.")]
	public class GDELoadSchemaList : ArrayListActions
	{
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
			List<string> allSchemas = new List<string>();
			string currentSchema = "";
			foreach(KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
			{
					if (pair.Key.StartsWith(GDMConstants.SchemaPrefix))
							continue;

					Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;
					currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
					if (!allSchemas.Contains(currentSchema))
						allSchemas.Add(currentSchema);
			}

			if (Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<PlayMakerArrayListProxy>() != null) {
				PlayMakerArrayListProxy _proxy = GetArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value,false);
				_proxy.AddRange(allSchemas, string.Empty);
			}

			if (viaProxyReference != null)
				viaProxyReference.AddRange(allSchemas, string.Empty);

			if (storeAsString != null) {
				foreach (string schema in allSchemas) {
					if (schema == allSchemas.ToArray().GetValue(allSchemas.ToArray().Length - 1)) {
						storeAsString.Value = string.Concat(storeAsString.Value + schema);
					} else {
						storeAsString.Value = string.Concat(storeAsString.Value + schema + ", ");
					}
				}
				string end = ", ";
				storeAsString.Value.Remove(storeAsString.Value.Length - 2);
			}

			if (storeAsStringArray != null)
				storeAsStringArray.Values = allSchemas.ToArray();

		}
	}
}

#endif
