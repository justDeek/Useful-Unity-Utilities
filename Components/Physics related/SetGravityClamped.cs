using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SetGravityClamped : MonoBehaviour
{

	public float maxVelocity = 10;
	public ForceMode forceMode = ForceMode.Acceleration;

	private Rigidbody ownRigidbody;

	void Start()
	{
		ownRigidbody = this.gameObject.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (ownRigidbody.velocity.sqrMagnitude < maxVelocity)
		{
			ownRigidbody.AddForce(Physics.gravity, forceMode);
		}
	}
}
