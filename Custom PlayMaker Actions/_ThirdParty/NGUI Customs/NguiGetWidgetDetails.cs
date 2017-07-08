
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI Tools")]
	[Tooltip("Retrieve one or many parameters of any NGUI Component. Currently supported: UIWidget, UIPanel, UISprite.")]
	public class NguiGetWidgetDetails : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The target GameObject with the NGUI Components attached.")]
		public FsmOwnerDefault target;

		[Tooltip("Get the current Widgets alpha Value.")]
		public FsmFloat widgetAlpha;

		[Tooltip("Get the depth of the attached widget.")]
		public FsmInt widgetDepth;

		[Tooltip("Store the attached Widget, if any. Includes UIPanel, UISprite, UIWidget and the like.")]
		public FsmObject storeWidget;

		[UIHint(UIHint.Variable)]
		[Tooltip("Returns if the GameObject contains any Widgets.")]
		public FsmBool containsWidget;

		[Tooltip("Get values every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			target = null;
			widgetDepth = new FsmInt() {UseVariable = true};
			widgetAlpha = new FsmFloat() {UseVariable = true};
			storeWidget = new FsmObject() {UseVariable = true};
			containsWidget = false;
			everyFrame = false;
		}

		public override void OnEnter()
    {
			DoGetDetails();
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoGetDetails();
		}

		void DoGetDetails()
		{
			var _go = Fsm.GetOwnerDefaultTarget(target);
			if (_go == null)
			{
				return;
			}

			if (!widgetDepth.IsNone || !widgetAlpha.IsNone || !storeWidget.IsNone)
			{
				// get the object as a widget
				UIWidget NWidget = _go.GetComponent<UIWidget>();
				UIPanel NPanel = _go.GetComponent<UIPanel>();
				UISprite NSprite = _go.GetComponent<UISprite>();
				// exit if no widget
				if (NWidget == null && NPanel == null && NSprite == null)
				{
					if (!containsWidget.IsNone)
					{
						Debug.LogWarning(_go.name + " does not contain any Widget Component!");
					}
					else
					{
						containsWidget.Value = false;
					}
				}
				else
				{
					containsWidget.Value = true;

					if (NWidget != null)
					{
						if (!widgetDepth.IsNone)
						widgetDepth.Value = NWidget.depth;

						if (!widgetAlpha.IsNone)
						widgetAlpha.Value = NWidget.alpha;

						if (!storeWidget.IsNone)
						storeWidget.Value = NWidget;
					}
					if (NPanel != null)
					{
						if (!widgetDepth.IsNone)
						widgetDepth.Value = NPanel.depth;

						if (!widgetAlpha.IsNone)
						widgetAlpha.Value = NPanel.alpha;

						if (!storeWidget.IsNone)
						storeWidget.Value = NPanel;
					}
					if (NSprite != null)
					{
						if (!widgetDepth.IsNone)
						widgetDepth.Value = NSprite.depth;

						if (!widgetAlpha.IsNone)
						widgetAlpha.Value = NSprite.alpha;

						if (!storeWidget.IsNone)
						storeWidget.Value = NSprite;
					}
				}
			}
		}
	}
}
