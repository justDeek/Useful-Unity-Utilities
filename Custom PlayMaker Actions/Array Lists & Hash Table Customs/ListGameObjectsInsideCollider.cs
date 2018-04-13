//License: Attribution 4.0 International (CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
//Author: Deek

//Based on dudebxl's action "ArrayListFindGameObjectsInsideCollider", searches for Collider's instead of Sprites
//http://hutonggames.com/playmakerforum/index.php?topic=11754.0

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=15458.0")]
	[Tooltip("Store all active GameObjects with a collider component that are inside the specified collider while optionally filtering by tag and/or layer. NOTE: Tags and layers must be declared in the tag/layer manager before using them.")]
	public class ListGameObjectsInsideCollider : ArrayListActions
	{
		[ActionSection("Set up")]

		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject).")]
		public FsmString reference;

		[RequiredField]
		[Tooltip("The collider to check against other intersecting colliders.")]
		[ObjectType(typeof(Collider))]
		public FsmObject colliderTarget;


		[ActionSection("Filter")]

		[UIHint(UIHint.Tag)]
		[Tooltip("Optionally filter by tag.")]
		public FsmString tag;

		[Title("Incl Layer Filter")]
		[Tooltip("Also filter by layer?")]
		public FsmBool layerFilterOn;

		[UIHint(UIHint.Layer)]
		public int layer;


		[ActionSection("Optionally")]

		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.GameObject)]
		[Tooltip("Store the found GameObjects in a GameObject-Array.")]
		public FsmArray storeArray;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the amount of matching GameObjects.")]
		public FsmInt storeAmount;

		[Tooltip("Wheter to update on every frame.")]
		public FsmBool everyFrame;

		private List<GameObject> tempList = new List<GameObject>();

		public override void Reset()
		{
			gameObject = null;
			colliderTarget = null;
			reference = null;
			tag = "Untagged";
			layerFilterOn = false;
			layer = 0;
			storeArray = null;
			storeAmount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			FindGOByTag();

			if(!everyFrame.Value) Finish();
		}

		public override void OnUpdate()
		{
			FindGOByTag();
		}

		public void FindGOByTag()
		{
			if(!SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
				return;

			if(!isProxyValid()) return;

			GameObject[] objtag = Object.FindObjectsOfType<GameObject>();

			if(objtag.Length == 0) return;

			proxy.arrayList.Clear();
			tempList.Clear();

			Collider temp = colliderTarget.Value as Collider;
			Bounds colliderBounds = temp.bounds;

			for(int i = 0; i < objtag.Length; i++)
			{
				GameObject go = objtag[i];

				if(!string.IsNullOrEmpty(tag.Value) && go.tag != tag.Value)
					continue;

				Collider tmpCol = objtag[i].GetComponent<Collider>();
				if(!tmpCol) continue;

				Bounds currBounds = tmpCol.bounds;
				bool insideCollider = colliderBounds.Intersects(currBounds);

				if(insideCollider == true)
				{
					if(layerFilterOn.Value == true
					&& objtag[i].gameObject.layer != layer) continue;

					tempList.Add(objtag[i]);
				}
			}

			proxy.arrayList.InsertRange(0, tempList);
			storeArray.Values = tempList.ToArray();
			storeAmount.Value = tempList.Count;
		}

		//explicitly declare using OnGUI
		public override void OnPreprocess()
		{
			Fsm.HandleOnGUI = true;
		}

		public override void OnGUI()
		{
			//reset tag value to 'Untagged' if toggled between None and tag selection
			if(string.IsNullOrEmpty(tag.Value) && !tag.UsesVariable) tag.Value = "Untagged";
		}

		public override string ErrorCheck()
		{
			if(colliderTarget.Value == null || colliderTarget.IsNone)
				return "Please specify the Collider Target!";

			return "";
		}
	}
}
