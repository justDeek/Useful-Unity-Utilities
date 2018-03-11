///ToDo: FindFieldByValue | is it possible to create Schemas? | SortBy, GDEStringSwitch, GDECompareFloat, GDECompareInt

///Current Problems: Setting the field values in CreateItem doesn't seem to work (at least not in the action
///GDECreateItem) | searching for FieldTypes in GDEFind returns no results/errors

/* Notes:
 * GDEDataManager.DataDictionary contains all pre-defined Schemas, Items, Field Types, Field Names and Field Values
 *						   ... structure: Dictionary<string, object> = <ItemName, <<Description, Type>, <Key, Value>, <Type, Key>, <Value, SchemaName>>>
 *										  example: <QA-Slot 1, <<_gdeType_Amount, int>, <Amount, 0>, <String, ItemName>, <Item 202, Inventory>>>
 * 
 * GDEDataManager.ModifiedData contains all runtime values, Items saved via scripts or otherwise altered Items
 *						   ... structure: Dictionary<string, Dictionary<string, object>> = <Item name, (Field name, Field value)>
 *										  The first value-pair of every Item holds the _gdeSchema and the Fields seem to be
 *										  sorted backwards, meaning the last Field comes right after the _gdeSchema pair
 */

using GameDataEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

#if GDE_PLAYMAKER_SUPPORT

namespace iDecay.GDE
{
	#region Enums
	/////////////

	public enum GDEDictionary
	{
		DataDictionary,
		ModifiedData,
		Both
	}

	public enum GDEDataType
	{
		Schema,
		Item,
		FieldName,
		FieldType,
		FieldValue
	}

	//all supported basic data types for Field values (except for lists and lists of lists)
	public enum GDEFieldType
	{
		None,
		Bool,
		BoolList,
		BoolTwoDList,
		Int,
		IntList,
		IntTwoDList,
		Float,
		FloatList,
		FloatTwoDList,
		String,
		StringList,
		StringTwoDList,
		Vector2,
		Vector2List,
		Vector2TwoDList,
		Vector3,
		Vector3List,
		Vector3TwoDList,
		Vector4,
		Vector4List,
		Vector4TwoDList,
		Color,
		ColorList,
		ColorTwoDList,
		GameObject,
		GameObjectList,
		GameObjectTwoDList,
		Texture2D,
		Texture2DList,
		Texture2DTwoDList,
		Material,
		MaterialList,
		MaterialTwoDList,
		AudioClip,
		AudioClipList,
		AudioClipTwoDList
	}

	public enum GDEOperation
	{
		Add,
		Subtract,
		Multiply,
		Divide
	}

	public enum SearchType
	{
		Contains,
		Equals,
		DoesNotContain,
		DoesNotEqual,
		StartsWith,
		EndsWith,
	}

	#endregion

	/// <summary>
	/// Various methods that can retrieve the GDE data at runtime, unlike those from GDEItemManager.
	/// </summary>
	public static class GDEHelpers
	{
		#region General
		///////////////

		/// <summary>
		/// Initialize the GDE data to retrieve its values.
		/// </summary>
		/// <param name="fileName">The name of the file that contains all relevant GDE data (usually in "Game Data Editor>Ressources")</param>
		/// <param name="encrypted">Wheter the data should be encrypted (should be also resembled by adding "_enc" to the file-name.</param>
		public static void GDEInit(string fileName = "gde_data", bool encrypted = false)
		{
			try
			{
				if(!GDEDataManager.Init(fileName, encrypted))
				{
					UnityEngine.Debug.LogError(GDMConstants.ErrorNotInitialized + " " + fileName);
				}
			} catch(UnityException ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
		}

		/// <summary>
		/// Checks wheter GDE has already been initialized (tries to load any Schema).
		/// </summary>
		public static bool GDEWasInitialized()
		{
			bool hasBeenInitialized = false;
			var allData = GDEGetAllDataBy(GDEDataType.Schema);

			if(allData != null || allData.Count != 0) hasBeenInitialized = true;

			return hasBeenInitialized;
		}

		public static void Save()
		{
			GDEDataManager.Save();
		}

		#endregion

		#region Data
		////////////

		/// <summary>
		/// Combine DataDictionary and ModifiedData dictionaries to retrieve all available GDE data.
		/// </summary>
		public static Dictionary<string, object> GetAllGDEData()
		{
			Dictionary<string, object> allGDEData = GDEDataManager.DataDictionary;

			foreach(var modPair in GDEDataManager.ModifiedData)
			{
				if(!allGDEData.ContainsKey(modPair.Key))
				{
					allGDEData.Add(modPair.Key, modPair.Value as object);
				}
			}

			return allGDEData;
		}

		/// <summary>
		/// Retrieve all available and relevant data, limited by specified parameters.
		/// </summary>
		/// <param name="outputType">Define what type should be returned. Current options: 'Schema', 'Item' and 'Field'.</param>
		/// <param name="limitBySchema">Limit result to only contains entries by given Schema.</param>
		/// <param name="limitByItem">Limit result to only contains entries by given Item Name.</param>
		/// <param name="limitByField">Limit result to only contains entries by given Field Name.</param>
		/// <param name="sort">Sort the list alphanumerically.</param>
		public static List<object> GDEGetAllDataBy(GDEDataType outputType, string limitBySchema = "",
												   string limitByItem = "", string limitByFieldType = "",
												   string limitByFieldName = "", object limitByFieldValue = null,
												   bool sort = false)
		{
			Dictionary<string, object> allGDEData = GetAllGDEData();
			Dictionary<string, object> currentDataSet = new Dictionary<string, object>();
			List<object> result = new List<object>();
			string currentSchema;

			//go through all data pairs to limit the result and list all entries by type
			foreach(KeyValuePair<string, object> pair in allGDEData)
			{
				//skip irrelevant KeyPairs
				if(pair.Key.StartsWith(GDMConstants.SchemaPrefix)) continue;

				currentDataSet = pair.Value as Dictionary<string, object>;

				if(limitBySchema != "")
				{
					//extract current Schema
					currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);

					//check if current Schema equals specified one
					if(currentSchema != limitBySchema) continue;
				}

				//skip if it doesn't contain the specified ItemName
				if(!string.IsNullOrEmpty(limitByItem) && pair.Key != limitByItem) continue;
				//skip if it doesn't contain the specified FieldType
				if(!string.IsNullOrEmpty(limitByFieldType)
					&& !DataSetContainsFieldType(currentDataSet, limitByFieldType)) continue;
				//skip if it doesn't contain the specified FieldName
				if(!string.IsNullOrEmpty(limitByFieldName)
					&& !DataSetContainsFieldName(currentDataSet, limitByFieldName)) continue;
				//skip if it doesn't contain the specified FieldValue
				if(limitByFieldValue != null
					&& !DataSetContainsFieldValue(currentDataSet, limitByFieldValue)) continue;

				//only add entries of given type
				switch(outputType)
				{
					case GDEDataType.Schema:
						currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
						result.AddUnique(currentSchema);
						break;
					case GDEDataType.Item:
						result.AddUnique(pair.Key);
						break;
					case GDEDataType.FieldName:
						foreach(var subPair in currentDataSet)
						{
							if(IsRelevant(subPair.Key)) result.AddUnique(subPair.Key);
						}
						break;
					case GDEDataType.FieldType:
						foreach(var subPair in currentDataSet)
						{
							if(IsRelevant(subPair.Key, true)) result.AddUnique(subPair.Value);
						}
						break;
					case GDEDataType.FieldValue:
						foreach(var subPair in currentDataSet)
						{
							if(IsRelevant(subPair.Key)) result.AddUnique(subPair.Value);
						}
						break;
				}
			}

			//if set, sort the complete list
			if(sort) result.Sort();

			return result;
		}

