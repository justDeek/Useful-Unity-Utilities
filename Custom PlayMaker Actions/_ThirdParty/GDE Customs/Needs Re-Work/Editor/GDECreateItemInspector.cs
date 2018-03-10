
///!!!!!!!!!
///SUSPENDED - because it brought more problems than it solved, but still good for reference
///!!!!!!!!!

using System;
using UnityEngine;
using UnityEditor;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using GameDataEditor;
using System.Collections.Generic;

#if GDE_PLAYMAKER_SUPPORT

//[CustomActionEditor(typeof(GDECreateItem))]
public class GDECreateItemInspector : CustomActionEditor
{
	private PlayMakerFSM thisFSM;
	private static GDECreateItem action;
	private bool isDirty = false;
	private bool isSchemaVar = false;
	private bool isItemVar = false;
	private List<string> allSchemas = new List<string>();
	private NamedVariable[] allFsmVariables;
	List<string> tempStringList = new List<string>();
	private List<string> allFsmStrings = new List<string>();
	private List<string> allItems;
	private string selectedSchema;
	private string selectedSchemaVar;
	private string selectedItem;
	private string selectedItemVar;
	private int schemaID;
	private int schemaVarID;
	private int lastSchemaID;
	private int lastSchemaVarID;
	private int itemID;
	private int itemVarID;
	private int lastItemID;
	private int lastItemVarID;

	public override void OnEnable()
	{
		GDEItemManager.Load(true);
		GetSchemas();
	}

	public override bool OnGUI()
	{
		///Add default value
		tempStringList.Add("None");

		///Get action and FSM reference
		action = target as GDECreateItem;

		if(action.Fsm == null || action.Fsm.FsmComponent == null)
		{
			return false;
		}
		thisFSM = action.Fsm.FsmComponent;

		///Get all FSMStringVariables
		allFsmVariables = thisFSM.FsmVariables.GetAllNamedVariables();
		foreach(var tmpString in allFsmVariables)
		{
			if(tmpString.VariableType != VariableType.String)
			{
				continue;
			}

			string tmp = String.Concat(tmpString.GetDisplayName(), " (\"", thisFSM.FsmVariables.GetVariable(tmpString.Name), "\")");

			tempStringList.Add(tmp);
		}
		allFsmStrings = tempStringList;

		///Display Schema Drop-Down
		GUILayout.BeginHorizontal();
		GUILayout.Label("Schema", GUILayout.Width(145), GUILayout.ExpandWidth(true));
		if(isSchemaVar) ///Use FSMString
		{
			schemaVarID = EditorGUILayout.Popup(lastSchemaVarID, allFsmStrings.ToArray(), GUILayout.ExpandWidth(true));
			if(lastSchemaVarID != schemaVarID)
			{
				selectedSchemaVar = allFsmStrings[schemaVarID];
				lastSchemaVarID = schemaVarID;
				isDirty = true;
			} else
			{
				selectedSchemaVar = allFsmStrings[lastSchemaVarID];
			}

			if(!string.IsNullOrEmpty(selectedSchemaVar))
			{
				action.schema.Value = selectedSchemaVar;
			}
		} else ///Use Drop-Down
		{
			schemaID = EditorGUILayout.Popup(lastSchemaID, allSchemas.ToArray(), GUILayout.ExpandWidth(true));
			if(lastSchemaID != schemaID)
			{
				selectedSchema = allSchemas[schemaID];
				lastSchemaID = schemaID;
				isDirty = true;
			} else
			{
				selectedSchema = allSchemas[lastSchemaID];
			}

			if(!string.IsNullOrEmpty(selectedSchema))
			{
				action.schema.Value = selectedSchema;
			}
		}

		///Display Schema Toggle-Button
		string schemaToggleStatus = isSchemaVar ? "-" : "=";

		if(GUILayout.Button(schemaToggleStatus, GUILayout.Width(22), GUILayout.Height(16)))
		{
			isSchemaVar = isSchemaVar ? false : true;
			isDirty = true;
		}
		GUILayout.EndHorizontal();

		///Display ItemName + DropDown
		GUILayout.BeginHorizontal();
		GUILayout.Label("Item Name", GUILayout.Width(146));
		if(isItemVar)
		{
			itemVarID = EditorGUILayout.Popup(lastItemVarID, allFsmStrings.ToArray(), GUILayout.ExpandWidth(true));
			if(lastItemVarID != itemVarID)
			{
				selectedItemVar = allFsmStrings[itemVarID];
				lastItemVarID = itemVarID;
				isDirty = true;
			} else
			{
				selectedItemVar = allFsmStrings[lastItemVarID];
			}

			if(!string.IsNullOrEmpty(selectedItemVar))
			{
				action.itemName.Value = selectedItemVar;
			}
		} else
		{
			selectedItem = EditorGUILayout.TextField(selectedItem, GUILayout.ExpandWidth(true));

			if(GUILayout.Button("...", GUILayout.Width(22), GUILayout.MaxHeight(16)))
			{
				if(lastSchemaID == 0)
				{
					selectedSchema = allSchemas[0];
				}

				GetAllItemsOfSchema(selectedSchema);

				GenericMenu itemMenu = new GenericMenu();

				foreach(string item in allItems)
				{
					itemMenu.AddItem(new GUIContent(item), false, OnMenu_SetItemName, item);
				}
				itemMenu.ShowAsContext();
				isDirty = true;
			}

			if(!string.IsNullOrEmpty(selectedItem))
			{
				action.itemName.Value = selectedItem;
			}
		}

		///Display Item Toggle-Button
		string itemNameToggleStatus = isItemVar ? "-" : "=";

		if(GUILayout.Button(itemNameToggleStatus, GUILayout.Width(22), GUILayout.Height(16)))
		{
			isItemVar = isItemVar ? false : true;
			isDirty = true;
		}
		GUILayout.EndHorizontal();

		///Draw Default Inspector
		isDirty = DrawDefaultInspector();

		///Diplay Create-Now Button
		if(action.schema != null)
		{
			if(action.schema.Value != null && action.schema.Value != "")
			{
				if(action.itemName.Value != null && action.itemName.Value != "")
				{
					if(GUILayout.Button("Create Now"))
					{
						action.OnEnter();
						isDirty = true;
					}
				}
			}
		}

		return isDirty || GUI.changed;
	}


	public void GetSchemas()
	{
		List<string> allTempSchemas = new List<string>();
		string currentSchema = "";
		foreach(KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
		{
			if(pair.Key.StartsWith(GDMConstants.SchemaPrefix))
				continue;

			Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;
			currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
			if(!allTempSchemas.Contains(currentSchema))
				allTempSchemas.Add(currentSchema);
		}
		allSchemas = allTempSchemas;
	}

	public void GetAllItemsOfSchema(string schema)
	{
		List<string> allTempItems = new List<string>();
		string currentSchema = "";
		foreach(KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
		{
			if(pair.Key.StartsWith(GDMConstants.SchemaPrefix))
				continue;

			//skip if not in the specified schema
			if(schema != null && schema != "" && schema != "None")
			{
				//get all values of current Item
				Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;
				//get Schema of current Item
				currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
				//check if current Schema equals specified one
				if(currentSchema != schema)
				{
					continue;
				}
			}
			//add current Item to List
			allTempItems.Add(pair.Key);
		}

		if(allTempItems.Count == 0)
		{
			UnityEngine.Debug.LogWarning("No Items in Schema: " + schema);
		}

		allItems = allTempItems;
	}

	///Gets called when an MenuItem is selected
	private void OnMenu_SetItemName(object item)
	{
		selectedItem = item.ToString();
		isDirty = true;
	}
}

#endif