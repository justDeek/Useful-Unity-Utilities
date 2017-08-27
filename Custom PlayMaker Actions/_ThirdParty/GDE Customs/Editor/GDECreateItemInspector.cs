using System;
using UnityEngine;
using UnityEditor;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using GameDataEditor;
using System.Collections.Generic;

#if GDE_PLAYMAKER_SUPPORT

[CustomActionEditor(typeof(GDECreateItem))]
public class GDECreateItemInspector : CustomActionEditor
{
    private PlayMakerFSM thisFSM;
    private static GDECreateItem action;
    private bool isDirty;
    private bool isSchemaVar;
    private bool isItemVar;
    private string[] allSchemas;
    private NamedVariable[] allFsmStrings;
    List<string> tempStringList = new List<string>();
    private string[] allStrings;
    private string[] allItems;
    private string selectedSchema;
    private string selectedSchemaVar;
    private int schemaPopupID;
    private int schemaVarID;
    private int lastSchemaPopupID;
    private int lastSchemaVarID;
    private int itemVarID;
    private int lastItemVarID;
    private string finalItemName;

    public override void OnEnable()
    {
        //tempStringList.Clear();
        GDEItemManager.Load(true);
        GetSchemas();
    }

    public override bool OnGUI()
    {
        tempStringList.Add("None");
//get action reference & all FSMStringVariables
        if (target != null)
        {
            action = target as GDECreateItem;
            if (action != null && action.Fsm != null && action.Fsm.FsmComponent != null)
            {
                thisFSM = action.Fsm.FsmComponent;
                if (thisFSM != null)
                {
                    allFsmStrings = thisFSM.FsmVariables.GetAllNamedVariables();
                    foreach (var tmpString in allFsmStrings)
                    {
                        string tmp = String.Concat(tmpString.GetDisplayName(), " (\"", thisFSM.FsmVariables.GetVariable(tmpString.Name), "\")");

                        tempStringList.Add(tmp);
                    } 
                }
            } 
        }
        allStrings = tempStringList.ToArray();

//Display Schema PopUp
        GUILayout.BeginHorizontal();
        GUILayout.Label("Schema", GUILayout.Width(145));
        if (isSchemaVar)
        {
            schemaVarID = EditorGUILayout.Popup(lastSchemaVarID, allStrings, GUILayout.ExpandWidth(true));
            if (lastSchemaVarID != schemaVarID)
            {
                selectedSchemaVar = allStrings[schemaVarID];
                if (selectedSchemaVar != null && selectedSchemaVar != "")
                {
                    action.Schema.Value = selectedSchemaVar;
                    isDirty = true;
                }
                lastSchemaVarID = schemaVarID;
            }
            else
            {
                selectedSchemaVar = allStrings[schemaVarID];
                if (action != null && action.Schema != null && (action.Schema == null || action.Schema.Value == ""))
                {
                    action.Schema.Value = selectedSchemaVar;
                    isDirty = true;
                }
            }
        }
        else
        {
            schemaPopupID = EditorGUILayout.Popup(lastSchemaPopupID, allSchemas, GUILayout.ExpandWidth(true));
            if (lastSchemaPopupID != schemaPopupID)
            {
                selectedSchema = allSchemas[schemaPopupID];
                if (selectedSchema != null && selectedSchema != "")
                {
                    action.Schema.Value = selectedSchema;
                    isDirty = true;
                }
                lastSchemaPopupID = schemaPopupID;
            }
            else
            {
                selectedSchemaVar = allStrings[schemaVarID];
                if (action != null && action.Schema != null && (action.Schema.Value == null || action.Schema.Value == ""))
                {
                    action.Schema.Value = selectedSchemaVar;
                    isDirty = true;
                }
            }
        }
//Display Schema Toggle-Button
        string schemaToggleStatus = "=";
        if (isSchemaVar)
            schemaToggleStatus = "-";
        if (GUILayout.Button(schemaToggleStatus, GUILayout.Width(22), GUILayout.Height(16)))
        {
            if (isSchemaVar)
            {
                isSchemaVar = false;
            }
            else
            {
                isSchemaVar = true;
            }
            isDirty = true;
        }
        GUILayout.EndHorizontal();
        
//Display ItemName + DropDown
        GUILayout.BeginHorizontal();
        GUILayout.Label("Item Name", GUILayout.Width(146));
        if (isItemVar)
        {
            itemVarID = EditorGUILayout.Popup(lastItemVarID, allStrings, GUILayout.ExpandWidth(true));
            if (lastItemVarID != itemVarID)
            {
                selectedSchemaVar = allStrings[itemVarID];
                if (selectedSchemaVar != null && selectedSchemaVar != "")
                {
                    action.Schema.Value = selectedSchemaVar;
                    isDirty = true;
                }
                lastItemVarID = itemVarID;
            }
        }
        else
        {
            finalItemName = EditorGUILayout.TextField(finalItemName, GUILayout.ExpandWidth(true));
            action.ItemName1.Value = finalItemName;
            if (GUILayout.Button("...", GUILayout.Width(22), GUILayout.MaxHeight(16)))
            {
                if (lastSchemaPopupID == 0)
                {
                    selectedSchema = allSchemas[0];
                }
                if (selectedSchema != null && selectedSchema != "")
                {
                    GetAllItemsOfSchema(selectedSchema);

                    GenericMenu itemMenu = new GenericMenu();
                    foreach (string item in allItems)
                    {
                        itemMenu.AddItem(new GUIContent(item), false, OnMenu_SetItemName, item);
                    }
                    itemMenu.ShowAsContext();

                    isDirty = true;
                }
            }
        }
//Display Item Toggle-Button
        string itemNameToggleStatus = "=";
        if (isItemVar)
            itemNameToggleStatus = "-";
        if (GUILayout.Button(itemNameToggleStatus, GUILayout.Width(22), GUILayout.Height(16)))
        {
            if (isItemVar)
            {
                isItemVar = false;
            }
            else
            {
                isItemVar = true;
            }
            isDirty = true;
        }
        GUILayout.EndHorizontal();
//Draw Default Inspector
        isDirty = DrawDefaultInspector();
//Diplay Create-Now Button
        if (action != null && action.Schema != null)
        {
            if (action.Schema.Value != null && action.Schema.Value != "")
            {
                if (action.ItemName1.Value != null && action.ItemName1.Value != "")
                {
                    if (GUILayout.Button("Create Now"))
                    {
                        action.OnEnter();
                        isDirty = true;
                    }
                }
            }
        }

        return isDirty || GUI.changed;
    }

