using UnityEngine;

/// <summary>
/// Disables every GameObject tagged as "RemoveAtRuntime" (tag has to be created if it doesn't exist)
/// </summary>
public class DisableOnAwake : MonoBehaviour
{
	void Awake()
	{
		GameObject[] allTaggedGOs = GameObject.FindGameObjectsWithTag("RemoveAtRuntime");

		if(allTaggedGOs.Length == 0)
		{
			return;
		}

		foreach(GameObject go in allTaggedGOs)
		{
			if(go != null)
			{
				go.SetActive(false);
			}
		}
	}
}
