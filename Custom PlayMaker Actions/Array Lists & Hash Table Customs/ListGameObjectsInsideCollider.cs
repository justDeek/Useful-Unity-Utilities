//License: Attribution 4.0 International (CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
//Author: Deek

//Based on dudebxl's action "ArrayListFindGameObjectsInsideCollider", searches for Collider's instead of Sprites
//http://hutonggames.com/playmakerforum/index.php?topic=11754.0

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

		[UIHint(UIHint.Layer)]
		public FsmInt layer;
		
		[Tooltip("Wheter to exclude colliders that have the 'Is Trigger' flag set.")]
		public FsmBool ignoreTriggers;


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

		public override void Reset()
		{
			gameObject = null;
			colliderTarget = null;
			reference = null;
			tag = new FsmString { UseVariable = true };
			layer = new FsmInt {UseVariable = true};
			ignoreTriggers = false;
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
			GameObject srcGO = Fsm.GetOwnerDefaultTarget(gameObject);
		
			if(!SetUpArrayListProxyPointer(srcGO, reference.Value)) return;
			if(!isProxyValid()) return;

			GameObject[] objtag = Object.FindObjectsOfType<GameObject>();

			if(objtag.Length == 0) return;

			proxy.arrayList.Clear();
			List<Collider> tmpResult;
			List<GameObject> tempList = new List<GameObject>();

			Collider temp = colliderTarget.Value as Collider;

			Vector3 rot = srcGO.transform.rotation.eulerAngles;
			Vector3 ext = temp.bounds.extents;

			if (temp.GetType().ToString().Contains("Box"))
			{
				float diffX = rot.x + 45f;
				while (diffX > 90f) diffX -= 90f;
				diffX /= 360;
				if(diffX > 0.125f) diffX -= 0.125f;
				else diffX = 0.125f - diffX;
				
				float diffY = rot.y + 45f;
				while (diffY > 90f) diffY -= 90f;
				diffY /= 360;
				if(diffY > 0.125f) diffY -= 0.125f;
				else diffY = 0.125f - diffY;
				
				float diffZ = rot.z + 45f;
				while (diffZ > 90f) diffZ -= 90f;
				diffZ /= 360;
				if(diffZ > 0.125f) diffZ -= 0.125f;
				else diffZ = 0.125f - diffZ;

				Vector3 extents = new Vector3(ext.x * (1f - diffX*2f), ext.y * (1f - diffY*2f), ext.z * (1f - diffZ*2f));
				
				Collider[] results = Physics.OverlapBox(temp.bounds.center, extents, srcGO.transform.localRotation);

				tmpResult = results.ToList();
			}
			else
			{
				SphereCollider sphere = temp as SphereCollider;
				Collider[] results = Physics.OverlapSphere(temp.bounds.center, sphere.radius);

				tmpResult = results.ToList();
			}
			
			foreach (var result in tmpResult)
			{
				GameObject go = result.gameObject;
				
				//skip source GameObject
				if(srcGO == go) continue;

				//skip triggers if set to ignore them
				if (ignoreTriggers.Value && result.isTrigger) continue;
				
				//skip GameObjects that don't have the specified tag
				if(!tag.IsNone && !string.IsNullOrEmpty(tag.Value) && !go.CompareTag(tag.Value)) continue;
				
				//skip GameObjects that don't match the given layer if included
				if(go.layer != layer.Value) continue;
				
				tempList.Add(result.gameObject);
			}
			
			proxy.arrayList.InsertRange(0, tempList);
			if(!storeArray.IsNone) storeArray.Values = tempList.ToArray();
			if(!storeAmount.IsNone) storeAmount.Value = tempList.Count;
		}

		//explicitly declare using OnGUI
		public override void OnPreprocess()
		{
			Fsm.HandleOnGUI = true;
		}

		public override void OnGUI()
		{
			//reset tag value to 'Untagged' if toggled between None and tag selection
			if(string.IsNullOrEmpty(tag.Value) && !tag.UsesVariable)
				tag.Value = "Untagged";
		}

		public override string ErrorCheck()
		{
			if(colliderTarget == null || colliderTarget.Value == null || colliderTarget.IsNone)
				return "Please specify the Collider Target!";

			return "";
		}
	}
}
