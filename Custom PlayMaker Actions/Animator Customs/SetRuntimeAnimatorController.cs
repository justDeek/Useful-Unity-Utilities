
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Set or change the runtimeAnimatorController of an Animator Component to switch the animation playing.")]
	public class SetRuntimeAnimatorController : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[ObjectType(typeof(RuntimeAnimatorController))]
		[Tooltip("The new controller to insert into the Animator.")]
		public FsmObject setController;

		[Tooltip("Repeat every frame. Useful when using normalizedTime to manually control the animation.")]
		public bool everyFrame;

		Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			setController = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			// get the animator component
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go == null)
			{
				Finish();
				return;
			}

			_animator = go.GetComponent<Animator>();

			DoAnimatorPlay();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoAnimatorPlay();
		}

		void DoAnimatorPlay()
		{
			if (_animator != null)
			{
				_animator.runtimeAnimatorController = (RuntimeAnimatorController)setController.Value;
			}
		}
	}
}