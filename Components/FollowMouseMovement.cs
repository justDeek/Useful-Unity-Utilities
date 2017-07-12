//When enabled, the GameObject this script is attached to follows the movement of the mouse- / touch-position in 2D space
//Requires the tag "UICamera" and always one GameObject tagged with this in every scene
//				(alternatively use Camera.main to get the Main Camera)

using UnityEngine;

public class FollowMouseMovement : MonoBehaviour {

	[Range(0.01f, 1.0f)]
	public float smoothing = 1.0f;
	public bool worldSpace = true;
	private Camera UICamCamera;

	void Awake()
	{
		//disable this component immediately to activate it later on when necessary
		this.enabled = false;
	}

	void Start()
	{
		var UICam = GameObject.FindWithTag("UICamera");
		UICamCamera = UICam.GetComponent<Camera>();
	}

	void Update()
	{
		if (worldSpace)
		{
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, UICamCamera.ScreenToWorldPoint(Input.mousePosition), smoothing);
		} else
		{
			gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, UICamCamera.ScreenToWorldPoint(Input.mousePosition), smoothing);;
		}
	}
}
