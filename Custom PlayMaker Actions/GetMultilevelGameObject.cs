// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Get the Game Object and name of a higher parent or lower child specified by an index.")]
	public class GetMultilevelGameObject : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to start from.")]
		public FsmOwnerDefault startFrom;

		[Tooltip ("How many 'Directories' to go up or down. For example '2' would be the grandparent of the GameObject this FSM is attached to, '3' the parent of that one ... and so on. 0 returns the Owner. Anything below 0 goes in the other direction (-2 = first Child of the first Child). You can also set a very high number to definitely get the root.")]
		public FsmInt index;

		[UIHint(UIHint.Variable)]
		[Tooltip("The final GameObject it reached.")]
		public FsmGameObject storeResult;

		[Tooltip("The Name of the final GameObject it reached.")]
		public FsmString resultName;

		[Tooltip("Follow the path it takes (throws Debug.Log's for every GameObject it passes). If there are several Log entries with the same GameObject, it means that the given index is higher than the GameObject has parents or lower than it has children.")]
		public FsmBool debug;

		public override void Reset()
		{
			startFrom = null;
			index = 1;
			storeResult = null;
			resultName = null;
			debug = false;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(startFrom);
			if (go != null) 
			{
				//Get Owner
				if (index.Value == 0)
				{
					storeResult.Value = Owner;
					resultName = storeResult.Value.name;
					if (debug.Value == true)
					{
						Debug.Log ("GetMultilevelGameObject - Owner: " + Owner);
					}

				}

				//Get ascending parent
				if (index.Value >= 1)
				{
					for (int i = 1; i < index.Value; ++i) {

							go = go.transform.parent == null ? go : go.transform.parent.gameObject;	

							if (debug.Value == true)
							{
								var j = i + 1;
								Debug.Log ("GetMultilevelGameObject - Parent " + j.ToString () + ": " + go.transform.gameObject.name);
							}

							storeResult.Value = go;
							resultName = storeResult.Value.name;
					}
				}

				//Get descending Child
				if (index.Value < 0)
				{
					for (int i = 0; i > index.Value; i--) {
							go = go.transform.GetChild (0) == null ? go.transform.gameObject : go.transform.GetChild (0).gameObject;	

							if (debug.Value == true)
							{
								//Invert and add 1 to current Index
								var j = (i * (-1)) + 1;
								Debug.Log ("GetMultilevelGameObject - Child " + j.ToString () + ": " + go.transform.gameObject.name);
							}

							storeResult.Value = go;
							resultName = storeResult.Value.name;
					}
				}

			}
			//If StartFrom is Null
			else
			{
				storeResult.Value = null;
				resultName = "None";
				if (debug.Value == true)
					Debug.Log ("GetMultilevelGameObject - NullReferenceException: 'Start From' is null");
			}
			Finish();
		}
	}
}