using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;
using System.Text;
using System;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
  [ActionCategory(GDMConstants.ActionCategory)]
  [Tooltip("Store the Schema of the given Item Name.")]
  public class GDEGetSchemaByItem : FsmStateAction
  {
		[Tooltip("The Name of the Item in the Schema.")]
    public FsmString itemName;

    [UIHint(UIHint.Variable)]
		[Tooltip("The Name of the Schema to store.")]
    public FsmString storeSchema;

    public override void Reset()
    {
			itemName = null;
      storeSchema = null;
    }

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
          {
            allSchemas.Add(currentSchema);
            if (pair.Key.Equals(itemName.Value))
            {
              storeSchema.Value = currentSchema;
            }
          }
      }
      if (storeSchema.Value == null || storeSchema.Value == "") {
        UnityEngine.Debug.LogError("No Schema with Item \"" + itemName.Value + "\" found!");
      }
    }
  }
}

#endif
