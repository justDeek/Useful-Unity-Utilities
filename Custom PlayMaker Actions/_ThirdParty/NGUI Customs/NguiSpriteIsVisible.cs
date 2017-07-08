using HutongGames.PlayMaker;
using UnityEngine;

/// <summary>
/// Checks if a NGUI UISprite is visible on Screen.
/// </summary>
[ActionCategory("NGUI")]
[HutongGames.PlayMaker.Tooltip("Checks if a NGUI UISprite is visible on Screen.")]
public class NguiSpriteIsVisible : FsmStateAction
{
    [RequiredField]
    [HutongGames.PlayMaker.Tooltip("NGUI Sprite to check")]
    public FsmOwnerDefault NguiSprite;

		[HutongGames.PlayMaker.Tooltip("Event to send if the Sprite is visible.")]
		public FsmEvent trueEvent;

		[HutongGames.PlayMaker.Tooltip("Event to send if the Sprite is not visible.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;

		public bool everyFrame;

    public override void Reset()
    {
        NguiSprite = null;
				trueEvent = null;
				falseEvent = null;
				storeResult = null;
				everyFrame = false;
    }

		public override void OnEnter()
		{
			DoCheck();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoCheck();
		}

    private void DoCheck()
    {
        // exit if sprite is null
        if (NguiSprite == null)
            return;

        // get the UISprite Component
        UISprite sprite = Fsm.GetOwnerDefaultTarget(NguiSprite).GetComponent<UISprite>();

        // exit if no sprite found
        if (sprite == null)
				{
					Debug.LogWarning("No UISprite Component attached to" + Fsm.GetOwnerDefaultTarget(NguiSprite));
					return;
				}
				var isVisible = sprite.isVisible;
				storeResult.Value = isVisible;
				Fsm.Event(isVisible ? trueEvent : falseEvent);
    }
}
