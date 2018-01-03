
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Sets the alpha value of a single or multiple widgets to the same specified alpha.")]
	public class NguiSetWidgetAlphaMulti : FsmStateActionAdvanced
	{
		[RequiredField]
		[Tooltip("NGUI Widgets to update. Can be UIWidget, UISprite or UIPanel.")]
		public FsmGameObject[] NguiWidgets;

		[RequiredField]
		[HasFloatSlider(0, 1)]
		[Tooltip("The new alpha to assign to the widgets. Set or ease between 0 and 1.")]
		public FsmFloat alpha;

		public override void Reset()
		{
			base.Reset();

			NguiWidgets = new FsmGameObject[1];
			alpha = null;
		}

		public override void OnEnter()
		{
			DoSetWidgetsAlpha();

			if(!everyFrame)
				Finish();
		}

		public override void OnActionUpdate()
		{
			DoSetWidgetsAlpha();
		}

		private void DoSetWidgetsAlpha()
		{
			// exit if objects are null
			if((NguiWidgets == null) || (NguiWidgets.Length == 0) || (alpha == null))
				return;

			// handle each widget
			int j = NguiWidgets.Length;
			for(int i = 0; i < j; i++)
			{
				// get the Widget component (one of the base classes for UISprites)
				UIWidget NWidget = NguiWidgets[i].Value.GetComponent<UIWidget>();
				if(NWidget == null)
				{
					UIPanel NPanel = NguiWidgets[i].Value.GetComponent<UIPanel>();
					if(NPanel == null)
					{
						Debug.LogWarning(NguiWidgets[i].Value.name + " does not contain any Widget Component!");
					} else
					{
						NPanel.alpha = alpha.Value;
					}
				} else
				{
					NWidget.alpha = alpha.Value;
				}
			}
		}
	}
}