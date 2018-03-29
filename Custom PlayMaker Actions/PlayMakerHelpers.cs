
/* Notes:
 * Any FsmVariable like FsmString, FsmBool, ... derives from NamedVariable, thus can be generalized by that
 */

using HutongGames.PlayMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iDecay.PlayMaker
{
	#region enums

	//makes the 'everyFrame' bool obsolete when allowing to select the updateType
	public enum FsmUpdateType
	{
		Once,
		OnUpdate,
		OnLateUpdate,
		OnFixedUpdate
	}

	#endregion

	/// <summary>
	/// A collection of helper functions and extensions useful for creating custom actions.
	/// </summary>
	public static class PlayMakerHelpers
	{
		#region General

		public static bool IsSet(this FsmString str)
		{
			return string.IsNullOrEmpty(str.Value) || str.IsNone;
		}

		#endregion

		#region FsmArray

		public static void Add<T>(this FsmArray array, T entry)
		{
			if(array == null) return;

			array.SetType(entry.GetVariableType());

			if(entry != null)
			{
				array.Resize(array.Length + 1);
				array.Set(array.Length - 1, (object)entry);
			} else
				array.Resize(0);

			array.SaveChanges();
		}

		#endregion

		#region VariableType

		/// <summary>
		/// Tries to parse a supported variable type like "String" into 
		/// the matching NamedVariable (in this case NamedVariable.String).
		/// </summary>
		public static VariableType ParseVariableType(string strToParse)
		{
			return (VariableType)Enum.Parse(typeof(VariableType), strToParse, true);
		}

		/// <summary>
		/// Tries to parse the type of an object to its VariableType.
		/// </summary>
		public static VariableType ParseToVariableType(this object obj)
		{
			string strToParse = obj.GetType().ToString();
			strToParse = strToParse.Replace("System.", "");

			switch(strToParse)
			{
				case "Int16":
				case "Int32":
				case "Int64":
					strToParse = "Int";
					break;
				case "Object":
					strToParse = "Unknown";
					break;
			}

			return ParseVariableType(strToParse);
		}

		public static VariableType GetVariableType<T>(this T var)
		{
			Type type = var.GetType();

			if(type.Equals(typeof(int)))
				return VariableType.Int;
			else if(type.Equals(typeof(float)))
				return VariableType.Float;
			else if(type.Equals(typeof(bool)))
				return VariableType.Bool;
			else if(type.Equals(typeof(string)))
				return VariableType.String;
			else if(type.Equals(typeof(Color)))
				return VariableType.Color;
			else if(type.Equals(typeof(GameObject)))
				return VariableType.GameObject;
			else if(type.Equals(typeof(Material)))
				return VariableType.Material;
			else if(type.Equals(typeof(Vector2)))
				return VariableType.Vector2;
			else if(type.Equals(typeof(Vector3)))
				return VariableType.Vector3;
			else
				return VariableType.Object;
		}

		#endregion

		#region Converter
		/////////////////

		/******************************
		********** FsmString **********
		******************************/

		/// <summary>
		/// Converts an FsmString-Array to a list.
		/// </summary>
		public static List<string> ToList(this FsmString[] fsmString)
		{
			List<string> result = new List<string>();

			for(int i = 0; i < fsmString.Length; i++)
				result.Add(fsmString[i].Value);

			return result;
		}

		/// <summary>
		/// Converts an FsmString-Array to an Array.
		/// </summary>
		public static string[] ToArray(this FsmString[] fsmString)
		{
			string[] result = new string[fsmString.Length];

			for(int i = 0; i < fsmString.Length; i++)
				result[i] = fsmString[i].Value;

			return result;
		}

		/******************************
		************ FsmVar ***********
		******************************/

		/// <summary>
		/// Converts an FsmVar-Array to a list.
		/// </summary>
		public static List<object> ToList(this FsmVar[] fsmVar)
		{
			List<object> result = new List<object>();

			for(int i = 0; i < fsmVar.Length; i++)
				result.Add(fsmVar[i].GetValue());

			return result;
		}

		/// <summary>
		/// Converts an FsmVar-Array to an Array.
		/// </summary>
		public static object[] ToArray(this FsmVar[] fsmVar)
		{
			object[] result = new object[fsmVar.Length];

			for(int i = 0; i < fsmVar.Length; i++)
				result[i] = fsmVar[i].GetValue();

			return result;
		}

		#endregion

		#region Extensions

		public static GameObject GetOwner(this FsmOwnerDefault odt)
		{
			//Fsm.GetOwnerDefaultTarget() unfortunately requires to derive 
			//from FsmStateAction which is not possible here; so try it this way instead:
			GameObject go = odt.GameObject.Value;

			if(!go) UnityEngine.Debug.LogError("GameObject is null!");

			return go;
		}

		#endregion
	}
}