		/// <summary>
		/// Returns a list of all data by data type.
		/// </summary>
		/// <param name="searchBy">What type of data to list.</param>
		public static List<string> ListAllBy(GDEDataType searchBy, string limitBySchema = "")
		{
			List<string> currList = new List<string>();

			switch(searchBy)
			{
				case GDEDataType.Schema:
					currList = GetAllSchemas();
					break;
				case GDEDataType.Item:
					currList = GetAllItems(limitBySchema);
					break;
				case GDEDataType.FieldName:
					currList = GetAllFieldNames(limitBySchema);
					break;
				default:
					break;
			}

			if(currList.Count == 0)
			{
				UnityEngine.Debug.LogError("Empty List in ListAllBy(" + searchBy.ToString() + ", " + limitBySchema + ")!");
				return null;
			}

			return currList;
		}

		/// <summary>
		/// Returns the first occurrence of the given search-parameter.
		/// </summary>
		/// <param name="dataType">The type to search through (Schema, Item or Field).</param>
		/// <param name="searchType">If the given String should contain, start/end with or equal the searched entries.</param>
		/// <param name="searchBy">Searches for this String in every entry of given type.</param>
		/// <param name="limitBySchema">Optionally only search through a specific Schema.</param>
		/// <returns></returns>
		public static string Find(GDEDataType dataType, SearchType searchType, string searchBy, string limitBySchema = "")
		{
			foreach(var entry in ListAllBy(dataType))
			{
				switch(searchType)
				{
					case SearchType.Contains:
						if(entry.Contains(searchBy)) return entry;
						break;
					case SearchType.Equals:
						if(entry == searchBy) return entry;
						break;
					case SearchType.DoesNotContain:
						if(!entry.Contains(searchBy)) return entry;
						break;
					case SearchType.DoesNotEqual:
						if(entry != searchBy) return entry;
						break;
					case SearchType.StartsWith:
						if(entry.StartsWith(searchBy)) return entry;
						break;
					case SearchType.EndsWith:
						if(entry.EndsWith(searchBy)) return entry;
						break;
				}
			}

			UnityEngine.Debug.LogWarning("Couldn't find any matching " + searchBy.ToString()
										  + " which " + searchType.ToString() + " \"" + searchBy + "\".");

			return "";
		}

		/// <summary>
		/// Returns all occurrences of the given search-parameter.
		/// </summary>
		/// <param name="dataType">The type to search through (Schema, Item or Field).</param>
		/// <param name="searchType">If the given String should contain, start/end with or equal the searched entries.</param>
		/// <param name="searchBy">Searches for this String in every entry of given type.</param>
		/// <param name="limitBySchema">Optionally only search through a specific Schema.</param>
		/// <returns></returns>
		public static List<string> FindAll(GDEDataType dataType, SearchType searchType,
											  string searchBy, string limitBySchema = "")
		{
			List<string> result = new List<string>();

			foreach(var entry in ListAllBy(dataType))
			{
				switch(searchType)
				{
					case SearchType.Contains:
						if(entry.Contains(searchBy)) result.AddUnique(entry);
						break;
					case SearchType.Equals:
						if(entry == searchBy) result.AddUnique(entry);
						break;
					case SearchType.DoesNotContain:
						if(!entry.Contains(searchBy)) result.AddUnique(entry);
						break;
					case SearchType.DoesNotEqual:
						if(entry != searchBy) result.AddUnique(entry);
						break;
					case SearchType.StartsWith:
						if(entry.StartsWith(searchBy)) result.AddUnique(entry);
						break;
					case SearchType.EndsWith:
						if(entry.EndsWith(searchBy)) result.AddUnique(entry);
						break;
				}
			}

			if(result.Count == 0)
			{
				UnityEngine.Debug.LogWarning("Couldn't find any matching " + dataType.ToString()
										  + " which " + searchType.ToString() + " \"" + searchBy + "\".");
			}

			return result;
		}

