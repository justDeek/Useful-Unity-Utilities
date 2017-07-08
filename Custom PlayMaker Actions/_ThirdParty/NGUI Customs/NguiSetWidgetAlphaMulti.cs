using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("NGUI")]
[HutongGames.PlayMaker.Tooltip("Sets the alpha value of a single or multiple widgets to the same specified alpha.")]
public class NguiSetWidgetAlphaMulti : FsmStateAction
{
    [RequiredField]
    [HutongGames.PlayMaker.Tooltip("NGUI Widgets to update. Can be UIWidget, UISprite or UIPanel.")]
    public FsmGameObject[] NguiWidgets;

    [RequiredField]
		[HasFloatSlider(0, 1)]
    [HutongGames.PlayMaker.Tooltip("The new alpha to assign to the widgets. Set or ease between 0 and 1.")]
    public FsmFloat alpha;

    [HutongGames.PlayMaker.Tooltip("If set, runs on every frame until state change.")]
    public bool everyFrame;

    public override void Reset()
    {
        NguiWidgets = new FsmGameObject[1];
        alpha = null;
        everyFrame = false;
    }

    public override void OnEnter()
    {
        DoSetWidgetsAlpha();

        if (!everyFrame)
            Finish();
    }

    public override void OnUpdate()
    {
        DoSetWidgetsAlpha();
    }

    private void DoSetWidgetsAlpha()
    {
        // exit if objects are null
        if ((NguiWidgets == null) || (NguiWidgets.Length == 0) || (alpha == null))
            return;

        // handle each widget
        int j = NguiWidgets.Length;
        for (int i = 0; i < j; i++)
        {
            // get the object as a widget
            UIWidget NWidget = NguiWidgets[i].Value.GetComponent<UIWidget>();
            UIPanel NPanel = NguiWidgets[i].Value.GetComponent<UIPanel>();
            UISprite NSprite = NguiWidgets[i].Value.GetComponent<UISprite>();
            // exit if no widget
            if (NWidget == null && NPanel == null && NSprite == null)
            {
              Debug.LogWarning(NguiWidgets[i].Value.name + " does not contain any Widget Component!");
              continue;
            }
            else
            {
              if (NWidget != null)
              {
                NWidget.alpha = alpha.Value;
              }
              if (NPanel != null)
              {
                NPanel.alpha = alpha.Value;
              }
              if (NSprite != null)
              {
                NSprite.alpha = alpha.Value;
              }
            }
        }
    }
}
