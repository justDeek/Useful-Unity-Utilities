using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public class CurrentSelectionMenu : MonoBehaviour
{
  //container for current selection
  static GameObject[] instance;

  static Object[] clones;

  //Validation for all selection-based options (Only enabled these options if something is selected in hirarchy)
	[MenuItem("Current Selection/Clone/Clone selected GameObject %#d", true)]
  [MenuItem("Current Selection/Rename selected GameObjects incrementally %#r", true)]
  [MenuItem("Current Selection/Parent/Select Parent &UP", true)]
  [MenuItem("Current Selection/Parent/Parent: Select all Children &DOWN", true)]
  [MenuItem("Current Selection/Parent/Parent: Sort Children by Name &s", true)]
  [MenuItem("Current Selection/Select previous GameObject #UP", true)]
  [MenuItem("Current Selection/Select next GameObject #DOWN", true)]
  [MenuItem("Current Selection/Prefabs/Revert To Prefab %&y", true)]
  [MenuItem("Current Selection/Prefabs/Apply Prefab Changes %#y", true)]
  [MenuItem("Current Selection/Prefabs/Disconnect From Prefab %#&y", true)]
	private static bool GeneralValidation()
	{
    //Disable Options if nothing is selected
		return Selection.activeGameObject != null;
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
		for (int i = 0; i < instance.Length; i++)
		{
			Object root = PrefabUtility.GetPrefabParent(instance[i]);
			GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(root);
			if (root != null)
			{
				clone.transform.position = instance[i].transform.position;
				clone.transform.rotation = instance[i].transform.rotation;
			}
			else //If you try to duplicate something other than a prefab
			{
				clone = Instantiate(instance[i], instance[i].transform.position, instance[i].transform.rotation) as GameObject;
			}

			clone.name = instance [i].name;

			clones[i] = clone;
			Selection.objects = clones;

		}

		//reparent and reset Transform
		foreach (GameObject obj in Selection.gameObjects)
    {
			obj.transform.parent = initRoot;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
		}
    //Mark active Scene as dirty to show that changes were made and that the user is able to save those
    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}

	[MenuItem("Current Selection/Clone/Undo Cloning %#z", true)]
	private static bool UndoCloneValidation()
	{
		return clones != null;
	}

	[MenuItem("Current Selection/Clone/Undo Cloning %#z")]
	public static void UndoClone()
	{
		if (clones != null)
		{
			for (int i = 0; i < clones.Length; i++)
			{
				GameObject temp = (GameObject)clones[i];
				DestroyImmediate(temp);
			}
			clones = null;
		}
	}

	[MenuItem("Current Selection/Rename selected GameObjects incrementally %#r")]
  public static void Rename()
  {
	instance = Selection.gameObjects;
	int j = 0;

    for (int i = 0; i < instance.Length; i++)
    {
  		j = i + 1;
  		if (j <= 9)
  		{
  			instance[i].name = instance[i].name + "0" + j;
  		}
  		else
  		{
  			instance[i].name = instance[i].name + j;
  		}
    }
    //Mark active Scene as dirty to show that changes were made and that the user is able to save those
    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
  }

/*******************************************
 *                 Parent                  *
 *******************************************/

	[MenuItem("Current Selection/Parent/Select Parent &UP")]
	public static void SelectParent()
	{
		List<Object> currentSelection = new List<Object>{ };
		Object[] tmpArray = new Object[]{ };
		//Try to add parent to List, otherwise add current selection
		try
		{
			currentSelection.Add ((Object)Selection.activeTransform.parent.gameObject);
		}
		catch (System.Exception e)
		{
			UnityEngine.Debug.LogWarning ("The selected GameObject has no parent. Caught the following Error: " + e.ToString ());
			currentSelection.Add ((Object)Selection.activeTransform.gameObject);
		}

		tmpArray = currentSelection.ToArray ();
		if (tmpArray != null)
		Selection.objects = tmpArray;
	}

	[MenuItem("Current Selection/Parent/Parent: Select all Children &DOWN")]
	public static void SelectAllChildren()
	{
		List<Object> childrenArray = new List<Object>{};
		Object[] tmpArray = new Object[] {};
		//Get Child amount
		int children = Selection.activeTransform.childCount;

		//iterate through children
		for(int i = 0; i < children; ++i)
		{
			//Add next child to List
			childrenArray.Add((Object)Selection.activeTransform.GetChild (i).gameObject);
			//convert List to Array
			tmpArray = childrenArray.ToArray ();
		}

		//Assign all children to Selection
		Selection.objects = tmpArray;

	}

	[MenuItem("Current Selection/Parent/Parent: Sort Children by Name &s")]
	public static void SortChildrenByName()
	{
		foreach (GameObject obj in Selection.gameObjects)
    {
			List<Transform> children = new List<Transform>();

			for (int i = obj.transform.childCount - 1; i >= 0; i--) {
				Transform child = obj.transform.GetChild(i);
				children.Add(child);
				child.parent = null;
			}
			children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
			foreach (Transform child in children)
      {
				child.parent = obj.transform;
			}
		}
    //Mark active Scene as dirty to show that changes were made and that the user is able to save those
    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}

  [MenuItem("Current Selection/Select previous GameObject #UP")]
  public static void SelectpreviousGO()
  {
    List<Object> currentSelection = new List<Object>{ };
    Object[] tmpArray = new Object[]{ };
    Transform selectionParent = Selection.activeTransform.parent;
    int parentChildCount = selectionParent.childCount;

    try{
      for (int i = 0; i<parentChildCount; i++)
      {
        if (selectionParent.GetChild(i) == Selection.activeTransform)
        {
          currentSelection.Add((Object)selectionParent.GetChild(i-1).gameObject);
        }
      }
    }
    catch (System.Exception e)
    {
      UnityEngine.Debug.LogWarning ("There are no more GameObject before this one: " + e.ToString ());
      currentSelection.Add ((Object)Selection.activeTransform.gameObject);
    }

    tmpArray = currentSelection.ToArray ();
    if (tmpArray != null)
    Selection.objects = tmpArray;
  }

  [MenuItem("Current Selection/Select next GameObject #DOWN")]
  public static void SelectNextGO()
  {
    List<Object> currentSelection = new List<Object>{ };
    Object[] tmpArray = new Object[]{ };
    Transform selectionParent = Selection.activeTransform.parent;
    int parentChildCount = selectionParent.childCount;

    try{
      for (int i = 0; i<=parentChildCount-1; i++)
      {
        if (selectionParent.GetChild(i) == Selection.activeTransform)
        {
          currentSelection.Add((Object)selectionParent.GetChild(i+1).gameObject);
        }
      }
    }
    catch (System.Exception e)
    {
      UnityEngine.Debug.LogWarning ("There is no more GameObject after this one: " + e.ToString ());
      currentSelection.Add ((Object)Selection.activeTransform.gameObject);
    }

    tmpArray = currentSelection.ToArray ();
    if (tmpArray != null)
    Selection.objects = tmpArray;
  }

/*******************************************
 *                Prefabs                  *
 *******************************************/

  [MenuItem("Current Selection/Prefabs/Revert To Prefab %&y")]
  static public void RevertToPrefab()
  {
      instance = Selection.gameObjects;

      for (int i = 0; i < instance.Length; i++)
      {
        //works even better than the 'Revert'-Button in the Inspector,
        //in that this also remains saved references to components from other GameObjects)
        GameObject root = PrefabUtility.FindPrefabRoot(instance[i]);
        GameObject source = (GameObject)PrefabUtility.GetPrefabParent(root);

        if (source != null)
        {
          PrefabUtility.ConnectGameObjectToPrefab(root, source);
          UnityEngine.Debug.Log(instance[i].name + " reverted to Prefab.");
        }
        else
        {
          UnityEngine.Debug.LogWarning("Selection has no prefab");
        }
      }
  }

  [MenuItem("Current Selection/Prefabs/Apply Prefab Changes %#y")]
  static public void ApplyPrefabChanges()
  {
      instance = Selection.gameObjects;

      for (int i = 0; i < instance.Length; i++)
      {
        GameObject root = PrefabUtility.FindPrefabRoot(instance[i]);
        Object source = PrefabUtility.GetPrefabParent(root);

        if (source != null)
        {
          PrefabUtility.ReplacePrefab(root, source,  ReplacePrefabOptions.ConnectToPrefab);
          UnityEngine.Debug.Log("Updating prefab : "+AssetDatabase.GetAssetPath(source));
        }
        else
        {
          UnityEngine.Debug.LogWarning("Selection has no prefab");
        }
      }
  }

  [MenuItem("Current Selection/Prefabs/Disconnect From Prefab %#&y")]
  static public void DisconnectFromPrefab()
  {
      instance = Selection.gameObjects;

      for (int i = 0; i < instance.Length; i++)
      {
        GameObject root = PrefabUtility.FindPrefabRoot(instance[i]);
        Object source = PrefabUtility.GetPrefabParent(root);

        if (source != null)
        {
          PrefabUtility.DisconnectPrefabInstance((Object)instance[i]);
          UnityEngine.Debug.Log(instance[i].name + " disconnected from Prefab.");
        }
        else
        {
          UnityEngine.Debug.LogWarning("Selection has no prefab");
        }
      }
  }

}