		/// <summary>
		/// Finds all matching occurrences with given search-parameters.
		/// </summary>
		public static List<string> FindAllMatching(GDEDataType dataType, SearchType[] searchTypes,
												   string[] searchBy, string limitBySchema = "")
		{
			List<string> result = FindAll(dataType, searchTypes[0], searchBy[0], limitBySchema);

			if(result.Count == 0)
			{
				UnityEngine.Debug.LogWarning("Couldn't find any matching " + dataType.ToString()
										  + " which " + searchTypes[0].ToString() + " \"" + searchBy[0] + "\".");
				return result;
			}

			for(int i = 1; i < searchTypes.Length; i++)
			{
				List<string> tmpResult = FindAll(dataType, searchTypes[i], searchBy[i], limitBySchema);
				result = result.Matches(tmpResult);
			}

			return result;
		}

		/// <summary>
		/// Returns the amount of occurences of the given search-parameter.
		/// </summary>
		/// <param name="searchBy">The type to search through (Schema, Item or Field).</param>
		/// <param name="contains">Searches for this String in every entry of given type.</param>
		/// <returns></returns>
		public static int CountOccurrences(GDEDataType searchBy, string contains, string limitBySchema = "")
		{
			int result = 0;

			foreach(var entry in ListAllBy(searchBy))
			{
				if(entry.Contains(contains)) result++;
			}

			return result;
		}

		/// <summary>
		/// Returns a random occurence of the given search-parameter.
		/// </summary>
		/// <param name="searchBy">The type to search through (Schema, Item or Field).</param>
		/// <param name="contains">Searches for this String in every entry of given type.</param>
		/// <returns></returns>
		public static string GetRandom(GDEDataType searchBy, string bySchema = "")
		{
			List<string> currList = new List<string>();

			switch(searchBy)
			{
				case GDEDataType.Schema:
					currList = GetAllSchemas();
					break;
				case GDEDataType.Item:
					currList = GetAllItems(bySchema);
					break;
				case GDEDataType.FieldName:
					currList = GetAllFieldNames(bySchema);
					break;
			}

			return currList.Random();
		}

		/// <summary>
		/// Flattens the ModifiedData dictionary to match the structure of the DataDictionary
		/// (item name as key and object containing field name and value).
		/// </summary>
		public static Dictionary<string, object> ConvertModifiedData()
		{
			Dictionary<string, object> modDict = new Dictionary<string, object>();

			foreach(var modPair in GDEDataManager.ModifiedData)
			{
				if(!modDict.ContainsKey(modPair.Key))
				{
					modDict.Add(modPair.Key, modPair.Value as object);
				}
			}

			return modDict;
		}

		#endregion

		#region Schemas
		///////////////

		/// <summary>
		/// Returns a list of all available Schemas.
		/// </summary>
		public static List<string> GetAllSchemas()
		{
			return GDEGetAllDataBy(GDEDataType.Schema).ToStringList();
		}

		/// <summary>
		/// Check wheter GDE contains the given Schema.
		/// </summary>
		public static bool HasSchema(string schema)
		{
			List<string> allSchemas = GDEGetAllDataBy(GDEDataType.Schema).ToStringList();

			return allSchemas.Contains(schema);
		}

		/// <summary>
		/// Returns the Schema containing the given Item.
		/// </summary>
		public static string GetSchemaByItem(string itemName)
		{
			List<object> matchingSchemas = GDEGetAllDataBy(GDEDataType.Schema, itemName);

			if(matchingSchemas.Count == 0)
			{
				UnityEngine.Debug.LogError("Couldn't find the Schema of the Item " + itemName + "!");
			}

			return matchingSchemas[0] as string;
		}

		#endregion

		#region Items (Keys)
		////////////////////

		/// <summary>
		/// Returns a list of all available Items.
		/// </summary>
		public static List<string> GetAllItems(string limitBySchema = "")
		{
			List<object> dataList = GDEGetAllDataBy(GDEDataType.Item, limitBySchema);
			List<string> strList = new List<string>();

			foreach(var data in dataList)
			{
				strList.Add(data.ToString());
			}

			if(strList.Count == 0)
				UnityEngine.Debug.LogError("Empty result in GetAllItems!");

			return strList;
		}

		/// <summary>
		/// Returns all runtime item names.
		/// </summary>
		public static List<string> GetAllModifiedItems()
		{
			List<string> result = new List<string>();

			foreach(var modPair in GDEDataManager.ModifiedData)
			{
				result.AddUnique(modPair.Key);
			}

			return result;
		}

		/// <summary>
		/// Returns wheter GDE contains the given Item.
		/// </summary>
		/// <param name="item">Name of the Item (Key) to search.</param>
		/// <param name="schema">Optionally only search within a specific Schema.</param>
		public static bool HasItem(string item, string schema = "")
		{
			bool hasItem = false;
			hasItem = GDEGetAllDataBy(GDEDataType.Item, schema).Contains(item);

			return hasItem;
		}

		/// <summary>
		/// Register a new Item to the GDE database.
		/// </summary>
		/// <param name="schema">Name of the Schema this Item should be added to.</param>
		/// <param name="itemName">The Name of the Item to create.</param>
		/// <param name="fieldNames">If set, specify into what Fields the values should be populated.</param>
		/// <param name="fieldTypes">The type of the field.</param>
		/// <param name="fieldValues">Optionally fill the newly created Items with values in the specified Fields.</param>
		/// <param name="save">Saves changes at the end by default.</param>
		public static void CreateItem(string schema, string itemName, string[] fieldNames = null,
									  object[] fieldValues = null, GDEFieldType[] fieldTypes = null, bool save = true)
		{
			//check if Schema already contains an Item with the same name
			if(CheckHasItem(itemName, schema, true, false)) return;

