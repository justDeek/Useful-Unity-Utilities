using UnityEngine;

/// <summary>
/// Sets the GameObject this component is attached to to not be destroyed 
/// on scene load and removes duplicates when re-visiting scenes.
/// </summary>
public class DontDestroyDeleteDuplicates : MonoBehaviour
{
	private void Awake()
	{
		if (GameObject.Find(gameObject.name)
			&& GameObject.Find(gameObject.name) != this.gameObject)
		{
			Destroy(GameObject.Find(gameObject.name));
		}
	}

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

}
