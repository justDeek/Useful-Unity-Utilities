using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class DontDestroyDeleteDuplicates : MonoBehaviour {

	void Awake()
	{  
		DontDestroyOnLoad (gameObject);

		//Destroy identical GO when switching Scenes (otherwise would leave you with a duplicate of this GO when going back to a previous scene)
		foreach(GameObject searchSameGO in GameObject.FindGameObjectsWithTag ("Persistent"))
		{
			if ((searchSameGO.GetInstanceID () > gameObject.GetInstanceID ()) || (searchSameGO.name == this.gameObject.name && searchSameGO != this.gameObject))
			{
				Destroy (searchSameGO);
			}
		}
		//disable this script
		this.enabled = false;

		//Seemingly not working:
//		if (FindObjectsOfType(GetType()).Length > 1)
//		{
//			Destroy(gameObject);
//		}
			
	}

//	void OnEnable()
//	{
//		SceneManager.sceneLoaded += OnLevelFinishedLoading;
//	}
//
//	void OnDisable()
//	{
//		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
//	}
//
//	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
//	{
//
//	}

}