			//register Item
			GDEDataManager.RegisterItem(schema, itemName);

			//skip rest if no field names or values set
			if(fieldNames.Length == 0 || fieldValues.Length == 0) return;

			//set fields and values
			for(int i = 0; i < fieldNames.Length; i++)
			{
				GDEFieldType currFieldType = fieldTypes == null || fieldTypes.Length == 0
											 ? ParseFieldValueType(fieldValues[i])
											 : ParseFieldValueTypeIfNone(fieldValues[i], fieldTypes[i]);

				if(!CheckHasField(fieldNames[i], schema)) return;

				if(!HasField(fieldNames[i], schema)) return;
				SetFieldValue(itemName, fieldNames[i], fieldValues[i], currFieldType);
			}

			//check if Item exists now
			if(!HasItem(itemName, schema))
			{
				UnityEngine.Debug.LogError("Failed to create Item " + itemName + " into " + schema + "!");
				return;
			}

			if(save) Save();
		}

		/// <summary>
		/// Removes an item from the runtime data.
		/// </summary>
		/// <param name="itemName">The item to remove.</param>
		public static bool RemoveItem(string itemName, bool save = true, bool debugFailure = true)
		{
			if(!HasItem(itemName) && debugFailure)
			{
				UnityEngine.Debug.LogError("GDE doens't contain the item " + itemName + " and thus can't remove it!");
				return false;
			}

			GDEDataManager.DataDictionary.Remove(itemName);
			GDEDataManager.ResetToDefault(itemName);
			if(save) GDEDataManager.Save();
			return true;
		}

		/// <summary>
		/// Swaps the Field values of two Items.
		/// </summary>
		public static void SwapItems(string schema, string prevItemName, string swapWithItem, bool save = true)
		{
			Dictionary<string, object> firstItemValues = GetFieldsByItem(schema, prevItemName);
			Dictionary<string, object> secondItemValues = GetFieldsByItem(schema, swapWithItem);

			foreach(var fieldName in firstItemValues.Keys)
			{
				var firstFieldValue = firstItemValues[fieldName];
				var secondFieldValue = secondItemValues[fieldName];

				SetFieldValue(prevItemName, fieldName, secondFieldValue);
				SetFieldValue(swapWithItem, fieldName, firstFieldValue);
			}

			if(save) Save();
		}

		/// <summary>
		/// Returns all Items containing the given Field Value.
		/// </summary>
		public static List<string> FindItemsByValue(object fieldValue)
		{
			var result = GDEGetAllDataBy(GDEDataType.Item, "", "", "", "", fieldValue);
			return result.ToStringList();
		}

		/// <summary>
		/// Returns the first Item with the given Field Value.
		/// </summary>
		public static string FindFirstItemWithValue(object fieldValue)
		{
			return FindItemsByValue(fieldValue)[0];
		}

		#endregion

		#region Field Names (Values)
		///////////////////

		/// <summary>
		/// Returns a list of all available Field Names.
		/// </summary>
		public static List<string> GetAllFieldNames(string limitBySchema = "")
		{
			return GDEGetAllDataBy(GDEDataType.FieldName, limitBySchema).ToStringList();
		}

		/// <summary>
		/// Returns wheter GDE contains the given Field.
		/// </summary>
		/// <param name="item">Name of the Field (Value) to search.</param>
		/// <param name="schema">Optionally only search within a specific Schema.</param>
		public static bool HasField(string field, string schema = "")
		{
			return GetAllFieldNames(schema).Contains(field);
		}

		/// <summary>
		/// Count all available Field Names.
		/// </summary>
		public static int GetFieldAmount(string schema = "")
		{
			return GetAllFieldNames(schema).Count;
		}

		/// <summary>
		/// Returns a dictionary of all Field Names and Values by Item.
		/// </summary>
		public static Dictionary<string, object> GetFieldsByItem(string schema, string itemName)
		{
			if(string.IsNullOrEmpty(schema)) schema = GetSchemaByItem(itemName);

			//get all field names
			List<string> fieldNames = GDEGetAllDataBy(GDEDataType.FieldName, schema, itemName).ToStringList();
			Dictionary<string, object> tmpDict = new Dictionary<string, object>();

			//go through all fields and store their names and values
			for(int i = 0; i < fieldNames.Count; i++)
			{
				tmpDict.Add(fieldNames[i], GetFieldValue(itemName, fieldNames[i]));
			}

			return tmpDict;
		}

		#endregion

		#region Field Types
		///////////////////////

		/// <summary>
		/// Returns the type of the given Field Name.
		/// </summary>
		/// <param name="specifySchema">Diminish searched scope by specifying the Schema name.</param>
		public static GDEFieldType GetFieldType(string itemName, string fieldName, string specifySchema = "")
		{
			List<object> foundFieldTypes = GDEGetAllDataBy(GDEDataType.FieldType, specifySchema, itemName, "", fieldName);

			if(foundFieldTypes.Count >= 1)
			{
				return ParseFieldType(foundFieldTypes[0].ToString());
			} else if(foundFieldTypes.Count < 1)
			{
				UnityEngine.Debug.LogError("Coudn't find Field Name \"" + fieldName + "\" in "
											+ specifySchema + ": " + itemName + "!");
			}

			return GDEFieldType.None;
		}

		/// <summary>
		/// Lists all existing Field Types.
		/// </summary>
		public static List<GDEFieldType> GetAllFieldTypes()
		{
			List<object> allFieldTypes = GDEGetAllDataBy(GDEDataType.FieldType);
			List<GDEFieldType> convFieldTypes = new List<GDEFieldType>();

			foreach(var fieldType in allFieldTypes)
			{
				convFieldTypes.Add(ParseFieldValueType(fieldType));
			}

			return convFieldTypes;
		}

		#endregion

		#region Field Values
		////////////////////

