using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DebugSpeed : MonoBehaviour
{
	private Vector3 acceleration;
	private Vector3 lastVelocity;
	private Rigidbody ownRigidbody;
	private Vector3 linAcc;

	private string debugDesc1 = "Acceleration:";
	private string debug1;
	private string debugDesc2 = "Acc. sqrMagnitude:";
	private string debug2;
	private string debugDesc3 = "Acc. normalized:";
	private string debug3;
	private string debugDesc5 = "Velocity:";
	private string debug5;
	private string debugDesc6 = "Vel. sqrMagnitude:";
	private string debug6;
	private string debugDesc7 = "Vel. normalized:";
	private string debug7;

	void OnGUI()
	{
		GUI.Label(new Rect(2, 10, Screen.width, Screen.height), debugDesc1);
		GUI.Label(new Rect(2, 23, Screen.width, Screen.height), debug1);
		GUI.Label(new Rect(2, 40, Screen.width, Screen.height), debugDesc2);
		GUI.Label(new Rect(2, 53, Screen.width, Screen.height), debug2);
		GUI.Label(new Rect(2, 70, Screen.width, Screen.height), debugDesc3);
		GUI.Label(new Rect(2, 83, Screen.width, Screen.height), debug3);
		GUI.Label(new Rect(2, 100, Screen.width, Screen.height), debugDesc5);
		GUI.Label(new Rect(2, 113, Screen.width, Screen.height), debug5);
		GUI.Label(new Rect(2, 130, Screen.width, Screen.height), debugDesc6);
		GUI.Label(new Rect(2, 143, Screen.width, Screen.height), debug6);
		GUI.Label(new Rect(2, 160, Screen.width, Screen.height), debugDesc7);
		GUI.Label(new Rect(2, 173, Screen.width, Screen.height), debug7);
	}

	void Start()
	{
		ownRigidbody = gameObject.GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		//Get Values
		acceleration = (ownRigidbody.velocity - lastVelocity) / Time.fixedDeltaTime;
		lastVelocity = ownRigidbody.velocity;

		//Debug
		debug1 = "" + acceleration;
		debug2 = "" + acceleration.sqrMagnitude;
		debug3 = "" + acceleration.normalized;
		debug5 = "" + ownRigidbody.velocity;
		debug6 = "" + ownRigidbody.velocity.sqrMagnitude;
		debug7 = "" + ownRigidbody.velocity.normalized;
	}
}
