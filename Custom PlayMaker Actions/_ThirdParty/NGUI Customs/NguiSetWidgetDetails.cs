
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI Tools")]
	[Tooltip("Change one or many parameters of any NGUI Component. Currently supported: UIWidget, UIPanel, UISprite.")]
	public class NguiSetWidgetDetails : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The target GameObject with the NGUI Components attached.")]
		public FsmOwnerDefault target;

		[Tooltip("Enable or disable the Component.")]
		public FsmBool enabled;

		[Tooltip("Set a custom alpha value for the attached widget. (O = Invisible, 1=fully visible)")]
		[HasFloatSlider(0, 1)]
		public FsmFloat widgetAlpha;

		[Tooltip("Set a custom Depth for the attached widget.")]
		public FsmInt widgetDepth;

		[Tooltip("Apply changes every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			target = null;
			enabled = new FsmBool() {UseVariable = true};
			widgetDepth = new FsmInt() {UseVariable = true};
			widgetAlpha = new FsmFloat() {UseVariable = true};
			everyFrame = false;
		}

		public override void OnEnter()
    {
			DoSetDetails();
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetDetails();
		}

		void DoSetDetails()
		{
			var _go = Fsm.GetOwnerDefaultTarget(target);
			if (_go == null)
			{
				return;
			}

			if (!widgetDepth.IsNone || !widgetAlpha.IsNone)
			{
				// get the object as a widget
				UIWidget NWidget = _go.GetComponent<UIWidget>();
				UIPanel NPanel = _go.GetComponent<UIPanel>();
				UISprite NSprite = _go.GetComponent<UISprite>();
				// exit if no widget
				if (NWidget == null && NPanel == null && NSprite == null)
				{
					Debug.LogWarning(_go.name + " does not contain any Widget Component!");
				}
				else
				{
					if (NWidget != null)
					{
						if (!enabled.IsNone)
							NWidget.enabled = enabled.Value;

						if (!widgetDepth.IsNone)
						NWidget.depth = widgetDepth.Value;

						if (!widgetAlpha.IsNone)
						NWidget.alpha = widgetAlpha.Value;
					}
					if (NPanel != null)
					{
						if (!enabled.IsNone)
							NPanel.enabled = enabled.Value;

						if (!widgetDepth.IsNone)
						NPanel.depth = widgetDepth.Value;

						if (!widgetAlpha.IsNone)
						NPanel.alpha = widgetAlpha.Value;
					}
					if (NSprite != null)
					{
						if (!enabled.IsNone)
							NSprite.enabled = enabled.Value;

						if (!widgetDepth.IsNone)
						NSprite.depth = widgetDepth.Value;

						if (!widgetAlpha.IsNone)
						NSprite.alpha = widgetAlpha.Value;
					}
				}
			}
		}

	}
}