		/// <summary>
		/// Get the value of the specified Field.
		/// </summary>
		public static object GetFieldValue(string itemName, string fieldName)
		{
			object result = null;

			try
			{
				Dictionary<string, object> data;
				if(GDEDataManager.Get(itemName, out data))
				{
					object val;
					data.TryGetValue(fieldName, out val);
					result = val;
				}

				if(GDEDataManager.ModifiedData.TryGetValue(itemName, out data) && data.ContainsKey(fieldName))
				{
					object temp;
					if(data.TryGetValue(fieldName, out temp) && temp != null) result = temp;
				}
			} catch(UnityException ex)
			{
				UnityEngine.Debug.LogException(ex);
			}

			return result;
		}

#pragma warning disable CS0618 // Type or member is obsolete
		/// <summary>
		/// Tries to set the value for any Field.
		/// </summary>
		/// <param name="fieldValue">Ensure that the object is of the same type as the given fieldType.</param>
		public static void SetFieldValue(string itemName, string fieldName,
										 object fieldValue, GDEFieldType fieldType = GDEFieldType.None)
		{
			if(fieldValue == null)
			{
				UnityEngine.Debug.LogError("Field Value is null while trying to set Field Value for "
											+ fieldName + " on " + itemName + "!");
				return;
			}

			GDEFieldType selFieldType = ParseFieldValueTypeIfNone(fieldValue, fieldType);

			switch(fieldValue.GetType().ToString().Replace("System.", ""))
			{
				case "Int16":
				case "Int64":
					fieldValue = Convert.ToInt32(fieldValue);
					break;
			}

			try
			{
				switch(selFieldType)
				{
					case GDEFieldType.Bool:
						GDEDataManager.SetBool(itemName, fieldName, (bool)fieldValue);
						break;
					case GDEFieldType.BoolList:
						GDEDataManager.SetBoolList(itemName, fieldName, (List<bool>)fieldValue);
						break;
					case GDEFieldType.BoolTwoDList:
						GDEDataManager.SetBoolTwoDList(itemName, fieldName, (List<List<bool>>)fieldValue);
						break;
					case GDEFieldType.Int:
						GDEDataManager.SetInt(itemName, fieldName, (int)fieldValue);
						break;
					case GDEFieldType.IntList:
						GDEDataManager.SetIntList(itemName, fieldName, (List<int>)fieldValue);
						break;
					case GDEFieldType.IntTwoDList:
						GDEDataManager.SetIntTwoDList(itemName, fieldName, (List<List<int>>)fieldValue);
						break;
					case GDEFieldType.Float:
						GDEDataManager.SetFloat(itemName, fieldName, (float)fieldValue);
						break;
					case GDEFieldType.FloatList:
						GDEDataManager.SetFloatList(itemName, fieldName, (List<float>)fieldValue);
						break;
					case GDEFieldType.FloatTwoDList:
						GDEDataManager.SetFloatTwoDList(itemName, fieldName, (List<List<float>>)fieldValue);
						break;
					case GDEFieldType.String:
						GDEDataManager.SetString(itemName, fieldName, (string)fieldValue);
						break;
					case GDEFieldType.StringList:
						GDEDataManager.SetStringList(itemName, fieldName, (List<string>)fieldValue);
						break;
					case GDEFieldType.StringTwoDList:
						GDEDataManager.SetStringTwoDList(itemName, fieldName, (List<List<string>>)fieldValue);
						break;
					case GDEFieldType.Vector2:
						GDEDataManager.SetVector2(itemName, fieldName, (Vector2)fieldValue);
						break;
					case GDEFieldType.Vector2List:
						GDEDataManager.SetVector2List(itemName, fieldName, (List<Vector2>)fieldValue);
						break;
					case GDEFieldType.Vector2TwoDList:
						GDEDataManager.SetVector2TwoDList(itemName, fieldName, (List<List<Vector2>>)fieldValue);
						break;
					case GDEFieldType.Vector3:
						GDEDataManager.SetVector3(itemName, fieldName, (Vector3)fieldValue);
						break;
					case GDEFieldType.Vector3List:
						GDEDataManager.SetVector3List(itemName, fieldName, (List<Vector3>)fieldValue);
						break;
					case GDEFieldType.Vector3TwoDList:
						GDEDataManager.SetVector3TwoDList(itemName, fieldName, (List<List<Vector3>>)fieldValue);
						break;
					case GDEFieldType.Vector4:
						GDEDataManager.SetVector4(itemName, fieldName, (Vector4)fieldValue);
						break;
					case GDEFieldType.Vector4List:
						GDEDataManager.SetVector4List(itemName, fieldName, (List<Vector4>)fieldValue);
						break;
					case GDEFieldType.Vector4TwoDList:
						GDEDataManager.SetVector4TwoDList(itemName, fieldName, (List<List<Vector4>>)fieldValue);
						break;
					case GDEFieldType.Color:
						GDEDataManager.SetColor(itemName, fieldName, (Color)fieldValue);
						break;
					case GDEFieldType.ColorList:
						GDEDataManager.SetColorList(itemName, fieldName, (List<Color>)fieldValue);
						break;
					case GDEFieldType.ColorTwoDList:
						GDEDataManager.SetColorTwoDList(itemName, fieldName, (List<List<Color>>)fieldValue);
						break;
					case GDEFieldType.GameObject:
						GDEDataManager.SetGameObject(itemName, fieldName, (GameObject)fieldValue);
						break;
					case GDEFieldType.GameObjectList:
						GDEDataManager.SetGameObjectList(itemName, fieldName, (List<GameObject>)fieldValue);
						break;
					case GDEFieldType.GameObjectTwoDList:
						GDEDataManager.SetGameObjectTwoDList(itemName, fieldName, (List<List<GameObject>>)fieldValue);
						break;
					case GDEFieldType.Texture2D:
						GDEDataManager.SetTexture2D(itemName, fieldName, (Texture2D)fieldValue);
						break;
					case GDEFieldType.Texture2DList:
						GDEDataManager.SetTexture2DList(itemName, fieldName, (List<Texture2D>)fieldValue);
						break;
					case GDEFieldType.Texture2DTwoDList:
						GDEDataManager.SetTexture2DTwoDList(itemName, fieldName, (List<List<Texture2D>>)fieldValue);
						break;
					case GDEFieldType.Material:
						GDEDataManager.SetMaterial(itemName, fieldName, (Material)fieldValue);
						break;
					case GDEFieldType.MaterialList:
						GDEDataManager.SetMaterialList(itemName, fieldName, (List<Material>)fieldValue);
						break;
					case GDEFieldType.MaterialTwoDList:
						GDEDataManager.SetMaterialTwoDList(itemName, fieldName, (List<List<Material>>)fieldValue);
						break;
					case GDEFieldType.AudioClip:
						GDEDataManager.SetAudioClip(itemName, fieldName, (AudioClip)fieldValue);
						break;
					case GDEFieldType.AudioClipList:
						GDEDataManager.SetAudioClipList(itemName, fieldName, (List<AudioClip>)fieldValue);
						break;
					case GDEFieldType.AudioClipTwoDList:
						GDEDataManager.SetAudioClipTwoDList(itemName, fieldName, (List<List<AudioClip>>)fieldValue);
						break;
				}
			} catch(UnityException ex)
			{
				UnityEngine.Debug.LogError(string.Format(GDMConstants.ErrorSettingValue, GDMConstants.StringType, itemName, fieldName));
				UnityEngine.Debug.LogError(ex.ToString());
			}
		}
#pragma warning restore CS0618 // Type or member is obsolete

