// (c) copyright Hutong Games, LLC 2010-2011. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.UnityObject)]
	[Tooltip("Activates/deactivates multiple Components.\nUse this to hide/show areas, or enable/disable many Behaviours at once. Some Components Might not work!")]
	public class ActivateMultipleComponents : FsmStateAction
	{
		[RequiredField]
		public FsmObject[] components;

		[RequiredField]
		[Tooltip("Check to activate, uncheck to deactivate Game Object.")]
		public FsmBool Activate;

		[Tooltip("Reset the components when exiting this state. Useful if you want an component to be active only while this state is active.")]
		public bool resetOnExit;

		// store the components that we activated on enter
		// so that we can de-activate them on exit.
		public List<Behaviour> _;

		public override void Reset()
		{
			components = new FsmObject[3];
			// components[0] = null;
			Activate = false;
			resetOnExit = false;
		}

		public override void OnEnter()
		{
			DoActivateComponents();
			Finish();
		}

		public override void OnExit()
		{
			// the stored component might be invalid now
			if (components == null) return;

			if (resetOnExit)
			{
				foreach (Behaviour tmp in _)
				tmp.enabled = !Activate.Value;
			}

		}

		void DoActivateComponents()
		{
			_.Clear();
			foreach (var co in components)
			{
				var currentType = co.Value.GetType();
				if (co.Value == null)
				{
					LogError("No Component Selected");
					Debug.Log("No Component");
				} else if (currentType == typeof(GameObject)) {
					LogError("Components can't be of type 'GameObject'!");
				}	else { //if Behaviour
					Behaviour be = co.Value as Behaviour;
					if (be != null) {
						be.enabled = Activate.Value;
					} else { //if Renderer
						Renderer rend = co.Value as Renderer;
						if (rend != null) {
							rend.enabled = Activate.Value;
						} else { //if Collider
							Collider col = co.Value as Collider;
							if (col != null) {
								col.enabled = Activate.Value;
							} else { //if none
								string status;
								if (Activate.Value) {
									status = "enabled";
								} else {
									status = "disabled";
								}
								LogError("Component " + co.Value.GetType().ToString() + " on " + co.Value.name + " can't be " + status + " with this action!");
							}
						}
					}
					_.Add(be);
				}
			}
		}
	}
}
