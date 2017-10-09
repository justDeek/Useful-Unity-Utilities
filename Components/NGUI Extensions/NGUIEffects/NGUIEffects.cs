using System.Collections.Generic;
using UnityEngine;

/**
 * Implementation (has to be repeated on every NGUI update/re-install):
 * 1. change the inheritance in UIBasicSpriteEditor.cs from 'UIWidgetInspector' to 'NGUIEffectsInspector'
 * 2. In UISprite.cs add "ApplyEffect(this, offset, verts, uvs, cols);" to the end of OnFill() and
 * 3. copy the following content (everything inside 'CopyToUISprite') to the end of UISprite (after OnFill(), of course uncommented)
 */
public class CopyToUISprite
{
	//public enum Effect
	//{
	//	None,
	//	Shadow,
	//	Shadow3D,
	//	Outline,
	//	Outline8,
	//	Grayscale,
	//}

	////Effect Properties
	//[HideInInspector] [SerializeField] public Effect effectStyle = Effect.None;
	//[HideInInspector] [SerializeField] public Color mEffectColor = new Color(0f, 0f, 0f, 0.5f); //(Black with half alpha)
	//[HideInInspector] [SerializeField] public Vector3 mEffectDistance = Vector3.zero;
	//[HideInInspector] [SerializeField] public Vector2 mEffectSource = Vector2.zero;
	//[HideInInspector] [SerializeField] public Vector2 mEffectScale = Vector2.one;
	//[HideInInspector] [System.NonSerialized] bool drawGizmos = true;

	//void LateUpdate()
	//{
	//	if(this.transform.hasChanged && effectStyle == Effect.Shadow3D)
	//	{
	//		base.OnInit();
	//	}
	//}

	//private void AfterAppliedEffect()
	//{
	//	//force redraw to refresh after changes
	//	panel.RebuildAllDrawCalls();
	//}
}


namespace CustomExtensions.NGUI
{
	[ExecuteInEditMode]
	public static class NGUIEffects
	{
		public static void ApplyEffect(UISprite ne, int offset, List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
		{
			Vector3 pos = Vector3.zero;
			int end = verts.Count;

			switch(ne.effectStyle)
			{
				case UISprite.Effect.None:
					break;
				case UISprite.Effect.Shadow:
				case UISprite.Effect.Shadow3D:
				case UISprite.Effect.Outline:
				case UISprite.Effect.Outline8:
					pos.x = ne.mEffectDistance.x;
					pos.y = ne.mEffectDistance.y;
					pos.z = ne.mEffectDistance.z;

					if(ne.effectStyle == UISprite.Effect.Shadow3D)
					{
						ApplyShadow3D(ne, verts, uvs, cols, offset, end, pos.x, -pos.y, pos.z);
					} else
					{
						ApplyShadow(ne, verts, uvs, cols, offset, end, pos.x, -pos.y, pos.z);
					}

					if((ne.effectStyle == UISprite.Effect.Outline) || (ne.effectStyle == UISprite.Effect.Outline8))
					{
						offset = end;
						end = verts.Count;

						ApplyShadow(ne, verts, uvs, cols, offset, end, -pos.x, pos.y, pos.z);

						offset = end;
						end = verts.Count;

						ApplyShadow(ne, verts, uvs, cols, offset, end, pos.x, pos.y, pos.z);

						offset = end;
						end = verts.Count;

						ApplyShadow(ne, verts, uvs, cols, offset, end, -pos.x, -pos.y, pos.z);

						if(ne.effectStyle == UISprite.Effect.Outline8)
						{
							offset = end;
							end = verts.Count;

							ApplyShadow(ne, verts, uvs, cols, offset, end, -pos.x, 0, pos.z);

							offset = end;
							end = verts.Count;

							ApplyShadow(ne, verts, uvs, cols, offset, end, pos.x, 0, pos.z);

							offset = end;
							end = verts.Count;

							ApplyShadow(ne, verts, uvs, cols, offset, end, 0, pos.y, pos.z);

							offset = end;
							end = verts.Count;

							ApplyShadow(ne, verts, uvs, cols, offset, end, 0, -pos.y, pos.z);
						}
					}
					break;
			}
			ne.AfterAppliedEffect();
		}

		public static void ApplyShadow(UISprite ne, List<Vector3> verts, List<Vector2> uvs, List<Color> cols, int start, int end, float x, float y, float z)
		{
			Color c = ne.mEffectColor;
			c.a *= ne.finalAlpha;
			Color col = c;

			for(int i = start; i < end; ++i)
			{
				verts.Add(verts[i]);
				uvs.Add(uvs[i]);
				cols.Add(cols[i]);

				var v = verts[i];
				v.x += x;
				v.y += y;
				v.z += z;
				v.Scale(new Vector3(ne.mEffectScale.x, ne.mEffectScale.y, 1f));
				verts[i] = v;

				Color uc = cols[i];

				if(uc.a == 1f)
				{
					cols[i] = col;
				} else
				{
					Color fc = c;
					fc.a = uc.a * c.a;
					cols[i] = fc;
				}
			}
		}

		public static void ApplyShadow3D(UISprite ne, List<Vector3> verts, List<Vector2> uvs, List<Color> cols, int start, int end, float x, float y, float z)
		{
			Color c = ne.mEffectColor;
			c.a *= ne.finalAlpha;
			Color col = c;

			//Get Screen-Center
			float xScreenHalf = Screen.width / 2 + ne.mEffectSource.x;
			float yScreenHalf = Screen.height / 2 + ne.mEffectSource.y;

			//Get UICamera
			// Camera uiCam = null;
			// Camera[] allCams = Camera.allCameras;
			// foreach (Camera cam in allCams) {
			// 	uiCam = cam.gameObject.GetComponent<UICamera>().gameObject.GetComponent<Camera>();
			// }
			Camera uiCam = NGUITools.FindCameraForLayer(ne.cachedGameObject.layer);

			//Get current Screen-Position of the GO with the UISprite-Component attached
			Vector3 currentPos = uiCam.WorldToScreenPoint(ne.gameObject.transform.position);

			//Calculate distance between the Screen-Center and the current position
			float xDistance = -(xScreenHalf - currentPos.x) * (z / 10);
			float yDistance = -(yScreenHalf - currentPos.y) * (z / 10);

			for(int i = start; i < end; ++i)
			{
				verts.Add(verts[i]);
				uvs.Add(uvs[i]);
				cols.Add(cols[i]);

				var v = verts[i];
				v.x += xDistance * (x + 0.5f) * 10;
				v.y += yDistance * -(y - 0.5f) * 10;
				v.z += z;
				v.Scale(new Vector3(ne.mEffectScale.x, ne.mEffectScale.y, 1));
				verts[i] = v;

				Color uc = cols[i];

				if(uc.a == 1f)
				{
					cols[i] = col;
				} else
				{
					Color fc = c;
					fc.a = uc.a * c.a;
					cols[i] = fc;
				}
			}
		}

	}

}
