using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(GDMConstants.ActionCategory)]
	[Tooltip("Changes: Lets you specify the reference name if several Array List Proxy Components coexist on the target GameObject. Also targets the owner by default and allows to null or none Arguments to be set.")]
	public class GDELoadStringListCustom : FsmStateAction
	{
		[RequiredField]
		[Tooltip(GDMConstants.ItemNameTooltip)]
		public FsmString ItemName;

		[RequiredField]
		[Tooltip(GDMConstants.FieldNameTooltip)]
		public FsmString FieldName;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("The target GameObject (requires an Array List Component)")]
		public FsmOwnerDefault target;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		private PlayMakerArrayListProxy proxy;

		public override void Reset()
		{
			ItemName = null;
			FieldName = null;
			target = null;
			reference = null;
			proxy = null;
		}

		public override void OnEnter()
		{
			GameObject _go = Fsm.GetOwnerDefaultTarget(target);
			//Get all Proxy Components
			PlayMakerArrayListProxy[] proxies = _go.GetComponents<PlayMakerArrayListProxy>();

			if(reference.Value != "" || reference.Value != null)
			{
				//Check if more than one Proxy Component exists on the target
				if(proxies.Length > 0)
				{
					foreach(PlayMakerArrayListProxy iProxy in proxies)
					{
						if(iProxy.referenceName == reference.Value)
						{
							proxy = iProxy;
						} else
						{
							// Debug.LogWarning("No Array List with the Reference " + reference.Value + " in " + _go.name + " found!");
							// Finish();
						}
					}
				}
			} else
			{
				proxy = _go.GetComponent("" + "PlayMakerArrayListProxy") as PlayMakerArrayListProxy;
			}

			try
			{
				Dictionary<string, object> data;

				if(proxy == null)
					LogError("ArrayMaker Proxy is null!");

				if(GDEDataManager.Get(ItemName.Value, out data) && proxy != null)
				{
					List<string> val;
					data.TryGetStringList(FieldName.Value, out val);

					proxy.AddRange(val, string.Empty);
				} else
				{
					//LogError(string.Format(GDMConstants.ErrorLoadingValue, "string array", ItemName.Value, FieldName.Value));
				}
			} catch(System.Exception ex)
			{
				UnityEngine.Debug.LogException(ex);
			} finally
			{
				Finish();
			}
		}
	}
}

#endif
