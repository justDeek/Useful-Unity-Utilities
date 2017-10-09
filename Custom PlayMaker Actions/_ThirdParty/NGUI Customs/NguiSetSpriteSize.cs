
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI Tools")]
	[Tooltip("Change the size of a NGUI UISprite component. Alternatively snap to sprite size (if set ignores the size values).")]
	public class NguiSetSpriteSize : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(UISprite))]
		[Tooltip("The target GameObject with the NGUISprite attached.")]
		public FsmOwnerDefault target;

		[Tooltip("")]
		public FsmVector2 size;

		[Tooltip("Adjust the scale to make it Pixel Perfect.")]
		public FsmBool makePixelPerfect;

		[Tooltip("Resize Collider to the UISprite size if any is attached.")]
		public FsmBool resizeCollider;

		[Tooltip("Apply changes every frame.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			target = null;
			size = Vector2.zero;
			makePixelPerfect = false;
			resizeCollider = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetDetails();
			if (!everyFrame.Value)
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

			UISprite nSprite = _go.GetComponent<UISprite>();

			nSprite.width = (int)size.Value.x;
			nSprite.height = (int)size.Value.y;

			if (makePixelPerfect.Value)
			{
				nSprite.MakePixelPerfect();
			}

			if (resizeCollider.Value)
			{
				nSprite.ResizeCollider();
			}

			nSprite.MarkAsChanged();
		}

	}
}
