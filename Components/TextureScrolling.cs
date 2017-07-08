using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScrolling : MonoBehaviour {

	public float scrollSpeed = 0.5f;
	public bool horizontal = true;
	public bool vertical = false;
	private Renderer _renderer;
	private float scrollUpdate;
	private Vector2 currentOffset;

	void Start () {
		_renderer = GetComponent<Renderer>();
	}

	void Update () {
		scrollUpdate = Time.time * scrollSpeed;

		if (horizontal == true && vertical == false) {
			currentOffset = new Vector2(scrollUpdate, 0);
		} else if (horizontal == true && vertical == true) {
			currentOffset = new Vector2(scrollUpdate, scrollUpdate);
		} else if (horizontal == false && vertical == true) {
			currentOffset = new Vector2(0, scrollUpdate);
		} else if (horizontal == false && vertical == false) {
			currentOffset = new Vector2(0, 0);
		}

		_renderer.material.mainTextureOffset = currentOffset;
	}
}
