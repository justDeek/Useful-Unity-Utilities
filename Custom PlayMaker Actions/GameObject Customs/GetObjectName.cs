// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets the name of an Object and stores it in a String Variable.")]
	public class GetObjectName : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Object to get the name of.")]
		public FsmObject specifyObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Name of the specified Object.")]
		public FsmString storeName;

		public bool everyFrame;

		public override void Reset()
		{
			specifyObject = new FsmObject { UseVariable = true};
			storeName = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetObjectName();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetObjectName();
		}

		void DoGetObjectName()
		{
			var go = specifyObject.Value;

			storeName.Value = go != null ? go.name : "";
		}
	}
}