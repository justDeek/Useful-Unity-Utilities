using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("An addition to 'DrawFullscreenColor'. Removes the set fullscreen color. You could also just set the alpha in 'DrawFullscreenColor' to 0, but this lets you toggle it more easily.")]
	public class RemoveFullscreenColor : FsmStateAction
	{
		[Tooltip("Flag for removing the fullscreen color or any previously set one. Sets the GUI-color alpha to 0.")]
		public FsmBool remove;

		public override void Reset()
		{
			remove = true;
		}

		public override void OnGUI()
		{
			var guiColor = GUI.color;
			if(remove.Value)
			guiColor.a = 0;
		}
	}
}