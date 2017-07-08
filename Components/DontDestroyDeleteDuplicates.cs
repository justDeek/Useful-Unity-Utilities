using UnityEngine;
using System.Collections;

public class DontDestroyDeleteDuplicates : MonoBehaviour {

	void Awake()
	{  
		DontDestroyOnLoad (gameObject);

		//Destroy identical GO when switching Scenes (otherwise would leave you with a duplicate of this GO when going back to a previous scene)
		foreach(GameObject searchSameGO in GameObject.FindGameObjectsWithTag ("Persistent"))
		{
			if ((searchSameGO.GetInstanceID () > gameObject.GetInstanceID ()) || (searchSameGO.name == gameObject.name && searchSameGO != gameObject))
			{
				DestroyImmediate (searchSameGO);
			}
		}

		//Destroy every GameObject tagged as "DestroyAtRuntime"
		foreach (GameObject searchGOByTag in GameObject.FindGameObjectsWithTag ("RemoveAtRuntime")) 
		{
			if (searchGOByTag != null)
			{
				DestroyImmediate (searchGOByTag);
			}
		}

		//Seemingly not working:
//		if (FindObjectsOfType(GetType()).Length > 1)
//		{
//			Destroy(gameObject);
//		}
			
	}

}
