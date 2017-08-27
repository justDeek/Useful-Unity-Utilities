using System.Collections.Generic;
using UnityEngine;
/* Implementation:
	 In UISprite.cs add "ApplyEffect(verts, uvs, cols, offset);" to the end of OnFill(), change
	 the inheritance in UIBasicSpriteEditor.cs from 'UIWidgetInspector' to 'NGUIEffectsInspector'
	 and in UISprite.cs from 'UIBasicSprite' to 'NGUIEffects'
*/
[ExecuteInEditMode]
public class NGUIEffects : UISprite
{

	public enum Effect
	{
		None,
		Shadow,
		Shadow3D,
		Outline,
		Outline8,
		Grayscale,
	}

	//Effect Properties
	[HideInInspector] [SerializeField] Effect mEffectStyle = Effect.None;
	[HideInInspector] [SerializeField] Color mEffectColor = new Color(0f, 0f, 0f, 0.5f); //(Black with half alpha)
	[HideInInspector] [SerializeField] Vector3 mEffectDistance = Vector3.zero;
	[HideInInspector] [SerializeField] Vector2 mEffectSource = Vector2.zero;
	[HideInInspector] [SerializeField] Vector2 mEffectScale = Vector2.one;
	[HideInInspector] [System.NonSerialized] bool drawGizmos = true;

	public Vector3 getPos = Vector3.zero;
	public bool ignore = false;

	virtual public int minWidth { get { return 2; } }
	virtual public int minHeight { get { return 2; } }

	public Effect effectStyle
	{
		get
		{
			return mEffectStyle;
		}
		set
		{
			if (mEffectStyle != value)
			{
				mEffectStyle = value;
			}
		}
	}

	public Color effectColor
	{
		get
		{
			return mEffectColor;
		}
		set
		{
			if (mEffectColor != value)
			{
				mEffectColor = value;
			}
		}
	}

	public Vector3 effectDistance
	{
		get
		{
			return mEffectDistance;
		}
		set
		{
			if (mEffectDistance != value)
			{
				mEffectDistance = value;
			}
		}
	}

	public int quadsPerCharacter
	{
		get
		{
			if (mEffectStyle == Effect.Shadow) return 2;
			else if (mEffectStyle == Effect.Outline) return 5;
			else if (mEffectStyle == Effect.Outline8) return 9;
			return 1;
		}
	}

	void Update()
	{
		if (this.transform.hasChanged && effectStyle == Effect.Shadow3D)
		{
			base.OnInit();
		}
	}

	// virtual public void OnFill (List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
	// {
	// 	// Call this in your derived classes:
	// 	if (onPostFill != null)
	// 		onPostFill(GetComponent<UIWidget>(), verts.Count, verts, uvs, cols);
	// }

