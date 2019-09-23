using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;

public class UIDragDropItem2D : MonoBehaviour
{
	[Range(0.01f, 1.0f)]
	public float smoothing = 1.0f;
	[Range(0.0f, 5.0f)]
	public float pressAndHoldDelay = 1f;
	private GameObject UICam;
	private Camera UICamCamera;

	#region Common functionality

	[System.NonSerialized] protected Transform mTrans;
	[System.NonSerialized] protected Transform mParent;
	[System.NonSerialized] protected Collider mCollider;
	[System.NonSerialized] protected Collider2D mCollider2D;
	[System.NonSerialized] protected UIButton mButton;
	[System.NonSerialized] protected UIRoot mRoot;
	[System.NonSerialized] protected float mDragStartTime = 0f;
	[System.NonSerialized] protected UIDragScrollView mDragScrollView = null;
	[System.NonSerialized] protected bool mPressed = false;
	[System.NonSerialized] protected bool mDragging = false;
	[System.NonSerialized] protected UICamera.MouseOrTouch mTouch;

	static public List<UIDragDropItem2D> draggedItems = new List<UIDragDropItem2D>();

	protected virtual void Awake()
	{
		mTrans = transform;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
		mCollider = collider;
		mCollider2D = collider2D;
#else
		mCollider = gameObject.GetComponent<Collider>();
		mCollider2D = gameObject.GetComponent<Collider2D>();
#endif
	}

	protected virtual void OnEnable() { }
	protected virtual void OnDisable() { if(mDragging) StopDragging(UICamera.hoveredObject); }

	protected virtual void Start()
	{
		UICam = GameObject.FindWithTag("UICamera");
		UICamCamera = UICam.GetComponent<Camera>();
		mButton = GetComponent<UIButton>();
		mDragScrollView = GetComponent<UIDragScrollView>();
	}

	protected virtual void OnPress(bool isPressed)
	{
		if(UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3) return;

		if(isPressed)
		{
			if(!mPressed)
			{
				mTouch = UICamera.currentTouch;
				mDragStartTime = RealTime.time + pressAndHoldDelay;
				mPressed = true;
			}
		} else if(mPressed && mTouch == UICamera.currentTouch)
		{
			mPressed = false;
			mTouch = null;
		}
	}

	protected virtual void OnDragStart()
	{
		if(!enabled || mTouch != UICamera.currentTouch) return;

		StartDragging();
	}

	public virtual void StartDragging()
	{
		if(!mDragging)
		{
			mDragging = true;
			OnDragDropStart();
		}
	}

	protected virtual void OnDrag(Vector2 delta)
	{
		if(!mDragging || !enabled || mTouch != UICamera.currentTouch) return;
		if(mRoot != null) OnDragDropMove(delta * mRoot.pixelSizeAdjustment);
		else OnDragDropMove(delta);
	}

	protected virtual void OnDragEnd()
	{
		if(!enabled || mTouch != UICamera.currentTouch) return;
		StopDragging(UICamera.hoveredObject);
	}

	public void StopDragging(GameObject go)
	{
		if(mDragging)
		{
			mDragging = false;
			OnDragDropRelease(go);
		}
	}

	#endregion

	protected virtual void OnDragDropStart()
	{
		if(!draggedItems.Contains(this)) draggedItems.Add(this);
		if(mDragScrollView != null) mDragScrollView.enabled = false;
		if(mButton != null) mButton.isEnabled = false;
		else if(mCollider != null) mCollider.enabled = false;
		else if(mCollider2D != null) mCollider2D.enabled = false;
		mParent = mTrans.parent;
		mRoot = NGUITools.FindInParents<UIRoot>(mParent);
		mTrans.parent = GameObject.FindWithTag("Drag and Drop Root").transform;

		foreach(Transform child in mTrans)
		{
			child.gameObject.SetActive(false);
		}

		TweenPosition tp = GetComponent<TweenPosition>();
		if(tp != null) tp.enabled = false;

		SpringPosition sp = GetComponent<SpringPosition>();
		if(sp != null) sp.enabled = false;

		NGUITools.MarkParentAsChanged(gameObject);
	}

	protected virtual void OnDragDropMove(Vector2 delta)
	{
		mTrans.position = Vector3.Lerp(mTrans.position, UICamCamera.ScreenToWorldPoint(Input.mousePosition), smoothing);
	}

	protected virtual void OnDragDropRelease(GameObject surface)
	{
		var drags = GetComponentsInChildren<UIDragScrollView>();
		foreach(var d in drags) d.scrollView = null;

		Collider2D[] cols = Physics2D.OverlapPointAll(new Vector2(mTrans.position.x, mTrans.position.y));
		float zDepth = 0;
		float closestZDepth = float.MaxValue;
		int closestZDepthID = 0;

		for(int i = 0; i < cols.Length; i++)
		{
			//remove any collider which GO doesn't contain the tag 'Slot'
			if(cols[i].gameObject.tag != "Slot") continue;
			//collect all z-depths
			zDepth = cols[i].gameObject.transform.localPosition.z;

			//get closest z-depth index
			if(zDepth < closestZDepth)
			{
				closestZDepth = zDepth;
				closestZDepthID = i;
			}
		}

		if(cols[closestZDepthID] && cols[closestZDepthID].gameObject.tag == "Slot")
		{
			try
			{
				GDEDataManager.SetString("DroppedSurface", "Value", cols[closestZDepthID].gameObject.name);
			} catch(UnityException ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
		}

		if(mButton != null) mButton.isEnabled = true;
		else if(mCollider != null) mCollider.enabled = true;
		else if(mCollider2D != null) mCollider2D.enabled = true;

		UIDragDropContainer container = surface ? NGUITools.FindInParents<UIDragDropContainer>(surface) : null;

		if(container != null) mTrans.parent = container.reparentTarget ?? container.transform;
		else mTrans.parent = mParent;

		//mTrans.localPosition = Vector3.zero;

		foreach(Transform child in mTrans)
		{
			child.gameObject.SetActive(true);
		}

		if(mDragScrollView != null) Invoke("EnableDragScrollView", 0.001f);

		NGUITools.MarkParentAsChanged(gameObject);

		OnDragDropEnd();
	}

	protected virtual void OnDragDropEnd() { draggedItems.Remove(this); }

	protected void EnableDragScrollView()
	{
		if(mDragScrollView != null)
			mDragScrollView.enabled = true;
	}
}
