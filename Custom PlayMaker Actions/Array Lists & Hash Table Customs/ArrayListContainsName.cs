//License: Attribution 4.0 International (CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
//Author: Deek

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=15458.0")]
	[Tooltip("Search for an item in an ArrayList by name.")]
	public class ArrayListContainsName : ArrayListActions
	{
		[ActionSection("Set up")]

		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component.")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject).")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;


		[ActionSection("Data")]

		[RequiredField]
		[Tooltip("The variable-name to check.")]
		public FsmString variableName;

		[Tooltip("If the array items can contain the given variable name. When unset, they have to match.")]
		public FsmBool contains;

		[ActionSection("Result")]

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the index of the found item.")]
		public FsmInt indexOf;

		[HideTypeFilter]
		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the resulting variable if the ArrayList contains it.")]
		public FsmVar storeFoundResult;

		[Tooltip("Event sent if this arraList contains that element.")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent itemFoundEvent;

		[Tooltip("Event sent if this arraList does not contains that element.")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent itemNotFoundEvent;


		public override void Reset()
		{
			gameObject = null;
			reference = null;
			variableName = null;
			contains = false;
			indexOf = null;
			storeFoundResult = new FsmVar();
			itemFoundEvent = null;
			itemNotFoundEvent = null;
		}

		public override void OnEnter()
		{
			if(SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
				DoesArrayListContains();

			Finish();
		}

		public void DoesArrayListContains()
		{
			if(!isProxyValid()) return;

			int i = 0;
			foreach(var item in proxy.arrayList)
			{
				string itemName = item.ToString();
				
				//remove any object type inside the item name
				if (itemName.Contains(" ("))
					itemName = itemName.Substring(0, itemName.IndexOf(" ("));

				bool containsName = !contains.Value ? itemName == variableName.Value 
																				     : itemName.Contains(variableName.Value);
				
				if(containsName)
				{
					if (!storeFoundResult.IsNone)
					{
						storeFoundResult.UpdateValue();

						if(storeFoundResult.ObjectType != item.GetType())
						{
							LogError("Found ArrayList item isn't of type " + storeFoundResult.Type + "! Please change the Object type of '" + storeFoundResult.variableName + "' in the Variables tab to " + item.GetType() + ".");
						}

						storeFoundResult.SetValue(item);
					}

					if (!indexOf.IsNone) indexOf.Value = i;

					Fsm.Event(itemFoundEvent);
				}
				i++;
			}

			Fsm.Event(itemNotFoundEvent);
		}
	}
}