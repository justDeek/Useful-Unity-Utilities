
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI Tools")]
	[Tooltip("Add a child to a Ngui Element. Optionally set a custom name, position and / or rotation of the created instance. Additionaly the ability to reference a local GameObject as parent.")]
	public class NguiToolsAddChildAdvanced : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Parent. Strongly recommended to be a child of an NGUI UI Root or an UI Root itself, otherwise creates one in the root of the Hierarchy (wich is probably not the desired outcome).")]
		public FsmGameObject parent;

		[Tooltip("The GameObject to add. Creates Empty if not set.")]
		public FsmGameObject childReference;

		[Tooltip("GameObject name. Leave to None for default (actual name of the GameObject).")]
		public FsmString name;

		[Tooltip("Position.")]
		public FsmVector3 position;

		[Tooltip("Rotation.")]
		public FsmVector3 rotation;

		[Tooltip("Scale.")]
		public FsmVector3 scale;

		[Tooltip("Use local or world space.")]
		public Space space;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the created object in a variable.")]
		public FsmGameObject storeChildInstance;

		[Tooltip("Wheter to reposition the UITable or UIGrid the created Object is a child of (should be disabled when creating a lot of items to manually reposition afterwards).")]
		public FsmBool reposition;

		public override void Reset()
		{
			parent = new FsmGameObject() { UseVariable = true }; ;
			childReference = null;
			name = new FsmString() { UseVariable = true };
			position = new FsmVector3 { UseVariable = true };
			rotation = new FsmVector3 { UseVariable = true };
			scale = new Vector3(1, 1, 1);
			space = Space.Self;
			storeChildInstance = null;
			reposition = true;
		}

		public override void OnEnter()
		{
			//if an GameObject Reference has been set, create that, otherwise create an Empty GameObject
			if(!childReference.IsNone && childReference.Value != null)
			{
				storeChildInstance.Value = NGUITools.AddChild(parent.Value, childReference.Value);

				//If name has been set, use that as the name for the created GO, otherwise use the name of the set Reference
				storeChildInstance.Value.name = !name.IsNone ? name.Value : childReference.Value.gameObject.name;
			} else
			{
				var _go = new GameObject("Empty GameObject");
				_go.transform.parent = parent.Value.transform;

				_go.name = !name.IsNone ? name.Value : _go.gameObject.name;

				storeChildInstance.Value = _go;
				storeChildInstance.Value.layer = parent.Value.layer;
			}

			if(!position.IsNone)
			{
				if(space == Space.Self)
					storeChildInstance.Value.transform.localPosition = position.Value;
				else
					storeChildInstance.Value.transform.position = position.Value;
			}

			if(!rotation.IsNone)
			{
				if(space == Space.Self)
					storeChildInstance.Value.transform.localRotation = Quaternion.Euler(rotation.Value);
				else
					storeChildInstance.Value.transform.rotation = Quaternion.Euler(rotation.Value);
			}

			if(!scale.IsNone)
				storeChildInstance.Value.transform.localScale = scale.Value;

			if(reposition.Value)
			{
				UITable mTable = NGUITools.FindInParents<UITable>(storeChildInstance.Value);
				if(mTable != null)
					mTable.repositionNow = true;

				UIGrid mGrid = NGUITools.FindInParents<UIGrid>(storeChildInstance.Value);
				if(mGrid != null)
					mGrid.repositionNow = true;
			}

			Finish();
		}
	}
}
