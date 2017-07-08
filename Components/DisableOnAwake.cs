using System.Collections;
using UnityEngine;

public class DisableOnAwake : MonoBehaviour {

	void Awake () {
		/* Disables every GameObject tagged as "RemoveAtRuntime" (Disabling prevents Lag-Spikes especially on mobile,
		 * since Destroy() can be very demanding and the disabled GOs get removed on scene change) 
		 */
		foreach (GameObject searchGOByTag in GameObject.FindGameObjectsWithTag ("RemoveAtRuntime"))
		{
//			if (searchGOByTag != null)
//			{
				searchGOByTag.SetActive (false);
//			}
		}
	}

}
