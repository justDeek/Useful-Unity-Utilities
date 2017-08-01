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
		[RequiredField]
		[Tooltip("Specify the existing Schema this Item should belong to.")]
		[UIHint(UIHint.FsmString)]
		public FsmString schema;

		[RequiredField]
		[UIHint(UIHint.FsmString)]
		[Tooltip("The name of the variable in the target FSM.")]
		public FsmString ItemName;

		[UIHint(UIHint.FsmString)]
		[Tooltip("The name of the variable in the target FSM.")]
		public FsmString FieldName;

		[Tooltip("Optionally apply the desired value to the created Item under the specified Field-Name.")]
		public FsmVar setValue;

		[Tooltip("Should be saved afterwards? If not you can still save later, otherwise changes will be discarded when restarting the game/project.")]
		public FsmBool save;

		bool doesSchemaExist = false;
		bool containsFieldName = false;
		string currentSchema = "";
		string typeOfField = "";

		public override void Reset()
		{
			schema = null;
			setValue = new FsmVar();
			save = true;
		}

		public override void OnEnter()
		{
			GoThroughAllData();

			if (!doesSchemaExist) {
				UnityEngine.Debug.LogError("Schema doesn't exist!");
				return;
			}

			try
			{
				GDEDataManager.RegisterItem(schema.Value, ItemName.Value);
				if (!setValue.IsNone || !string.IsNullOrEmpty(FieldName.Value)) {
					if (!containsFieldName) {
						UnityEngine.Debug.LogError("Schema doesn't contain the specified Field Name!");
						return;
					}
					if (setValue.Type.ToString() != typeOfField) {
						UnityEngine.Debug.LogError("The specified Field Name is of type \"" + typeOfField + "\" and doesn't match Type \"" + setValue.Type.ToString() + "\"!");
						return;
					}
					switch (setValue.Type.ToString()) {
						case "Int":
							GDEDataManager.SetInt(ItemName.Value, FieldName.Value, setValue.intValue);
						break;
						case "Float":
							GDEDataManager.SetFloat(ItemName.Value, FieldName.Value, setValue.floatValue);
						break;
						case "Bool":
							GDEDataManager.SetBool(ItemName.Value, FieldName.Value, setValue.boolValue);
						break;
						case "GameObject":
							GDEDataManager.SetGameObject(ItemName.Value, FieldName.Value, setValue.gameObjectValue);
						break;
						case "String":
							GDEDataManager.SetString(ItemName.Value, FieldName.Value, setValue.stringValue);
						break;
						case "Vector2":
							GDEDataManager.SetVector2(ItemName.Value, FieldName.Value, setValue.vector2Value);
						break;
						case "Vector3":
							GDEDataManager.SetVector3(ItemName.Value, FieldName.Value, setValue.vector3Value);
						break;
						case "Vector4":
							GDEDataManager.SetVector4(ItemName.Value, FieldName.Value, setValue.vector4Value);
						break;
						case "Color":
							GDEDataManager.SetColor(ItemName.Value, FieldName.Value, setValue.colorValue);
						break;
						case "Texture":
							Texture2D tmpTexture = setValue.textureValue as Texture2D;
							GDEDataManager.SetTexture2D(ItemName.Value, FieldName.Value, tmpTexture);
						break;
						case "Material":
							GDEDataManager.SetMaterial(ItemName.Value, FieldName.Value, setValue.materialValue);
						break;
						case "Object":
							AudioClip tmpAudio = setValue.objectReference as AudioClip;
							GDEDataManager.SetAudioClip(ItemName.Value, FieldName.Value, tmpAudio);
						break;
					}
				}
				//save option
				if (save.Value)
					GDEDataManager.Save();
					// #if UNITY_EDITOR
					// GDEDataManager.SaveToDisk(); //for debugging purposes
					// #endif
			}
			catch(UnityException ex)
			{
				LogError(string.Format(GDMConstants.ErrorSettingValue, GDMConstants.StringType, ItemName.Value, FieldName.Value));
				LogError(ex.ToString());
			}
			finally
			{
				Finish();
			}
		}

		public void GoThroughAllData() {
			foreach(KeyValuePair<string, object> pair in GDEDataManager.DataDictionary)
			{
				if (pair.Key.StartsWith(GDMConstants.SchemaPrefix))
						continue;

				Dictionary<string, object> currentDataSet = pair.Value as Dictionary<string, object>;

				currentDataSet.TryGetString(GDMConstants.SchemaKey, out currentSchema);
				//skip all Data that is not in the specified Schema
				if (currentSchema != schema.Value) {
					continue;
				} else {
					doesSchemaExist = true;
				}
				//go through all Values to check if FieldName is prevalent & get its Type
				foreach (var subPair in currentDataSet) {
					if (subPair.Key.ToString().Equals(string.Concat("_gdeType_", FieldName.Value))) {
						containsFieldName = true;
						typeOfField = subPair.Value.ToString();
						typeOfField = typeOfField.UppercaseFirst();
					}
				}
			}
		}

		#if UNITY_EDITOR
		public override string AutoName()
		{
			return ("Set FSM Variable: " + ActionHelpers.GetValueLabel(FieldName));
		}
		#endif
	}
}

#endif