		/// <summary>
		/// Swaps the Field values of two Items.
		/// </summary>
		public static void SwapFieldValues(string firstItemName, string firstFieldName, string secondItemName,
										   string secondFieldName, bool save = true)
		{
			object firstFieldValue = GetFieldValue(firstItemName, firstFieldName);
			object secondFieldValue = GetFieldValue(secondItemName, secondFieldName);

			SetFieldValue(firstItemName, firstFieldName, secondFieldValue);
			SetFieldValue(secondItemName, secondFieldName, firstFieldValue);

			if(save) Save();
		}

		#endregion

		#region Logic
		////////////

		/// <summary>
		/// Returns the bool value by Item and Field Name.
		/// </summary>
		public static bool GetBool(string itemName, string fieldName)
		{
			CheckFieldType(itemName, fieldName, GDEFieldType.Bool);
			return (bool)GetFieldValue(itemName, fieldName);
		}

		/// <summary>
		/// Inverts a bool value and optionally returns the result.
		/// </summary>
		/// <returns></returns>
		public static bool BoolFlip(string itemName, string fieldName)
		{
			bool flippedBool = !GetBool(itemName, fieldName);
			SetFieldValue(itemName, fieldName, flippedBool, GDEFieldType.Bool);

			return flippedBool;
		}

		#endregion

		#region Strings
		///////////////

		/// <summary>
		/// Appends a string value to the given field value, either before or after it.
		/// </summary>
		/// <param name="stringToAdd">The string value to add to the field value.</param>
		/// <param name="addToEnd">Wheter to add the string to the end or front of the field value.</param>
		public static void AddString(string itemName, string fieldName, string stringToAdd, bool addToEnd = true, bool save = true)
		{
			if(!CheckFieldType(itemName, fieldName, GDEFieldType.String)) return;

			object prevValue = GetFieldValue(itemName, fieldName);

			if(addToEnd) prevValue = string.Concat(prevValue, stringToAdd);
			else prevValue = string.Concat(stringToAdd, prevValue);

			SetFieldValue(itemName, fieldName, prevValue, GDEFieldType.String);

			if(save) Save();
		}

		/// <summary>
		/// Remove a string value from the given field value.
		/// </summary>
		/// <param name="stringToAdd">The string value to add to the field value.</param>
		/// <param name="addToEnd">Wheter to add the string to the end or front of the field value.</param>
		public static void RemoveString(string itemName, string fieldName, string remove, bool addToEnd = true, bool save = true)
		{
			if(!CheckFieldType(itemName, fieldName, GDEFieldType.String)) return;

			object prevValue = GetFieldValue(itemName, fieldName);
			string prevStringValue = Convert.ToString(prevValue);

			SetFieldValue(itemName, fieldName, prevStringValue.Replace(prevStringValue, remove), GDEFieldType.String);

			if(save) Save();
		}

		#endregion

		#region Math
		////////////

