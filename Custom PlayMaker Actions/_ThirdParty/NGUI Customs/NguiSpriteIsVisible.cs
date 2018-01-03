
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	/// <summary>
	/// Checks if a NGUI UISprite is visible on Screen.
	/// </summary>
	[ActionCategory("NGUI")]
	[Tooltip("Checks if a NGUI UISprite is visible on Screen.")]
	public class NguiSpriteIsVisible : FsmStateActionAdvanced
	{
		[RequiredField]
		[Tooltip("NGUI Sprite to check")]
		public FsmOwnerDefault NguiSprite;

		[Tooltip("Event to send if the Sprite is visible.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if the Sprite is not visible.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;

		public override void Reset()
		{
			base.Reset();

			NguiSprite = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoCheck();

			if(!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			DoCheck();
		}

		private void DoCheck()
		{
			// exit if sprite is null
			if(NguiSprite == null)
				return;

			// get the UISprite Component
			UISprite sprite = Fsm.GetOwnerDefaultTarget(NguiSprite).GetComponent<UISprite>();

			// exit if no sprite found
			if(sprite == null)
			{
				Debug.LogWarning("No UISprite Component attached to" + Fsm.GetOwnerDefaultTarget(NguiSprite));
				return;
			}
			var isVisible = sprite.isVisible;
			storeResult.Value = isVisible;
			Fsm.Event(isVisible ? trueEvent : falseEvent);
		}
	}
}