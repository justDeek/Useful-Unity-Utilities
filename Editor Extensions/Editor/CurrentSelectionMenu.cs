using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.Linq;
using System.Collections.Generic;

public class CurrentSelectionMenu : MonoBehaviour
{
	//container for current selection
	static GameObject[] instance;

	static UnityEngine.Object[] clones;

	//Validation for all selection-based options (Only enabled these options if something is selected in hirarchy)
	[MenuItem("Current Selection/Clone/Clone selected GameObject %#d", true)]
	[MenuItem("Current Selection/Rename selected GameObjects incrementally %#r", true)]
	[MenuItem("Current Selection/Parent/Select Parent &UP", true)]
	[MenuItem("Current Selection/Parent/Parent: Select all Children &DOWN", true)]
	[MenuItem("Current Selection/Parent/Parent: Sort Children by Name", true)]
	[MenuItem("Current Selection/Parent/Parent: Sort Children by Number", true)]
	[MenuItem("Current Selection/Select previous GameObject #UP", true)]
	[MenuItem("Current Selection/Select next GameObject #DOWN", true)]
	[MenuItem("Current Selection/Get Info %i", true)]
	[MenuItem("Current Selection/Prefabs/Revert To Prefab %&y", true)]
	[MenuItem("Current Selection/Prefabs/Apply Prefab Changes %#y", true)]
	[MenuItem("Current Selection/Prefabs/Disconnect From Prefab %#&y", true)]
	[MenuItem("Current Selection/Transform/Center Children", true)]
	[MenuItem("Current Selection/Transform/Center Transform To Collider", true)]
	private static bool GeneralValidation()
	{
		//Disable Options if nothing is selected
		return Selection.activeGameObject != null;
	}

	private static void Finish()
	{
		//Mark any active Scene as dirty to show that changes 
		//were made and that the user is able to save those
		EditorSceneManager.MarkAllScenesDirty();
	}

	/*******************************************
	 *                Cloning                  *
	 *******************************************/

	[MenuItem("Current Selection/Clone/Clone selected GameObject %#d")]
	public static void Clone()
	{
		instance = Selection.gameObjects;
		clones = new GameObject[instance.Length];
		Transform initRoot = Selection.activeTransform.parent;

		//clone the selection
		for(int i = 0; i < instance.Length; i++)
		{
			UnityEngine.Object root = PrefabUtility.GetPrefabParent(instance[i]);
			GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(root);
			if(root != null)
			{
				clone.transform.position = instance[i].transform.position;
				clone.transform.rotation = instance[i].transform.rotation;
			} else //If you try to duplicate something other than a prefab
			{
				clone = Instantiate(instance[i], instance[i].transform.position, instance[i].transform.rotation) as GameObject;
			}

			clone.name = instance[i].name;

			clones[i] = clone;
			Selection.objects = clones;

		}

		//reparent and reset Transform
		foreach(GameObject obj in Selection.gameObjects)
		{
			obj.transform.parent = initRoot;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
		}

		Finish();
	}

	[MenuItem("Current Selection/Clone/Undo Cloning %#z", true)]
	private static bool UndoCloneValidation()
	{
		return clones != null;
	}

	[MenuItem("Current Selection/Clone/Undo Cloning %#z")]
	public static void UndoClone()
	{
		if(clones != null)
		{
			for(int i = 0; i < clones.Length; i++)
			{
				GameObject temp = (GameObject)clones[i];
				DestroyImmediate(temp);
			}
			clones = null;
		}

		Finish();
	}

	[MenuItem("Current Selection/Rename selected GameObjects incrementally %#r")]
	public static void Rename()
	{
		instance = Selection.gameObjects;
		int j = 0;

		for(int i = 0; i < instance.Length; i++)
		{
			j = i + 1;
			//if (j <= 9)
			//{
			//	instance[i].name = instance[i].name + "0" + j;
			//}
			//else
			//{
			instance[i].name = instance[i].name + j;
			//}
		}

		Finish();
	}