    //public override void OnSceneGUI()
    //{
        
    //}

    public void GetSchemas()
    {
        List<string> allTempSchemas = new List<string>();
        string currentSchema = "";
        foreach (KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
        {
            if (pair.Key.StartsWith(GDMConstants.SchemaPrefix))
                continue;

            Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;
            currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
            if (!allTempSchemas.Contains(currentSchema))
                allTempSchemas.Add(currentSchema);
        }
        allSchemas = allTempSchemas.ToArray();
    }

    public void GetAllItemsOfSchema(string schema)
    {
        List<string> allTempItems = new List<string>();
        string currentSchema = "";
        foreach (KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
        {
            if (pair.Key.StartsWith(GDMConstants.SchemaPrefix))
                continue;

            //skip if not in the specified schema
            if (schema != null && schema != "")
            {
                //get all values of current Item
                Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;
                //get Schema of current Item
                currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
                //check if current Schema equals specified one
                if (currentSchema != schema)
                {
                    continue;
                }
            }
            //add current Item to List
            allTempItems.Add(pair.Key);
        }
        allItems = allTempItems.ToArray();
    }
    
    //gets called when an MenuItem is selected
    private void OnMenu_SetItemName(object item)
    {
        finalItemName = item.ToString();
        isDirty = true;
    }

    private void OnMenu_SetFsmString(object tmpString)
    {
        
    }
}

#endif