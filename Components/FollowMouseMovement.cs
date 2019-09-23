using UnityEngine;

/// <summary>
/// When enabled, the GameObject this script is attached to follows the movement of the mouse-/touch-position in 2D space.
/// </summary>
public class FollowMouseMovement : MonoBehaviour {

	[Tooltip("If not set, uses the GameObject tagged 'MainCamera'. When using custom GUI systems like NGUI, you should insert its own camera.")]
	public Camera cam;
	[Tooltip("How much delay between the current mouse position and the position of the GameObject should be added. " +
	         "The higher the value, the closer the GameObject follows the mouse position.")]
	[Range(0.01f, 1f)]
	public float smoothing = 1f;
	[Tooltip("Whether to use the world or local space of the GameObject.")]
	public bool worldSpace = true;
	[Tooltip("if set, disable this component immediately to manually activate it at a later point.")]
	public bool startDisabled = true;	

	void Awake()
	{
		if(startDisabled) enabled = false;
	}

	void Start()
	{
		if(cam == null) cam = Camera.main;
	}

	void Update()
	{
		if (cam == null)
		{
			Debug.LogError("No camera set in FollowMouseMovement on " + name);
			return;
		}
		
		Vector3 currPos = worldSpace ? gameObject.transform.position : transform.localPosition;
		Vector3 result = Vector3.Lerp(currPos, cam.ScreenToWorldPoint(Input.mousePosition), smoothing);

		if (worldSpace) gameObject.transform.position = result;
		else gameObject.transform.localPosition = result;
	}
}