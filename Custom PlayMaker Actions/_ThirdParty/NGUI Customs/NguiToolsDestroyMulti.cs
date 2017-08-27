using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI Tools")]
	[Tooltip("Destroys multiple GameObjects with NGUI components (removes the object from the parent's hirarchy before destroying). If the specified GameObjects don't have any NGUI component attached, they are getting destroyed the normal way. Optionally reposition afterwards or only destroy the children of the GameObjects.")]
	public class NguiToolsDestroyMulti : FsmStateAction
	{
		[RequiredField]
		[ArrayEditor(VariableType.GameObject)]
		[Tooltip("The Game Objects to destroy. Destroys them immediately when in Editor.")]
		public FsmGameObject[] gameObjects;

		[Tooltip("Reposition the Table or Grid the GameObjects are children.")]
		public FsmBool reposition;

		[Tooltip("Only destroys all children of the specified GameObjects.")]
		public FsmBool onlyDestroyChildren;

		public override void Reset()
		{
			gameObjects = new FsmGameObject[1];
			reposition = true;
			onlyDestroyChildren = false;
		}

		public override void OnEnter()
		{
			if (gameObjects.Length != 0)
			{
				foreach (var go in gameObjects)
				{
					if (go.Value != null)
					{
						GameObject currentGOParent = null;
						if (go.Value.transform.parent == null)
						{
							reposition.Value = false;
							UnityEngine.Debug.Log("Couldn't reposition \"" + go.Value.name + "\" since it has no parent.");
						}
						else
						{
							currentGOParent = go.Value.transform.parent.gameObject;
						}

						if (go.Value.GetComponent(typeof(UIWidgetContainer)) == null)
						{
							UnityEngine.Debug.Log("GameObject \"" + go.Value.name +
												  "\" doesn't contain any NGUI Component. Trying to destroy it the normal way...");
							if (onlyDestroyChildren.Value)
							{
								for (int i = 0; i < go.Value.transform.childCount; i++)
								{
									GameObject.Destroy(go.Value.transform.GetChild(i).gameObject);
								}
							}
							else
							{
								GameObject.Destroy(go.Value);
							}
						}
						else
						{
							if (onlyDestroyChildren.Value)
							{
								go.Value.transform.DestroyChildren();
							}
							else
							{
								NGUITools.Destroy(go.Value);
							}
						}

						//Reposition Table or Grid if prevalent
						if (currentGOParent != null && reposition.Value)
						{
							UITable mTable = NGUITools.FindInParents<UITable>(currentGOParent);
							if (mTable != null)
							{
								mTable.repositionNow = true;
							}

							UIGrid mGrid = NGUITools.FindInParents<UIGrid>(currentGOParent);
							if (mGrid != null)
							{
								mGrid.repositionNow = true;
							}
						}
					}
				}
			}
			Finish();
		}
	}
}