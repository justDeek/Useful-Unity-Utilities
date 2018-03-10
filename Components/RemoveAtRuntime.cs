using UnityEngine;

/// <summary>
/// Removes the GameObject this is attached to immediately
/// on scene load without letting it execute anything.
/// Useful to remove GOs that are used for debugging at
/// runtime or in build.
/// Optionally remove all GOs which name contains "OBSOLETE"
/// (best to only use one of this component per scene or on
/// a GameObject that's persistent throughout all scenes).
/// </summary>
public class RemoveAtRuntime : MonoBehaviour
{
	public bool removeOBSOLETE = false;

	void Awake()
	{
		if(!removeOBSOLETE)
		{
			Destroy(this.gameObject);
		} else
		{
			foreach(var currentGO in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				if(currentGO.name.Contains("OBSOLETE"))
				{
					Destroy(currentGO);
				}
			}
		}
	}
}