		/// <summary>
		/// Apply an operation to the given Field.
		/// </summary>
		/// <param name="itemName"></param>
		/// <param name="FieldName"></param>
		/// <param name="operation">Wheter to add, subtract, multiply or divide the Field value.</param>
		/// <param name="byValue"></param>
		public static void GDEOperator(string itemName, string fieldName, GDEFieldType fieldType,
									   GDEOperation operation, object byValue)
		{
			if(!CheckFieldType(itemName, fieldName, fieldType)) return;

			object prevValue = GetFieldValue(itemName, fieldName);

			switch(fieldType)
			{
				case GDEFieldType.Int:
					int tmpIntValue = Convert.ToInt32(prevValue);
					int operateIntBy = Convert.ToInt32(byValue);

					switch(operation)
					{
						case GDEOperation.Add:
							tmpIntValue += operateIntBy;
							break;
						case GDEOperation.Subtract:
							tmpIntValue -= operateIntBy;
							break;
						case GDEOperation.Multiply:
							tmpIntValue *= operateIntBy;
							break;
						case GDEOperation.Divide:
							tmpIntValue /= operateIntBy;
							break;
					}

					prevValue = tmpIntValue;
					break;
				case GDEFieldType.Float:
					float tmpFloatValue = Convert.ToSingle(prevValue);
					float operateFloatBy = Convert.ToSingle(byValue);

					switch(operation)
					{
						case GDEOperation.Add:
							tmpFloatValue += operateFloatBy;
							break;
						case GDEOperation.Subtract:
							tmpFloatValue -= operateFloatBy;
							break;
						case GDEOperation.Multiply:
							tmpFloatValue *= operateFloatBy;
							break;
						case GDEOperation.Divide:
							tmpFloatValue /= operateFloatBy;
							break;
					}

					prevValue = tmpFloatValue;
					break;
				case GDEFieldType.Vector2:
					Vector2 tmpV2Value = (Vector2)prevValue;
					Vector2 operateV2By = (Vector2)byValue;

					switch(operation)
					{
						case GDEOperation.Add:
							tmpV2Value += operateV2By;
							break;
						case GDEOperation.Subtract:
							tmpV2Value -= operateV2By;
							break;
						case GDEOperation.Multiply:
							var multResult = Vector2.zero;
							multResult.x = tmpV2Value.x * operateV2By.x;
							multResult.y = tmpV2Value.y * operateV2By.y;
							tmpV2Value = multResult;
							break;
						case GDEOperation.Divide:
							var divResult = Vector2.zero;
							divResult.x = tmpV2Value.x / operateV2By.x;
							divResult.y = tmpV2Value.y / operateV2By.y;
							tmpV2Value = divResult;
							break;
					}

					prevValue = tmpV2Value;
					break;
				case GDEFieldType.Vector3:
					Vector3 tmpV3Value = (Vector3)prevValue;
					Vector3 operateV3By = (Vector3)byValue;

					switch(operation)
					{
						case GDEOperation.Add:
							tmpV3Value += operateV3By;
							break;
						case GDEOperation.Subtract:
							tmpV3Value -= operateV3By;
							break;
						case GDEOperation.Multiply:
							var multResult = Vector3.zero;
							multResult.x = tmpV3Value.x * operateV3By.x;
							multResult.y = tmpV3Value.y * operateV3By.y;
							multResult.z = tmpV3Value.z * operateV3By.z;
							tmpV3Value = multResult;
							break;
						case GDEOperation.Divide:
							var divResult = Vector3.zero;
							divResult.x = tmpV3Value.x / operateV3By.x;
							divResult.y = tmpV3Value.y / operateV3By.y;
							divResult.z = tmpV3Value.z / operateV3By.z;
							tmpV3Value = divResult;
							break;
					}

					prevValue = tmpV3Value;
					break;
				case GDEFieldType.Vector4:
					Vector4 tmpV4Value = (Vector4)prevValue;
					Vector4 operateV4By = (Vector4)byValue;

					switch(operation)
					{
						case GDEOperation.Add:
							tmpV4Value += operateV4By;
							break;
						case GDEOperation.Subtract:
							tmpV4Value -= operateV4By;
							break;
						case GDEOperation.Multiply:
							var multResult = Vector4.zero;
							multResult.w = tmpV4Value.w * operateV4By.w;
							multResult.x = tmpV4Value.x * operateV4By.x;
							multResult.y = tmpV4Value.y * operateV4By.y;
							multResult.z = tmpV4Value.z * operateV4By.z;
							tmpV4Value = multResult;
							break;
						case GDEOperation.Divide:
							var divResult = Vector4.zero;
							divResult.w = tmpV4Value.w / operateV4By.w;
							divResult.x = tmpV4Value.x / operateV4By.x;
							divResult.y = tmpV4Value.y / operateV4By.y;
							divResult.z = tmpV4Value.z / operateV4By.z;
							tmpV4Value = divResult;
							break;
					}

					prevValue = tmpV4Value;
					break;
				default:
					UnityEngine.Debug.LogError("Unsorted Field Type!");
					break;
			}

			SetFieldValue(itemName, fieldName, prevValue, fieldType);
		}

		#endregion

		#region Extensions
		//////////////////

		/// <summary>
		/// Adds an item to a list if the list doesn't already contain it.
		/// </summary>
		private static void AddUnique<T>(this List<T> list, T item)
		{
			if(!list.Contains(item)) list.Add(item);
		}

		/// <summary>
		/// Converts a list of any type to a list of strings a.k.a. extracts the names of all entries.
		/// </summary>
		public static List<string> ToStringList<T>(this List<T> list)
		{
			List<string> strList = new List<string>();

			foreach(var entry in list)
			{
				strList.Add(entry.ToString());
			}

			return strList;
		}

		/// <summary>
		/// Compares two lists and returns all items that they both contain.
		/// </summary>
		/// <param name="compare">Compare each entry with the entries of this list.</param>
		/// <returns></returns>
		public static List<T> Matches<T>(this List<T> list, List<T> compare)
		{
			List<T> result = new List<T>();

			foreach(var item in list)
			{
				if(compare.Contains(item)) result.AddUnique(item);
			}

			return result;
		}

		/// <summary>
		/// Tries to remove an entry from a dictionary.
		/// </summary>
		/// <param name="variable">A dictionary of Key-Value-pairs.</param>
		/// <param name="key">The entry to remove.</param>
		/// <returns>Wheter the removal was successful or if there were any errors such as missing entry.</returns>
		public static bool TryRemoveValue<TKey, TValue>(this Dictionary<TKey, TValue> variable, TKey key)
		{
			bool result;
			try
			{
				variable.Remove(key);
				result = true;
			} catch
			{
				result = false;
			}
			return result;
		}

		#endregion

		#region ErrorHandling

