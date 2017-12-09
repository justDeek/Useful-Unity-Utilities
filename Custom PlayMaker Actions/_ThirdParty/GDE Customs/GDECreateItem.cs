using System;
using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Create an Item in the specified Schema and optionally set it's Value.")]
	public class GDECreateItem : FsmStateAction
	{
		//[RequiredField]
		[Tooltip("Specify the existing Schema this Item should belong to.")]
		private FsmString schema = "";

		//[RequiredField]
		//[Tooltip("The name of the variable in the target FSM.")]
		private FsmString ItemName = "";

		[CompoundArray("Key Amount", "Field Name", "Set Value")]

		[Tooltip("The name of the variable in the target FSM.")]
		public FsmString[] FieldName = new FsmString[0];

		[Tooltip("Optionally apply the desired value to the created Item under the specified Field-Name.")]
		public FsmVar[] setValue = new FsmVar[0];

		[Tooltip("Should be saved afterwards? If not you can still save later, otherwise changes will be discarded when restarting the game/project.")]
		public FsmBool save;

		bool doesSchemaExist;
		bool containsFieldName;
		string currentSchema = "";
		string[] typeOfField;

		public FsmString Schema
		{
			get
			{
				return schema;
			}

			set
			{
				schema = value;
			}
		}

		public FsmString ItemName1
		{
			get
			{
				return ItemName;
			}

			set
			{
				ItemName = value;
			}
		}

		public override void Reset()
		{
			schema = null;
			save = true;
		}

		public override void OnEnter()
		{
			typeOfField = new string[FieldName.Length];
			GoThroughAllData();
			UnityEngine.Debug.Log(Schema.Value);
			UnityEngine.Debug.Log(schema.Value);
			if(!doesSchemaExist)
			{
				UnityEngine.Debug.LogError("Schema doesn't exist!");
				return;
			}

			try
			{
				GDEDataManager.RegisterItem(schema.Value, ItemName.Value);
				for(int i = 0; i < FieldName.Length; i++)
				{
					setValue[i].UpdateValue(); //(See PlayMaker Documentation "Using FSM Variables")
					if(!setValue[i].IsNone || !string.IsNullOrEmpty(FieldName[i].Value))
					{
						if(!containsFieldName)
						{
							UnityEngine.Debug.LogError("Schema doesn't contain the specified Field Name!");
							return;
						}
						if(setValue[i].Type.ToString() != typeOfField[i])
						{
							UnityEngine.Debug.LogError("The specified Field Name \"" + FieldName[i].Value + "\" is of type \"" + typeOfField + "\" and doesn't match Type \"" + setValue[i].Type.ToString() + "\"!");
							return;
						}
						switch(setValue[i].Type.ToString())
						{
							case "Int":
								GDEDataManager.SetInt(ItemName.Value, FieldName[i].Value, setValue[i].intValue);
								break;
							case "Float":
								GDEDataManager.SetFloat(ItemName.Value, FieldName[i].Value, setValue[i].floatValue);
								break;
							case "Bool":
								GDEDataManager.SetBool(ItemName.Value, FieldName[i].Value, setValue[i].boolValue);
								break;
							case "GameObject":
								GDEDataManager.SetGameObject(ItemName.Value, FieldName[i].Value, setValue[i].gameObjectValue);
								break;
							case "String":
								GDEDataManager.SetString(ItemName.Value, FieldName[i].Value, setValue[i].stringValue);
								break;
							case "Vector2":
								GDEDataManager.SetVector2(ItemName.Value, FieldName[i].Value, setValue[i].vector2Value);
								break;
							case "Vector3":
								GDEDataManager.SetVector3(ItemName.Value, FieldName[i].Value, setValue[i].vector3Value);
								break;
							case "Vector4":
								GDEDataManager.SetVector4(ItemName.Value, FieldName[i].Value, setValue[i].vector4Value);
								break;
							case "Color":
								GDEDataManager.SetColor(ItemName.Value, FieldName[i].Value, setValue[i].colorValue);
								break;
							case "Texture":
								Texture2D tmpTexture = setValue[i].textureValue as Texture2D;
								GDEDataManager.SetTexture2D(ItemName.Value, FieldName[i].Value, tmpTexture);
								break;
							case "Material":
								GDEDataManager.SetMaterial(ItemName.Value, FieldName[i].Value, setValue[i].materialValue);
								break;
							case "Object":
								AudioClip tmpAudio = setValue[i].objectReference as AudioClip;
								GDEDataManager.SetAudioClip(ItemName.Value, FieldName[i].Value, tmpAudio);
								break;
						}
					}
				}

				//save option
				if(save.Value)
					GDEDataManager.Save();
				// #if UNITY_EDITOR
				// GDEDataManager.SaveToDisk(); //for debugging purposes
				// #endif
			} catch(UnityException ex)
			{
				for(int i = 0; i < FieldName.Length; i++)
				{
					LogError(string.Format(GDMConstants.ErrorSettingValue, GDMConstants.StringType, ItemName.Value, FieldName[i].Value));
				}
				LogError(ex.ToString());
			} finally
			{
				Finish();
			}
		}

		public void GoThroughAllData()
		{
			foreach(KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
			{
				if(pair.Key.StartsWith(GDMConstants.SchemaPrefix))
					continue;

				Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;

				currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
				//skip all Data that is not in the specified Schema
				if(currentSchema != schema.Value)
				{
					continue;
				} else
				{
					doesSchemaExist = true;
				}
				//go through all Values to check if FieldName is prevalent & get its Type
				foreach(var subPair in currentDataSet)
				{
					for(int i = 0; i < FieldName.Length; i++)
					{
						if(subPair.Key.ToString().Equals(string.Concat("_gdeType_", FieldName[i].Value)))
						{
							containsFieldName = true;
							typeOfField[i] = subPair.Value.ToString();
							typeOfField[i] = typeOfField[i].UppercaseFirst();
						}
					}
				}
			}
		}

#if UNITY_EDITOR
		public override string AutoName()
		{
			for(int i = 0; i < FieldName.Length; i++)
			{
				return ("Set FSM Variable: " + ActionHelpers.GetValueLabel(FieldName[i]));
			}
			return null;
		}
#endif
	}
}

#endif
