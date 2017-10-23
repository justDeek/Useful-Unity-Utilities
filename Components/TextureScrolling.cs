using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class TextureScrolling : MonoBehaviour
{

	public enum UpdateType
	{
		Update,
		LateUpdate,
		FixedUpdate
	}

	public float scrollSpeed = 0.5f;
	public bool horizontal = true;
	public bool vertical = false;
	public UpdateType updateType = UpdateType.Update;
	public bool ignoreTimeScale = true;

	private Renderer _renderer;
	private float scrollUpdate;
	private Vector2 currentOffset;
	private float usedTime;

	void Start()
	{
		_renderer = GetComponent<Renderer>();
	}

	void Update()
	{
		if(updateType == UpdateType.Update)
		{
			DoScroll();
		}
	}

	void LateUpdate()
	{
		if(updateType == UpdateType.LateUpdate)
		{
			DoScroll();
		}
	}

	void FixedUpdate()
	{
		if(updateType == UpdateType.FixedUpdate)
		{
			DoScroll();
		}
	}

	public void DoScroll()
	{
		if(ignoreTimeScale)
		{
			if(Time.inFixedTimeStep)
			{
				usedTime = Time.fixedUnscaledTime;
			} else
			{
				usedTime = Time.unscaledTime;
			}
		} else
		{
			usedTime = Time.time;
		}

		scrollUpdate = usedTime * scrollSpeed;

		if(horizontal == true && vertical == false)
		{
			currentOffset = new Vector2(scrollUpdate, 0);
		} else if(horizontal == true && vertical == true)
		{
			currentOffset = new Vector2(scrollUpdate, scrollUpdate);
		} else if(horizontal == false && vertical == true)
		{
			currentOffset = new Vector2(0, scrollUpdate);
		} else if(horizontal == false && vertical == false)
		{
			currentOffset = new Vector2(0, 0);
		}

		var allMaterials = _renderer.materials;
		foreach(var currentMaterial in allMaterials)
		{
			currentMaterial.mainTextureOffset = currentOffset;
		}
	}
}
