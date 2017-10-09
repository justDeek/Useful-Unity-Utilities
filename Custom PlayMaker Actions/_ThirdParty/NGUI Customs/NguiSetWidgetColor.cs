using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Sets the color values of a widget. Set any color value to 'none' to ignore it.")]
	public class NguiSetWidgetColor : FsmStateAction
	{
		[RequiredField]
		[Tooltip("NGUI Widget to update.")]
		public FsmOwnerDefault NguiWidget;

		[Tooltip("The new color to assign to the widget. Set to 'none' to ignore.")]
		public FsmColor color;

		[Tooltip("The color for the top gradient of the UISprite or UILabel. Set to 'none' to ignore.")]
		public FsmColor gradientTop;

		[Tooltip("The color for the bottom gradient of the UISprite or UILabel. Set to 'none' to ignore.")]
		public FsmColor gradientBottom;

		[Tooltip("When true, runs every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			NguiWidget = null;
			color = Color.white;
			gradientTop = Color.white;
			gradientBottom = new Color(0.7f, 0.7f, 0.7f, 1f);
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetWidgetColor();

			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoSetWidgetColor();
		}

		private void DoSetWidgetColor()
		{
			// exit if objects are null
			if (NguiWidget == null)
				return;

			// get the object as a widget
			UIWidget NWidget = Fsm.GetOwnerDefaultTarget(NguiWidget).GetComponent<UIWidget>();

			// exit if no widget
			if (NWidget == null)
				return;

			// set color value
			if (!color.IsNone)
			{
				NWidget.color = color.Value;
			}

			UISprite NSprite = Fsm.GetOwnerDefaultTarget(NguiWidget).GetComponent<UISprite>();
			if (NSprite != null)
			{
				if (!gradientTop.IsNone)
				{
					NSprite.gradientTop = gradientTop.Value;
				}
				if (!gradientTop.IsNone)
				{
					NSprite.gradientBottom = gradientBottom.Value;
				}
			}

			UILabel NLabel = Fsm.GetOwnerDefaultTarget(NguiWidget).GetComponent<UILabel>();
			if (NLabel != null)
			{
				if (!gradientTop.IsNone)
				{
					NLabel.gradientTop = gradientTop.Value;
				}
				if (!gradientTop.IsNone)
				{
					NLabel.gradientBottom = gradientBottom.Value;
				}
			}
		}
	}
}