	public static void ApplyEffect(int offset, List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
	{
		NGUIEffects ne = new NGUIEffects();
		Vector3 pos = Vector3.zero;
		int end = verts.Count;

		switch (ne.effectStyle)
		{
			case Effect.None:
				break;
			case Effect.Shadow:
			case Effect.Shadow3D:
			case Effect.Outline:
			case Effect.Outline8:
				pos.x = ne.mEffectDistance.x;
				pos.y = ne.mEffectDistance.y;
				pos.z = ne.mEffectDistance.z;

				if (ne.effectStyle == Effect.Shadow3D)
				{
					ne.ApplyShadow3D(verts, uvs, cols, offset, end, pos.x, -pos.y, pos.z);
				}
				else
				{
					ne.ApplyShadow(verts, uvs, cols, offset, end, pos.x, -pos.y, pos.z);
				}

				if ((ne.effectStyle == Effect.Outline) || (ne.effectStyle == Effect.Outline8))
				{
					offset = end;
					end = verts.Count;

					ne.ApplyShadow(verts, uvs, cols, offset, end, -pos.x, pos.y, pos.z);

					offset = end;
					end = verts.Count;

					ne.ApplyShadow(verts, uvs, cols, offset, end, pos.x, pos.y, pos.z);

					offset = end;
					end = verts.Count;

					ne.ApplyShadow(verts, uvs, cols, offset, end, -pos.x, -pos.y, pos.z);

					if (ne.effectStyle == Effect.Outline8)
					{
						offset = end;
						end = verts.Count;

						ne.ApplyShadow(verts, uvs, cols, offset, end, -pos.x, 0, pos.z);

						offset = end;
						end = verts.Count;

						ne.ApplyShadow(verts, uvs, cols, offset, end, pos.x, 0, pos.z);

						offset = end;
						end = verts.Count;

						ne.ApplyShadow(verts, uvs, cols, offset, end, 0, pos.y, pos.z);

						offset = end;
						end = verts.Count;

						ne.ApplyShadow(verts, uvs, cols, offset, end, 0, -pos.y, pos.z);
					}
				}
				break;
			case Effect.Grayscale:
				ne.ApplyGrayscale(verts, uvs, cols, offset, end);
				break;
		}
	}

	public void ApplyShadow(List<Vector3> verts, List<Vector2> uvs, List<Color> cols, int start, int end, float x, float y, float z)
	{
		Color c = mEffectColor;
		c.a *= finalAlpha;
		Color col = c;

		for (int i = start; i < end; ++i)
		{
			verts.Add(verts[i]);
			uvs.Add(uvs[i]);
			cols.Add(cols[i]);

			var v = verts[i];
			v.x += x;
			v.y += y;
			v.z += z;
			v.Scale(new Vector3(mEffectScale.x, mEffectScale.y, 1f));
			verts[i] = v;

			Color uc = cols[i];

			if (uc.a == 1f)
			{
				cols[i] = col;
			}
			else
			{
				Color fc = c;
				fc.a = uc.a * c.a;
				cols[i] = fc;
			}
		}
		AfterAppliedEffect();
	}

	public void ApplyShadow3D(List<Vector3> verts, List<Vector2> uvs, List<Color> cols, int start, int end, float x, float y, float z)
	{
		Color c = mEffectColor;
		c.a *= finalAlpha;
		Color col = c;

		//Get Screen-Center
		float xScreenHalf = Screen.width / 2 + mEffectSource.x;
		float yScreenHalf = Screen.height / 2 + mEffectSource.y;

		//Get UICamera
		// Camera uiCam = null;
		// Camera[] allCams = Camera.allCameras;
		// foreach (Camera cam in allCams) {
		// 	uiCam = cam.gameObject.GetComponent<UICamera>().gameObject.GetComponent<Camera>();
		// }
		Camera uiCam = NGUITools.FindCameraForLayer(cachedGameObject.layer);

		//Get current Screen-Position of the GO with the UISprite-Component attached
		Vector3 currentPos = uiCam.WorldToScreenPoint(this.gameObject.transform.position);

		//Calculate distance between the Screen-Center and the current position
		float xDistance = -(xScreenHalf - currentPos.x) * (z / 10);
		float yDistance = -(yScreenHalf - currentPos.y) * (z / 10);

		for (int i = start; i < end; ++i)
		{
			verts.Add(verts[i]);
			uvs.Add(uvs[i]);
			cols.Add(cols[i]);

			var v = verts[i];
			v.x += xDistance * (x + 0.5f) * 10;
			v.y += yDistance * -(y - 0.5f) * 10;
			v.z += z;
			v.Scale(new Vector3(mEffectScale.x, mEffectScale.y, 1));
			verts[i] = v;

			Color uc = cols[i];

			if (uc.a == 1f)
			{
				cols[i] = col;
			}
			else
			{
				Color fc = c;
				fc.a = uc.a * c.a;
				cols[i] = fc;
			}
		}
		AfterAppliedEffect();
	}

	public void ApplyGrayscale(List<Vector3> verts, List<Vector2> uvs, List<Color> cols, int start, int end)
	{
		Texture2D tex2D = mainTexture as Texture2D;
		Texture2D newTex2D = new Texture2D(tex2D.width, tex2D.height, tex2D.format, false);
		float h, s, v;
		Color c;
		// for (int i = start; i < end; ++i)
		// {
		// 	c = cols[i];
		// 	Color.RGBToHSV(c, out h, out s, out v);
		// 	s = 0f;
		// 	c = Color.HSVToRGB(h, s, v);
		// 	cols[i] = c;
		// }
		// if (!ignore) {
		// 	ignore = true;
		// 	this.GetComponent<UISprite>().OnFill(verts, uvs, cols);
		// 	ignore = false;
		// }

		// for (int x = 1; x < tex2D.width; ++x)
		// {
		// 	for (int y = 1; y < tex2D.height; ++y)
		// 	{
		// 		c = newTex2D.GetPixel(x,y);
		// 		Color.RGBToHSV(c, out h, out s, out v);
		// 		s = 0f;
		// 		c = Color.HSVToRGB(h, s, v);
		// 		newTex2D.SetPixel(x, y, c);
		// 	}
		// }
		// newTex2D.Apply();
		// SpriteRenderer renderer = this.gameObject.AddComponent<SpriteRenderer>();
		// renderer.sprite = Sprite.Create(newTex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f));
		AfterAppliedEffect();
	}

	private void AfterAppliedEffect()
	{
		panel.RebuildAllDrawCalls(); //force redraw to refresh after changes
	}

}