		/// <summary>
		/// Check wheter the type of the field to access is actually of the same type as the given value.
		/// </summary>
		/// <param name="supposedType">What type the field should be.</param>
		/// <returns></returns>
		public static bool CheckFieldType(string itemName, string fieldName, GDEFieldType supposedType, string specifySchema = "")
		{
			GDEFieldType fieldType = GetFieldType(itemName, fieldName, specifySchema);

			if(fieldType != supposedType)
			{
				UnityEngine.Debug.LogError("Field \"" + fieldName + "\"(" + fieldType.ToString()
											+ ") isn't of type " + supposedType.ToString() + "!");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Check wheter GDE contains the given Item name.
		/// Optionally specify the Schema where the Item should be in to narrow it down.
		/// </summary>
		public static bool CheckHasItem(string itemName, string schema = "",
										bool throwTrueError = false, bool throwFalseError = true)
		{
			string source = string.IsNullOrEmpty(schema) ? "GDE" : "Schema " + schema;

			if(!HasItem(itemName, schema))
			{
				if(throwFalseError) UnityEngine.Debug.LogError(source
									+ " doesn't contain the specified Item \"" + itemName + "\"!");
				return false;
			} else
			{
				if(throwTrueError) UnityEngine.Debug.LogError(source
								   + " already contains the Item \"" + itemName + "\"!");
			}

			return true;
		}

		/// <summary>
		/// Check wheter GDE contains the given Item name.
		/// Optionally specify the Schema where the Item should be in to narrow it down.
		/// </summary>
		public static bool CheckHasField(string fieldName, string schema = "",
										bool throwTrueError = false, bool throwFalseError = true)
		{
			string source = string.IsNullOrEmpty(schema) ? "GDE" : "Schema " + schema;

			if(!HasField(fieldName, schema))
			{
				if(throwFalseError) UnityEngine.Debug.LogError(source + " doesn't contain the specified Field \"" + fieldName + "\"!");
				return false;
			} else
			{
				if(throwTrueError) UnityEngine.Debug.LogError(source + " already contains the Field \"" + fieldName + "\"!");
			}

			return true;
		}

		#endregion

		#region Misc
		////////////

		/// <summary>
		/// Tries to parse a supported variable type like "String" into 
		/// the matching GDEFieldType (in this case GDEFieldType.String).
		/// </summary>
		public static GDEFieldType ParseFieldType(string strToParse)
		{
			return (GDEFieldType)Enum.Parse(typeof(GDEFieldType), strToParse, true);
		}

		/// <summary>
		/// Tries to parse the type of an object to its GDEFieldType.
		/// Also converts the type to an GDE-friendly/-supported type if necessary.
		/// </summary>
		public static GDEFieldType ParseFieldValueType(object fieldValue)
		{
			string strToParse = fieldValue.GetType().ToString();
			strToParse = strToParse.Replace("System.", "");

			switch(strToParse)
			{
				case "Int16":
				case "Int32":
				case "Int64":
					strToParse = "Int";
					break;
			}

			return ParseFieldType(strToParse);
		}

		/// <summary>
		/// Only parse the object type if no GDEFieldType was provided.
		/// </summary>
		public static GDEFieldType ParseFieldValueTypeIfNone(object fieldValue, GDEFieldType fieldType = GDEFieldType.None)
		{
			return fieldType != GDEFieldType.None ? fieldType : ParseFieldValueType(fieldValue);
		}

		/// <summary>
		/// Checks if a FieldName contains a substring that indicates that it's not part of actual user data.
		/// </summary>
		public static bool IsRelevant(string fieldName, bool containsType = false, bool containsSchema = false)
		{
			if(fieldName.Contains("_gdeType_") == containsType
				&& fieldName.Contains("_gdeSchema") == containsSchema) return true;

			return false;
		}

		/// <summary>
		/// Checks if the DataSet contains a specific field type.
		/// </summary>
		public static bool DataSetContainsFieldType(Dictionary<string, object> currentDataSet, string fieldType)
		{
			foreach(var subPair in currentDataSet)
			{
				if(!IsRelevant(fieldType, true)) continue;
				if(subPair.Key == fieldType) return true;
			}

			return false;
		}

		/// <summary>
		/// Checks if the DataSet contains a specific field name.
		/// </summary>
		public static bool DataSetContainsFieldName(Dictionary<string, object> currentDataSet, string fieldName)
		{
			foreach(var subPair in currentDataSet)
			{
				if(!IsRelevant(fieldName)) continue;
				if(subPair.Key == fieldName) return true;
			}

			return false;
		}

		/// <summary>
		/// Checks if the DataSet contains a specific field value.
		/// </summary>
		public static bool DataSetContainsFieldValue(Dictionary<string, object> currentDataSet, object fieldValue)
		{
			foreach(var subPair in currentDataSet)
			{
				if(!IsRelevant(fieldValue.ToString())) continue;
				if(subPair.Value == fieldValue) return true;
			}

			return false;
		}

		#endregion

		#region Debug

		/// <summary>
		/// Throws neatly formatted logs to display all data of the given dictionary type.
		/// </summary>
		public static void DebugDictionary(GDEDictionary dictionary)
		{
			Dictionary<string, object> selDict = new Dictionary<string, object>();

			switch(dictionary)
			{
				case GDEDictionary.DataDictionary:
					selDict = GDEDataManager.DataDictionary;
					break;
				case GDEDictionary.ModifiedData:
					selDict = ConvertModifiedData();
					break;
				case GDEDictionary.Both:
					selDict = GetAllGDEData();
					break;
			}

			foreach(var pair in selDict)
			{
				UnityEngine.Debug.Log("Key: " + pair.Key);

				if(pair.Value.IsGenericDictionary())
				{
					foreach(var valPair in pair.Value as Dictionary<string, object>)
					{
						UnityEngine.Debug.Log("    SubKey: " + valPair.Key);
						UnityEngine.Debug.Log("    SubValue: " + valPair.Value);
					}
				} else
				{
					UnityEngine.Debug.Log("Value: " + pair.Value.ToString());
				}
			}
		}

		#endregion
	}

#endif
}
