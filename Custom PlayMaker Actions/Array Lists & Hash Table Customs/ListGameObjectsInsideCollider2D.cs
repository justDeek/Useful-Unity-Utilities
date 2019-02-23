//License: Attribution 4.0 International (CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
//Author: Deek

//Based on dudebxl's action "ArrayListFindGameObjectsInsideCollider2D", searches for Collider2D's instead of Sprites
//http://hutonggames.com/playmakerforum/index.php?topic=11754.0

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=15458.0")]
	[Tooltip("Store all active GameObjects with a collider2D component that are inside the specified collider2D while optionally filtering by tag and/or layer. NOTE: Tags and layers must be declared in the tag/layer manager before using them.")]
	public class ListGameObjectsInsideCollider2D : ArrayListActions
	{
		[ActionSection("Set up")]

		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject).")]
		public FsmString reference;

		[RequiredField]
		[Tooltip("The collider2D to check against other intersecting collider2Ds.")]
		[ObjectType(typeof(Collider2D))]
		public FsmObject collider2DTarget;


		[ActionSection("Filter")]

		[UIHint(UIHint.Tag)]
		[Tooltip("Optionally filter by tag. Set to 'None' to ignore this check.")]
		public FsmString tag;

		[Tooltip("Optionally filter by layer. Set to 'None' to ignore this check.")]
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
			collider2DTarget = null;
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
			FindOverlappingColliders();

			if(!everyFrame.Value) Finish();
		}

		public override void OnUpdate()
		{
			FindOverlappingColliders();
		}

		public void FindOverlappingColliders()
		{
			GameObject srcGO = Fsm.GetOwnerDefaultTarget(gameObject);
		
			if(!SetUpArrayListProxyPointer(srcGO, reference.Value)) return;
			if(!isProxyValid()) return;

			//reset lists
			proxy.arrayList.Clear();
			
			List<GameObject> tempList = new List<GameObject>();

			Collider2D col = collider2DTarget.Value as Collider2D;
			
			ContactFilter2D filter = new ContactFilter2D();
			filter.NoFilter();
			
			List<Collider2D> results = new List<Collider2D>();
			col.OverlapCollider(filter, results);

			foreach (var result in results)
			{
				GameObject go = result.gameObject;

				//skip triggers if set to ignore them
				if (ignoreTriggers.Value && result.isTrigger) continue;
				
				//skip GameObjects that don't have the specified tag
				if(!tag.IsNone && !string.IsNullOrEmpty(tag.Value) && !go.CompareTag(tag.Value)) continue;
				
				//skip GameObjects that don't match the given layer if included
				if(go.layer != layer.Value) continue;
				
				tempList.Add(go);
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
			if(collider2DTarget == null || collider2DTarget.Value == null || collider2DTarget.IsNone)
				return "Please specify the Collider2D Target!";

			return "";
		}
	}
}