	[MenuItem("Current Selection/Select previous GameObject #UP")]
	public static void SelectpreviousGO()
	{
		List<UnityEngine.Object> currentSelection = new List<UnityEngine.Object> { };
		UnityEngine.Object[] tmpArray = new UnityEngine.Object[] { };
		Transform selectionParent = Selection.activeTransform.parent;
		int parentChildCount = selectionParent.childCount;

		try
		{
			for(int i = 0; i < parentChildCount; i++)
			{
				if(selectionParent.GetChild(i) == Selection.activeTransform)
				{
					currentSelection.Add((UnityEngine.Object)selectionParent.GetChild(i - 1).gameObject);
				}
			}
		} catch(System.Exception e)
		{
			UnityEngine.Debug.LogWarning("There are no more GameObject before this one: " + e.ToString());
			currentSelection.Add((UnityEngine.Object)Selection.activeTransform.gameObject);
		}

		tmpArray = currentSelection.ToArray();
		if(tmpArray != null)
			Selection.objects = tmpArray;
	}

	[MenuItem("Current Selection/Select next GameObject #DOWN")]
	public static void SelectNextGO()
	{
		List<UnityEngine.Object> currentSelection = new List<UnityEngine.Object> { };
		UnityEngine.Object[] tmpArray = new UnityEngine.Object[] { };
		Transform selectionParent = Selection.activeTransform.parent;
		int parentChildCount = selectionParent.childCount;

		try
		{
			for(int i = 0; i <= parentChildCount - 1; i++)
			{
				if(selectionParent.GetChild(i) == Selection.activeTransform)
				{
					currentSelection.Add((UnityEngine.Object)selectionParent.GetChild(i + 1).gameObject);
				}
			}
		} catch(System.Exception e)
		{
			UnityEngine.Debug.LogWarning("There is no more GameObject after this one: " + e.ToString());
			currentSelection.Add((UnityEngine.Object)Selection.activeTransform.gameObject);
		}

		tmpArray = currentSelection.ToArray();
		if(tmpArray != null)
			Selection.objects = tmpArray;
	}

	[MenuItem("Current Selection/Get Info %i")]
	public static void GetInfo()
	{
		UnityEngine.Debug.Log("Selected Amount: " + Selection.gameObjects.Length);
	}

	/*******************************************
	 *                 Parent                  *
	 *******************************************/

	[MenuItem("Current Selection/Parent/Select Parent &UP")]
	public static void SelectParent()
	{
		List<UnityEngine.Object> currentSelection = new List<UnityEngine.Object> { };
		UnityEngine.Object[] tmpArray = new UnityEngine.Object[] { };
		//Try to add parent to List, otherwise add current selection
		try
		{
			currentSelection.Add((UnityEngine.Object)Selection.activeTransform.parent.gameObject);
		} catch(System.Exception e)
		{
			UnityEngine.Debug.LogWarning("The selected GameObject has no parent. Caught the following Error: " + e.ToString());
			currentSelection.Add((UnityEngine.Object)Selection.activeTransform.gameObject);
		}

		tmpArray = currentSelection.ToArray();
		if(tmpArray != null)
			Selection.objects = tmpArray;
	}

	[MenuItem("Current Selection/Parent/Parent: Select all Children &DOWN")]
	public static void SelectAllChildren()
	{
		List<UnityEngine.Object> childrenArray = new List<UnityEngine.Object> { };
		UnityEngine.Object[] tmpArray = new UnityEngine.Object[] { };
		//Get Child amount
		int children = Selection.activeTransform.childCount;

		//iterate through children
		for(int i = 0; i < children; ++i)
		{
			//Add next child to List
			childrenArray.Add((UnityEngine.Object)Selection.activeTransform.GetChild(i).gameObject);
			//convert List to Array
			tmpArray = childrenArray.ToArray();
		}

		//Assign all children to Selection
		Selection.objects = tmpArray;

	}

	[MenuItem("Current Selection/Parent/Parent: Sort Children by Name")]
	public static void SortChildrenByName()
	{
		foreach(GameObject obj in Selection.gameObjects)
		{
			List<Transform> children = new List<Transform>();

			for(int i = obj.transform.childCount - 1; i >= 0; i--)
			{
				Transform child = obj.transform.GetChild(i);
				children.Add(child);
				child.parent = null;
			}

			children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });

			foreach(Transform child in children)
			{
				child.parent = obj.transform;
			}
		}

		Finish();
	}

	[MenuItem("Current Selection/Parent/Parent: Sort Children by Number")]
	public static void SortChildrenByNumber()
	{
		foreach(GameObject obj in Selection.gameObjects)
		{
			List<Transform> children = new List<Transform>();
			List<int> numbers = new List<int>();

			for(int i = obj.transform.childCount - 1; i >= 0; i--)
			{
				Transform child = obj.transform.GetChild(i);
				child.parent = null;
				//collect children
				children.Add(child);
				//collect name numbers
				numbers.Add(int.Parse(new String(child.name.Where(Char.IsDigit).ToArray())));
			}

			//sort name numbers numerically
			numbers.Sort();

			for(int i = 0; i < numbers.Count; i++)
			{
				int currID = numbers[i];

				foreach(Transform child in children)
				{
					//get current name number
					int currNum = int.Parse(new String(child.name.Where(Char.IsDigit).ToArray()));

					if(currNum == currID)
					{
						child.parent = obj.transform;
						continue;
					}
				}
			}
		}

		Finish();
	}

	/*******************************************
	 *                Prefabs                  *
	 *******************************************/

	[MenuItem("Current Selection/Prefabs/Revert To Prefab %&y")]
	static public void RevertToPrefab()
	{
		instance = Selection.gameObjects;

		for(int i = 0; i < instance.Length; i++)
		{
			//works even better than the 'Revert'-Button in the Inspector,
			//in that this also remains saved references to components from other GameObjects)
			GameObject root = PrefabUtility.FindPrefabRoot(instance[i]);
			GameObject source = (GameObject)PrefabUtility.GetPrefabParent(root);

			if(source != null)
			{
				PrefabUtility.ConnectGameObjectToPrefab(root, source);
				UnityEngine.Debug.Log(instance[i].name + " reverted to Prefab.");
			} else
			{
				UnityEngine.Debug.LogWarning("Selection has no prefab");
			}
		}

		Finish();
	}

	[MenuItem("Current Selection/Prefabs/Apply Prefab Changes %#y")]
	static public void ApplyPrefabChanges()
	{
		instance = Selection.gameObjects;

		for(int i = 0; i < instance.Length; i++)
		{
			GameObject root = PrefabUtility.FindPrefabRoot(instance[i]);
			UnityEngine.Object source = PrefabUtility.GetPrefabParent(root);

			if(source != null)
			{
				PrefabUtility.ReplacePrefab(root, source, ReplacePrefabOptions.ConnectToPrefab);
				UnityEngine.Debug.Log("Updating prefab : " + AssetDatabase.GetAssetPath(source));
			} else
			{
				UnityEngine.Debug.LogWarning("Selection has no prefab");
			}
		}

		Finish();
	}

	[MenuItem("Current Selection/Prefabs/Disconnect From Prefab %#&y")]
	static public void DisconnectFromPrefab()
	{
		instance = Selection.gameObjects;

		for(int i = 0; i < instance.Length; i++)
		{
			GameObject root = PrefabUtility.FindPrefabRoot(instance[i]);
			UnityEngine.Object source = PrefabUtility.GetPrefabParent(root);

			if(source != null)
			{
				PrefabUtility.DisconnectPrefabInstance((UnityEngine.Object)instance[i]);
				UnityEngine.Debug.Log(instance[i].name + " disconnected from Prefab.");
			} else
			{
				UnityEngine.Debug.LogWarning("Selection has no prefab");
			}
		}

		Finish();
	}

	/*******************************************
	 *                Transform                *
	 *******************************************/

	[MenuItem("Current Selection/Transform/Center Children")]
	public static void CenterChildren()
	{
		Transform selTrans = Selection.activeTransform;

		Collider col = Selection.activeGameObject.GetComponent<Collider>();
		Vector3 colCenter = col.bounds.center;

		for(int i = 0; i < selTrans.childCount; i++)
		{
			Transform currChildTrans = selTrans.GetChild(i);

			UnityEngine.Debug.Log(colCenter + "|"+ currChildTrans.position +"|"+ CounteractTransformToCenter(currChildTrans, colCenter));

			currChildTrans.localPosition = CounteractTransformToCenter(currChildTrans, colCenter);
		}

		Finish();
	}

	//[MenuItem("Current Selection/Transform/Center Transform To Collider")]
	//public static void CenterTransformToCollider()
	//{
	//	Collider col = Selection.activeGameObject.GetComponent<Collider>();
	//	Selection.activeTransform.localPosition = col.bounds.center;

	//	Finish();
	//}

	private static Vector3 CounteractTransformToCenter(Transform trans, Vector3 center)
	{
		Vector3 pos = trans.localPosition;
		
		return center - pos;
	}

}
