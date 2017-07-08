// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;
using AnimationOrTween;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI Tools")]
	[Tooltip("Add a child to a Ngui Element. Optionally set a custom name, position and / or rotation of the created instance. In Addition the option to reference a local GameObject.")]
	public class NguiToolsAddChildAdvanced : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Parent. Strongly recommended to be a child of an NGUI UI Root or an UI Root itself, otherwise creates one in the root of the Hierarchy (wich is probably not the desired outcome).")]
		public FsmGameObject parent;

		[Tooltip("The GameObject to add. Creates Empty if not set.")]
		public FsmGameObject childReference;

		[Tooltip("GameObject name. Leave to null or none for default")]
		public FsmString name;

		[Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
		public FsmVector3 position;

		[Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
		public FsmVector3 rotation;

		[Tooltip("Scale. NOTE: Overrides the scale of the Spawn Point.")]
		public FsmVector3 scale;

		[Tooltip("Use local or world space.")]
		public Space space;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the created object.")]
		public FsmGameObject childInstance;
		
		public override void Reset()
		{
			parent = new FsmGameObject() {UseVariable=true};;
			childReference = null;
			name = new FsmString() {UseVariable=true};
			position = new FsmVector3 { UseVariable = true };
			rotation = new FsmVector3 { UseVariable = true };
			scale = new Vector3(1,1,1);
			space = Space.World;
			childInstance = null;
		}
		
		public override void OnEnter()
		{
			//if an GameObject Reference has been set, create that, otherwise create an Empty GameObject
			if (childReference.Value != null) {
				childInstance.Value = NGUITools.AddChild(parent.Value,childReference.Value);

				//If name has been set, use that as the name for the created GO, otherwise use the name of the set Reference
				if (name != null)
				{
					childInstance.Value.gameObject.name = name.Value;
				}
				else
				{
					childInstance.Value.gameObject.name = childReference.Value.gameObject.name;
				}

			}
			else 
			{
				var _go = new GameObject ("Empty GameObject");
				_go.transform.parent = parent.Value.transform;
				childInstance.Value = _go;
				childInstance.Value.gameObject.name = name.Value;
			}

			if (!position.IsNone)
			{
				if (space == Space.Self)
				{
					childInstance.Value.transform.localPosition = position.Value;
				}
				else
				{
					childInstance.Value.transform.position = position.Value;
				}

			}

			if (!rotation.IsNone)
			{
				if (space == Space.Self)
				{
					childInstance.Value.transform.localRotation = Quaternion.Euler (rotation.Value);
				} 
				else 
				{
					childInstance.Value.transform.rotation = Quaternion.Euler(rotation.Value);
				}
					
			}

			if (!scale.IsNone) {
				childInstance.Value.transform.localScale = scale.Value;
			}
			
			UITable mTable = NGUITools.FindInParents<UITable>(childInstance.Value);
			if (mTable != null) { 
				mTable.repositionNow = true; 
			}
			
			UIGrid mGrid = NGUITools.FindInParents<UIGrid>(childInstance.Value);
			if (mGrid != null) { 
				mGrid.repositionNow = true; 
			}
				
			
			Finish();

		}

	}
}